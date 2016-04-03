using Kokoro2.Engine.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine;

namespace Kokoro2.IDE.Editor
{
    public class EditorUIManager : IScene
    {
        public IScene Parent
        {
            get;
            set;
        }

        private System.Diagnostics.Stopwatch frameTimer;
        private int rFrames = 0, uFrames = 0;
        public int RenderRate { get; private set; }
        public int UpdateRate { get; private set; }

        private bool ResourcesLoaded;

        public void LoadResources(GraphicsContext context)
        {
            if (!ResourcesLoaded)
            {
                frameTimer = new System.Diagnostics.Stopwatch();
                frameTimer.Start();

                ResourcesLoaded = true;
            }
        }

        public void Render(double interval, GraphicsContext context)
        {
            if (ResourcesLoaded)
            {
                rFrames++;
                context.ClearColor(0, 0.5f, 1.0f, 0.0f);
                context.ClearDepth();

                context.SwapBuffers();
            }
        }

        public void Update(double interval, GraphicsContext context)
        {
            if (ResourcesLoaded)
            {
                uFrames++;
                if (frameTimer.ElapsedMilliseconds >= 1000)
                {
                    UpdateRate = uFrames;
                    RenderRate = rFrames;
                    uFrames = rFrames = 0;
                    frameTimer.Restart();
                }
            }
        }
    }
}
