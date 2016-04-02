using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Kokoro2.Engine.Shaders
{
    public class GeometryShader : Shader
    {
        public GeometryShader(string fshader, GraphicsContext c) : base(fshader, ShaderTypes.Geometry, c) { }

        public static GeometryShader Load(string dir, GraphicsContext c)
        {
            return new GeometryShader(Shader.GetFile(dir + "/geometry.glsl"), c);
        }
    }
}
