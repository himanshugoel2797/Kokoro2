﻿#if OPENGL && PC

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine.Shaders;
using Kokoro2.Math;
using Kokoro2.Engine;
using OpenTK.Graphics.OpenGL4;

namespace Kokoro2.OpenGL.PC
{
    public class ShaderProgramLL : IEngineObject
    {
        private static Dictionary<string, ulong> programDB = new Dictionary<string, ulong>();
        private string shaderName;
        private Shader[] shaderStages;
        private bool transformSet = false;

        private ShaderProgramLL(GraphicsContext c)
        {
            ParentContext = c;
            variables = new Dictionary<string, shaderVars>();
            shaderStages = new Shader[6];
            shaderName = "";
        }

        public ShaderProgramLL(GraphicsContext c, params Shader[] shaders) : this(c)
        {
            int id = GL.CreateProgram();
            ID = c.EngineObjects.RegisterObject(id);

            for (int i = 0; i < shaders.Length; i++)
            {
                if (shaders[i].GetShaderType() != ShaderTypes.TessellationComb)
                {
                    GL.AttachShader(id, c.EngineObjects[shaders[i].ID, shaders[i].GetType()]);
                }
                else
                {
                    Shader a = (shaders[i] as TessellationShader).eval;
                    Shader b = (shaders[i] as TessellationShader).control;

                    GL.AttachShader(id, c.EngineObjects[a.ID, shaders[i].GetType()]);
                    GL.AttachShader(id, c.EngineObjects[b.ID, shaders[i].GetType()]);
                }
            }

            GL.LinkProgram(id);

            int status = 0;
            GL.GetProgram(id, GetProgramParameterName.LinkStatus, out status);
            GL.ValidateProgram(id);
            if (status == 0)
            {
                //Retrieve the errors
                string error = "";
                GL.GetProgramInfoLog(id, out error);

                GL.DeleteProgram(id);

                Console.WriteLine(error);
                throw new Exception("Shader linking error: " + error);
            }

            for (int i = 0; i < shaders.Length; i++)
            {
                if (shaders[i].GetShaderType() != ShaderTypes.TessellationComb)
                {
                    GL.DetachShader(id, c.EngineObjects[shaders[i].ID, shaders[i].GetType()]);
                }
                else
                {

                    Shader a = (shaders[i] as TessellationShader).eval;
                    Shader b = (shaders[i] as TessellationShader).control;

                    GL.DetachShader(id, c.EngineObjects[a.ID, shaders[i].GetType()]);
                    GL.DetachShader(id, c.EngineObjects[b.ID, shaders[i].GetType()]);
                }
            }

            c.Disposing += Dispose;
        }

        public ShaderProgramLL(GraphicsContext c, string[] transformVars, params Shader[] shaders) : this(c)
        {
            int id = GL.CreateProgram();
            ID = c.EngineObjects.RegisterObject(id);

            for (int i = 0; i < shaders.Length; i++)
            {
                if (shaders[i].GetShaderType() != ShaderTypes.TessellationComb)
                {
                    GL.AttachShader(id, c.EngineObjects[shaders[i].ID, shaders[i].GetType()]);
                }
                else
                {
                    Shader a = (shaders[i] as TessellationShader).eval;
                    Shader b = (shaders[i] as TessellationShader).control;

                    GL.AttachShader(id, c.EngineObjects[a.ID, shaders[i].GetType()]);
                    GL.AttachShader(id, c.EngineObjects[b.ID, shaders[i].GetType()]);
                }
            }

            GL.TransformFeedbackVaryings(id, transformVars.Length, transformVars, TransformFeedbackMode.SeparateAttribs);
            GL.LinkProgram(id);

            int status = 0;
            GL.GetProgram(id, GetProgramParameterName.LinkStatus, out status);
            GL.ValidateProgram(id);
            if (status == 0)
            {
                //Retrieve the errors
                string error = "";
                GL.GetProgramInfoLog(id, out error);

                GL.DeleteProgram(id);

                Console.WriteLine(error);
                throw new Exception("Shader linking error: " + error);
            }

            for (int i = 0; i < shaders.Length; i++)
            {
                if (shaders[i].GetShaderType() != ShaderTypes.TessellationComb)
                {
                    GL.DetachShader(id, c.EngineObjects[shaders[i].ID, shaders[i].GetType()]);
                }
                else
                {

                    Shader a = (shaders[i] as TessellationShader).eval;
                    Shader b = (shaders[i] as TessellationShader).control;

                    GL.DetachShader(id, c.EngineObjects[a.ID, shaders[i].GetType()]);
                    GL.DetachShader(id, c.EngineObjects[b.ID, shaders[i].GetType()]);
                }
            }

            c.Disposing += Dispose;
        }

