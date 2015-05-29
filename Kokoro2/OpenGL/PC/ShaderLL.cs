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
    public class ShaderLL : IDisposable
    {
        public ShaderLL()
        {

        }

        protected Engine.Shaders.ShaderTypes shaderType;
        protected int id;
        protected int pGetID()
        {
            return id;
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
            id = GL.CreateShader(EnumConverters.EShaderTypes(type));
            GL.ShaderSource(id, file);
            GL.CompileShader(id);
            return id;
        }

        protected void CheckForErrors(string fshader, Engine.Shaders.ShaderTypes type)
        {
            int result = 0;
            GL.GetShader(id, ShaderParameter.CompileStatus, out result);
            if (result != 1)
            {
                Kokoro2.Debug.ErrorLogger.AddMessage(id, "RESULT: " + result + "\n" + GL.GetShaderInfoLog(id), Kokoro2.Debug.DebugType.Error, Kokoro2.Debug.Severity.High);
                Kokoro2.Debug.ErrorLogger.AddMessage(id, fshader, Kokoro2.Debug.DebugType.Other, Kokoro2.Debug.Severity.High);
                Kokoro2.Debug.DebuggerManager.logger.Pause = true;
            }
        }

        public void Dispose()
        {
            GL.DeleteShader(id);
        }

    }
}

#endif