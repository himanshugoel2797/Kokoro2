using Kokoro2.Engine;
using Kokoro2.Engine.SceneGraph;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Game
{
    public class Game
    {
        public Game(GraphicsContext context)
        {
            context.Initialize += Initialize;
            context.Start(160000, 160000);
        }

        public SceneManager manager;

        public void Initialize(GraphicsContext context)
        {
            manager = new SceneManager();
            manager.Register(context);
            manager.Add("SphereRenderTest", new SphereRenderTest());
            manager.Activate("SphereRenderTest");
        }

    }
}
