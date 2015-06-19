#if OPENGL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public static class TextureSlotManager
    {
        static Dictionary<int, int> VirtualSlots = new Dictionary<int, int>();
        static Dictionary<int, int> RealSlots = new Dictionary<int, int>();

        public const int VirtualSlotCount = 512;

        static TextureSlotManager()
        {

        }

        public static void PlaceTextures(params Texture[] textures)
        {

        }
    }
}
#endif