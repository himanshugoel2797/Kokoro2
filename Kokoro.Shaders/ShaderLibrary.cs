using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Shaders
{
    public class ShaderLibrary
    {
        public static string[] LoadShaders()
        {
            return new string[]
            {
                "GBuffer/fragment.glsl",
                "GBuffer/vertex.glsl",
                "PBR/fragment.glsl",
                "Default/fragment.glsl",
                "Default/vertex.glsl",
                "FrameBuffer/fragment.glsl",
                "FrameBuffer/vertex.glsl",
                "LoD/fragment.glsl",
                "LoD/vertex.glsl",
                "LoD/tessEval.glsl",
                "LoD/tessControl.glsl",
                "BlurHorizontal/fragment.glsl",
                "BlurVertical/fragment.glsl",
                "ShadowMap/vertex.glsl",
                "ShadowMap/fragment.glsl",
                "Shadowed/vertex.glsl",
                "Shadowed/fragment.glsl",
                "DirectionalLight/vertex.glsl",
                "DirectionalLight/fragment.glsl",
                "PointLight/vertex.glsl",
                "PointLight/fragment.glsl",
                "RSM/vertex.glsl",
                "RSM/fragment.glsl",
                "LightShadowBloom/vertex.glsl",
                "LightShadowBloom/fragment.glsl"
            };
        }

        public static string LoadFile(string name)
        {
            Assembly _assembly;
            StreamReader _textStreamReader;
            string res = "";

            try
            {
                _assembly = Assembly.GetExecutingAssembly();
                _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("Kokoro2.Shaders." + name.Replace("/", ".")));
                res = _textStreamReader.ReadToEnd();
                _textStreamReader.Dispose();
            }
            catch
            {
                Console.WriteLine("Error accessing resources!");
            }
            return res;
        }
    }
}
