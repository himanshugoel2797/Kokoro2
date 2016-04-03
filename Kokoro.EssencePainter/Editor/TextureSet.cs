using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro.EssencePainter.Editor
{
    public struct TextureSet
    {
        public string Name { get; set; }
        public string File { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public bool AlbedoMap { get; set; }
        public bool ReflectivityMap { get; set; }
        public bool RoughnessMap { get; set; }
        public bool NormalMap { get; set; }
    }
}
