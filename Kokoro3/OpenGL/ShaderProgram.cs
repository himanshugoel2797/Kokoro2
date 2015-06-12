#if OPENGL
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public class ShaderProgram : IDisposable
    {
        internal int ID;
        List<Shader> shaders;

        public ShaderProgram()
        {
            ID = GL.CreateProgram();
            shaders = new List<Shader>();
        }

        public void Attach(Shader s)
        {
            GL.AttachShader(ID, s.ID);
            shaders.Add(s);
        }

        public void Link()
        {
            GL.LinkProgram(ID);

            int result = 0;
            GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out result);
            if (result != 1)
            {
                throw new Exception("Program Link Failed!", new Exception(GL.GetProgramInfoLog(ID)));  //If compile failed, throw an exception
            }
        }

        public void Detach(Shader s)
        {
            GL.DetachShader(ID, s.ID);
        }

        public void DetachAll()
        {
            for (int i = 0; i < shaders.Count; i++)
            {
                Detach(shaders[i]);
            }
        }

        public void CacheBinary(string file)
        {
            int result = 0;
            GL.GetProgram(ID, GetProgramParameterName.ProgramBinaryRetrievableHint, out result);

            if (result != 0)
            {
                //TODO submit bug fix in OpenTK and update binaries used so this can actually be used
                GL.GetProgram(ID, GetProgramParameterName.ProgramBinaryLength, out result);

                int outLength = 0;
                BinaryFormat bFormat;
                byte[] outData = new byte[result];

                GL.GetProgramBinary(ID, result, out outLength, out bFormat, outData);
                if (outLength > 0) System.IO.File.WriteAllBytes(file, outData);
                else throw new Exception("Failed to retrieve program binary");
            }

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
#endif