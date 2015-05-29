using System;

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

        public static string LoadVertexShader(string name)
        {

        }

        public static string LoadFragmentShader(string name)
        {

        }
    }
}

