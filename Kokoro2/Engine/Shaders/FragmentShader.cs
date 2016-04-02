using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Kokoro2.Engine.Shaders
{
    public class FragmentShader : Shader
    {
        public FragmentShader(string fshader, GraphicsContext c) : base(fshader, ShaderTypes.Fragment, c) { }

        public static FragmentShader Load(string dir, GraphicsContext c)
        {
            return new FragmentShader(Shader.GetFile(dir + "/fragment.glsl"), c);
        }
    }
}
