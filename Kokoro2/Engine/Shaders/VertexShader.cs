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
        public VertexShader(string fshader, GraphicsContext c) : base(fshader, ShaderTypes.Vertex, c) { }

        public static VertexShader Load(string dir, GraphicsContext c)
        {
            return new VertexShader(Shader.GetFile(dir + "/vertex.glsl"), c);
        }
    }
}
