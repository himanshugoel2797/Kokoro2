#if OPENGL && PC

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine;
using Kokoro2.Math;

using OpenTK.Graphics.OpenGL4;

namespace Kokoro2.OpenGL.PC
{
    public class ShaderLL : IEngineObject
    {
        public ShaderLL(GraphicsContext c)
        {
            ParentContext = c;
        }

        protected Engine.Shaders.ShaderTypes shaderType;

        public ulong ID
        {
            get; set;
        }

        public GraphicsContext ParentContext
        {
            get; set;
        }

        protected Engine.Shaders.ShaderTypes pGetShaderType()
        {
            return shaderType;
        }

        protected void aSetPatchSize(int num)
        {
            GL.PatchParameter(PatchParameterInt.PatchVertices, num);
        }

        protected int aCreate(Engine.Shaders.ShaderTypes type, string file)
        {
            ID = ParentContext.EngineObjects.RegisterObject(GL.CreateShader(EnumConverters.EShaderTypes(type)));

            int id = ParentContext.EngineObjects[ID, this.GetType()];
            GL.ShaderSource(id, file);
            GL.CompileShader(id);
            return id;
        }

        protected void CheckForErrors(string fshader, Engine.Shaders.ShaderTypes type)
        {
            int result = 0;
            GL.GetShader(ParentContext.EngineObjects[ID, this.GetType()], ShaderParameter.CompileStatus, out result);
            if (result != 1)
            {
                Kokoro2.Engine.ErrorLogger.AddMessage(ID, "RESULT: " + result + "\n" + GL.GetShaderInfoLog(ParentContext.EngineObjects[ID, this.GetType()]), Kokoro2.Engine.DebugType.Error, Kokoro2.Engine.Severity.High);
                Kokoro2.Engine.ErrorLogger.AddMessage(ID, fshader, Kokoro2.Engine.DebugType.Other, Kokoro2.Engine.Severity.High);
            }
        }

        public void Dispose()
        {
            if (ID != 0)
            {
                GL.DeleteShader(ParentContext.EngineObjects[ID, this.GetType()]);
                ParentContext.EngineObjects.UnregisterObject(ID);
                ID = 0;
            }
        }

    }
}

#endif