        internal bool canBindToCompute()
        {
            return shaderStages[(int)ShaderTypes.Compute] != null;
        }

        internal bool canBindToRender()
        {
            return shaderStages[(int)ShaderTypes.Vertex] != null && shaderStages[(int)ShaderTypes.Fragment] != null;
        }


        #region Shader Handler
        private enum VarType
        {
            Matrix4, Matrix3, Matrix2, Vector4, Vector3, Vector2, Float, Texture
        }
        private struct shaderVars
        {
            public int metadata;
            public int pos;
            public object obj;
            public VarType type;
            public bool dirty;
        }
        //Using a dictionary with the name as the key helps prevent unnecessary entries in the variables list, old entries get overwritten automatically
        private Dictionary<string, shaderVars> variables;
        /// <summary>
        /// Set the shader and its parameters, should be called right before rendering to prevent any bugs
        /// </summary>
        /// <param name="context"></param>
        protected virtual void sApply(GraphicsContext context)
        {
            var variables = this.variables.Values.ToList();
            int id = ParentContext.EngineObjects[ID, this.GetType()];

            for (int i = 0; i < variables.Count; i++)
            {
                if (!variables[i].dirty | variables[i].pos == -1) continue;
                else variables[i] = new shaderVars()
                {
                    metadata = variables[i].metadata,
                    obj = variables[i].obj,
                    pos = variables[i].pos,
                    type = variables[i].type,
                    dirty = false
                };

                switch (variables[i].type)
                {
                    case VarType.Float:
                        GL.ProgramUniform1(id, variables[i].pos, (float)variables[i].obj);
                        break;
                    case VarType.Matrix2:
                        var mat2 = (Matrix2)variables[i].obj;
                        GL.ProgramUniformMatrix2(id, variables[i].pos, 1, false, (float[])mat2);
                        break;
                    case VarType.Matrix3:
                        var mat3 = (Matrix3)variables[i].obj;
                        GL.ProgramUniformMatrix3(id, variables[i].pos, 1, false, (float[])mat3);
                        break;
                    case VarType.Matrix4:
                        var mat4 = (Matrix4)variables[i].obj;
                        GL.ProgramUniformMatrix4(id, variables[i].pos, 1, false, (float[])mat4);
                        break;
                    case VarType.Texture:
                        (variables[i].obj as Texture).Bind(variables[i].metadata);
                        GL.ProgramUniform1(id, variables[i].pos, variables[i].metadata);
                        break;
                    case VarType.Vector2:
                        Vector2 tmpA = (Vector2)variables[i].obj;
                        GL.ProgramUniform2(id, variables[i].pos, 1, new float[] { tmpA.X, tmpA.Y });
                        break;
                    case VarType.Vector3:
                        Vector3 tmpB = (Vector3)variables[i].obj;
                        GL.ProgramUniform3(id, variables[i].pos, 1, new float[] { tmpB.X, tmpB.Y, tmpB.Z });
                        break;
                    case VarType.Vector4:
                        Vector4 tmpC = (Vector4)variables[i].obj;
                        GL.ProgramUniform4(id, variables[i].pos, 1, new float[] { tmpC.X, tmpC.Y, tmpC.Z, tmpC.W });
                        break;
                }
            }

            GL.UseProgram(id);

        }

        /// <summary>
        /// Clean up after the shader has been used (Apply + Draw)
        /// </summary>
        /// <param name="context">The current GraphicsContext</param>
        protected virtual void sCleanup(GraphicsContext context)
        {
            var variables = this.variables.Values.ToList();

            for (int i = 0; i < variables.Count; i++)
            {
                switch (variables[i].type)
                {
                    case VarType.Texture:
                        //GL.ProgramUniform1(context.EngineObjects[ID, this.GetType()], variables[i].pos, 0);
                        if (variables[i].pos != -1) (variables[i].obj as Texture)?.UnBind(variables[i].metadata);
                        break;
                    default:
                        break;
                }
            }

            GL.UseProgram(0);
        }
        #endregion

