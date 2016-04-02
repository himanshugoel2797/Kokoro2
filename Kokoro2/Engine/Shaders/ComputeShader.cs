using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.Shaders
{
    //TODO Compute Shaders don't use the same pipeline as other shaders

    public class ComputeShader : Shader
    {
        public ComputeShader(string fshader, GraphicsContext c) : base(fshader, ShaderTypes.Compute, c) { }

        public static ComputeShader Load(string dir, GraphicsContext c)
        {
            return new ComputeShader(Shader.GetFile(dir + "/compute.glsl"), c);
        }
    }
}
