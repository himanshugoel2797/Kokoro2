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
    }
}

