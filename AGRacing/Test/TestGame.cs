using Kokoro2.Engine.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine;

namespace AGRacing.Test
{
    internal class TestGame : IScene
    {
        SceneManager man;

        public TestGame(GraphicsContext c)
        {
            man = new SceneManager(c);
            man.Add("TrackLoadTest", new TrackLoadTest.Test());
            man.Activate("TrackLoadTest");
        }

        public IScene Parent
        {
            get;
            set;
        }

        public void LoadResources(GraphicsContext context)
        {
            man.LoadResources(context);
        }

        public void Render(double interval, GraphicsContext context)
        {
            man.Render(interval, context);
        }

        public void Update(double interval, GraphicsContext context)
        {
            man.Update(interval, context);
        }
    }
}
