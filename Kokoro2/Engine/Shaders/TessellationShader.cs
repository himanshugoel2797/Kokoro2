using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Kokoro2.Engine.Shaders
{
    public class TessellationShader : Shader
    {
        internal TessellationEvalShader eval;
        internal TessellationControlShader control;
        
        public TessellationShader(string controlShader, string evalShader, GraphicsContext c) : base("", ShaderTypes.TessellationComb, c)
        {
            eval = new TessellationEvalShader(evalShader, c);
            control = new TessellationControlShader(controlShader, c);
        }

        public static TessellationShader Load(string controlShaderName, string evalShaderName, GraphicsContext c)
        {
            return new TessellationShader(Shader.GetFile(controlShaderName + "/tessControl.glsl"), Shader.GetFile(evalShaderName + "/tessEval.glsl"), c);
        }
    }

    class TessellationControlShader : Shader
    {
        public TessellationControlShader(string fshader, GraphicsContext c) : base(fshader, ShaderTypes.TessellationControl, c) { }

    }

    class TessellationEvalShader : Shader
    {
        public TessellationEvalShader(string fshader, GraphicsContext c) : base(fshader, ShaderTypes.TessellationEval, c) { }
    }
}
