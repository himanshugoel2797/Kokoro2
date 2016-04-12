using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro.Editor.Shaders
{
    public class ShaderLibrary
    {

        public static string[] LoadShaders()
        {
            return new string[]
            {
                "UVTex/vertex.glsl",
                "UVTex/fragment.glsl"
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
                _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("Kokoro.Editor.Shaders." + name.Replace("/", ".")));
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
