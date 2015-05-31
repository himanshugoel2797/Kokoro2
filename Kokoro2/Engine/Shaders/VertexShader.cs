using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Kokoro2.Engine.Shaders
{
    public class VertexShader : Shader
    {
        public VertexShader(string fshader) : base(fshader, ShaderTypes.Vertex){ }

        public static VertexShader Load(string dir)
        {
            return new VertexShader(Shader.GetFile(dir + "/vertex.glsl"));
        }
    }
}
