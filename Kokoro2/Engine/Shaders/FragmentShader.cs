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
        public FragmentShader(string fshader) : base(fshader, ShaderTypes.Fragment) { }

        public static FragmentShader Load(string dir)
        {
            return new FragmentShader(new StreamReader(VFS.FSReader.OpenFile(dir + "/fragment.glsl")).ReadToEnd());
        }
    }
}
