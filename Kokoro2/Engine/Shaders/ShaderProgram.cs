using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Kokoro2.Math;

#if OPENGL
#if PC
using Kokoro2.OpenGL.PC;
#endif
#endif

namespace Kokoro2.Engine.Shaders
{
    /// <summary>
    /// A Program consisting of shader stages
    /// </summary>
    public class ShaderProgram : ShaderProgramLL, IDisposable
    {
        /// <summary>
        /// Called before the application of the shader program
        /// </summary>
        public Action<GraphicsContext, ShaderProgram> PreApply { get; set; }

        /// <summary>
        /// Create a new instance of a ShaderProgram using the platform specific language
        /// </summary>
        /// <param name="shaders">The shaders</param>
        public ShaderProgram(params Shader[] shaders) : base(shaders) { }

        /// <summary>
        /// Create a new instance of a ShaderProgram using the platform specific language
        /// </summary>
        /// <param name="shaders">The shaders</param>
        /// <param name="transformVars">The transform feedback attributes</param>
        public ShaderProgram(string[] transformVars, params Shader[] shaders) : base(shaders, transformVars) { }


        /// <summary>
        /// Apply the shader program
        /// </summary>
        /// <param name="context">The current GraphicsContext</param>
        public void Apply(GraphicsContext context)
        {
            if (PreApply != null) PreApply(context, this);
            base.sApply(context);
        }

        /// <summary>
        /// Clean up after applying the shader program
        /// </summary>
        /// <param name="context">The current GraphicsContext</param>
        public void Cleanup(GraphicsContext context)
        {
            base.sCleanup(context);
        }

        /// <summary>
        /// Set the value of a shader uniform
        /// </summary>
        /// <param name="name">The uniform name</param>
        /// <returns>The value of the uniform</returns>
        public object this[string name]
        {
            set
            {
                Type t = value.GetType();

                if (t == typeof(bool)) SetShaderBool(name, (bool)value);
                else if (t == typeof(Matrix4)) SetShaderMatrix(name, (Matrix4)value);
                else if (t == typeof(Matrix3)) SetShaderMatrix(name, (Matrix3)value);
                else if (t == typeof(Matrix2)) SetShaderMatrix(name, (Matrix2)value);
                else if (t == typeof(Vector4)) SetShaderVector(name, (Vector4)value);
                else if (t == typeof(Vector3)) SetShaderVector(name, (Vector3)value);
                else if (t == typeof(Vector2)) SetShaderVector(name, (Vector2)value);
                else if (t == typeof(float)) SetShaderFloat(name, (float)value);
                else if (t == typeof(double)) SetShaderFloat(name, (float)(double)value);
                else if (t == typeof(int)) SetShaderFloat(name, (float)(int)value);
                else if (t == typeof(Texture)) SetTexture(name, (Texture)value);
                else if (t == typeof(FrameBufferTexture)) SetTexture(name, (Texture)value);
                else if (t == typeof(CubeMapTexture)) SetCubeMapTexture(name, (CubeMapTexture)value);
                else throw new Exception("Unknown type " + name);
            }
        }

        public void SetShaderBool(string name, bool val)
        {
            base.aSetShaderBool(name, val);
        }

        public void SetShaderMatrix(string name, Matrix4 val)
        {
            base.aSetShaderMatrix(name, val);
        }

        public void SetShaderMatrix(string name, Matrix2 val)
        {
            base.aSetShaderMatrix(name, val);
        }

        public void SetShaderMatrix(string name, Matrix3 val)
        {
            base.aSetShaderMatrix(name, val);
        }

        public void SetShaderVector(string name, Vector4 val)
        {
            base.aSetShaderVector(name, val);
        }

        public void SetShaderVector(string name, Vector3 val)
        {
            base.aSetShaderVector(name, val);
        }

        public void SetShaderVector(string name, Vector2 val)
        {
            base.aSetShaderVector(name, val);
        }

        public void SetShaderFloat(string name, float val)
        {
            base.aSetShaderFloat(name, val);
        }

        public void SetTexture(string name, Texture tex)
        {
            base.aSetTexture(name, tex);
        }

        public void SetCubeMapTexture(string name, CubeMapTexture tex)
        {
            base.aSetCubeMapTexture(name, tex);
        }
    }

}
