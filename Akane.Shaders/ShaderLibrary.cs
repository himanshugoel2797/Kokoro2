using System;
using System.IO;
using System.Reflection;

namespace Akane.Shaders
{
    public class ShaderLibrary
    {
        public static string[] LoadShaders()
        {
            return new string[]
            {
                "Shaders/Default",
                "Shaders/FrameBuffer",
                "Shaders/LayerDrawer",
                "Shaders/Sprite",
                "Shaders/TileLayer",
                "Shaders/TransparentColor"
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
                _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("Akane.Shaders." + name.Replace("/", ".")));
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