        #region Shader Variables
        private int GetUniformLocation(int id, string name)
        {
            if (variables.ContainsKey(name)) return variables[name].pos;
            else return GL.GetUniformLocation(id, name);
        }


        protected void aSetShaderBool(string name, bool val)
        {
            aSetShaderFloat(name, val ? 1 : 0);
        }
        protected void aSetShaderMatrix(string name, Matrix4 val)
        {
            if (variables.ContainsKey(name) && (Matrix4)variables[name].obj == val) return;

            variables[name] = new shaderVars()
            {
                obj = val,
                pos = GetUniformLocation(ParentContext.EngineObjects[ID, this.GetType()], name),
                type = VarType.Matrix4,
                dirty = true
            };
        }

        protected void aSetShaderMatrix(string name, Matrix2 val)
        {
            if (variables.ContainsKey(name) && val.Equals((Matrix2)variables[name].obj)) return;

            variables[name] = new shaderVars()
            {
                obj = val,
                pos = GetUniformLocation(ParentContext.EngineObjects[ID, this.GetType()], name),
                type = VarType.Matrix2,
                dirty = true
            };
        }

        protected void aSetShaderMatrix(string name, Matrix3 val)
        {
            if (variables.ContainsKey(name) && val.Equals((Matrix3)variables[name].obj)) return;

            variables[name] = new shaderVars()
            {
                obj = val,
                pos = GetUniformLocation(ParentContext.EngineObjects[ID, this.GetType()], name),
                type = VarType.Matrix3,
                dirty = true
            };
        }

        protected void aSetShaderVector(string name, Vector4 val)
        {
            if (variables.ContainsKey(name) && (Vector4)variables[name].obj == val) return;

            variables[name] = new shaderVars()
            {
                obj = val,
                pos = GetUniformLocation(ParentContext.EngineObjects[ID, this.GetType()], name),
                type = VarType.Vector4,
                dirty = true
            };
        }

        protected void aSetShaderVector(string name, Vector3 val)
        {
            if (variables.ContainsKey(name) && (Vector3)variables[name].obj == val) return;

            variables[name] = new shaderVars()
            {
                obj = val,
                pos = GetUniformLocation(ParentContext.EngineObjects[ID, this.GetType()], name),
                type = VarType.Vector3,
                dirty = true
            };
        }

        protected void aSetShaderVector(string name, Vector2 val)
        {
            if (variables.ContainsKey(name) && (Vector2)variables[name].obj == val) return;

            variables[name] = new shaderVars()
            {
                obj = val,
                pos = GetUniformLocation(ParentContext.EngineObjects[ID, this.GetType()], name),
                type = VarType.Vector2,
                dirty = true
            };
        }

        protected void aSetShaderFloat(string name, float val)
        {
            if (variables.ContainsKey(name) && (float)variables[name].obj == val) return;

            if (variables.ContainsKey(name) && (float)variables[name].obj == val) return;

            variables[name] = new shaderVars()
            {
                obj = val,
                pos = GetUniformLocation(ParentContext.EngineObjects[ID, this.GetType()], name),
                type = VarType.Float,
                dirty = true
            };
        }

        int texUnit = 0;

        public ulong ID
        {
            get; set;
        }

        public GraphicsContext ParentContext { get; set; }

        protected void aSetTexture(string name, Texture tex)
        {
            if (variables.ContainsKey(name) && (((Texture)variables[name].obj).ID == tex.ID | variables[name].pos == -1)) return;

            if (variables.ContainsKey(name))
            {
                variables[name] = new shaderVars()
                {
                    metadata = variables[name].metadata,
                    obj = tex,
                    pos = variables[name].pos,
                    type = VarType.Texture,
                    dirty = true
                };
            }
            else
            {
                variables[name] = new shaderVars()
                {
                    metadata = texUnit++,
                    obj = tex,
                    pos = GetUniformLocation(ParentContext.EngineObjects[ID, this.GetType()], name),
                    type = VarType.Texture,
                    dirty = true
                };
            }
        }
        #endregion


        public void Dispose()
        {
            // throw new NotImplementedException();
        }
    }
}

#endif