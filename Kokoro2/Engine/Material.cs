using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Kokoro2.Math;
using Kokoro2.Engine.Shaders;

namespace Kokoro2.Engine
{
    public class Material
    {
        public string Name { get; set; }
        public Texture AlbedoMap { get; set; }
        public Texture SpecularMap { get; set; }
        public Texture GlossinessMap { get; set; }
    }
}
