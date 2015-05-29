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
        public GeometryShader(string fshader) : base(fshader, ShaderTypes.Geometry) { }

        public static GeometryShader Load(string dir)
        {
            return new GeometryShader(new StreamReader(VFS.FSReader.OpenFile(dir + "/geometry.glsl")).ReadToEnd());
        }
    }
}
