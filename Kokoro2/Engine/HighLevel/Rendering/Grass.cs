using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro2.Math;
using Kokoro2.Engine;
using Kokoro2.Engine.SceneGraph;
using Kokoro2.Engine.Prefabs;
using Kokoro2.Engine.Shaders;
using Kokoro2.Engine.HighLevel.Cameras;

namespace Kokoro2.Engine.HighLevel.Rendering
{
    public class Grass : Model
    {
        /// <summary>
        /// Load a mesh to grow grass/fur on
        /// </summary>
        /// <param name="filename">The path to the file to load</param>
        public Grass(string filename, GraphicsContext c) : base(c)
        {

        }
    }
}
