#define DEBUG

using Kokoro2.Engine;
using Kokoro2.Engine.SceneGraph;
using AGRacing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if DEBUG
using AGRacing.Test;
#endif

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

#if DEBUG
            manager.Add("TestGame", new TestGame());
            manager.Activate("TestGame");
#endif
        }

    }
}
