#if OPENGL
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public partial class Shader : IDisposable
    {
        internal ShaderType shaderType;
        internal int ID;

        /// <summary>
        /// Create a new Shader object
        /// </summary>
        /// <param name="shaderString">The shader source code</param>
        /// <param name="shaderType">The type of shader to compile</param>
        public Shader(string shaderString, Engine.ShaderTypes shaderType)
        {
            this.shaderType = EnumConverters.EShaderTypes(shaderType);

            ID = GL.CreateShader(this.shaderType);
            GL.ShaderSource(ID, shaderString);
            GL.CompileShader(ID);

            int result = 0;
            GL.GetShader(ID, ShaderParameter.CompileStatus, out result);
            if (result != 1)
            {
                throw new Exception("Shader Compilation Failed!", new Exception(GL.GetShaderInfoLog(ID)));  //If compile failed, throw an exception
            }
        }

        /// <summary>
        /// Free the resources used by this object
        /// </summary>
        public void Dispose()
        {
            GL.DeleteShader(ID);
            ID = 0;
            GC.SuppressFinalize(this);
        }

        ~Shader()
        {
            System.Diagnostics.Debug.WriteLine($"[WARN] The shader object {ID} was automatically disposed");
            Dispose();
        }
    }
}
#endif