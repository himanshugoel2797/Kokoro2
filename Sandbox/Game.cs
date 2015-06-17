using Kokoro3.Engine;
using Kokoro3.Common;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro.Game
{
    public class Game
    {
        public Game(GameDevice context)
        {
            context.Start(Initialize);
        }

        public StateMachine scenes;

        public void Initialize(GameDevice context)
        {
            scenes = new StateMachine();
            scenes.Initialize(context);
            scenes.Add("SphereRenderTest", new SphereRenderTest(context));
            scenes.Activate("SphereRenderTest");

            context.Update += scenes.Update;
            context.Render += scenes.Render;
        }

    }
}
