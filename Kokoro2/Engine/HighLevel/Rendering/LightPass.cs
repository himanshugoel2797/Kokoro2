using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro2.Math;
using Kokoro2.Engine;
using Kokoro2.Debug;
using Kokoro2.Engine.SceneGraph;
using Kokoro2.Engine.Prefabs;
using Kokoro2.Engine.Shaders;
using Kokoro2.Engine.HighLevel.Cameras;

namespace Kokoro2.Engine.HighLevel.Rendering
{
    public class LightPass
    {

        FrameBuffer lightBuffer;

        public Texture LightData
        {
            get
            {
                return lightBuffer["Light0"];
            }
        }

        public Texture LightColor
        {
            get
            {
                return lightBuffer["Color0"];
            }
        }

        public LightPass()
        {

        }

    }
}
