using Kokoro2.Engine.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine;
using Kokoro2.Engine.Prefabs;

namespace Sandbox
{
    public class SphereRenderTest : IScene
    {
        public IScene Parent
        {
            get; set;
        }

        public Sphere TestSphereA;
        public bool ResourcesLoaded = false;

        public void LoadResources(GraphicsContext context)
        {
            if (!ResourcesLoaded)
            {
                TestSphereA = new Sphere(1, 50);
                ResourcesLoaded = true;
            }
        }

        public void Render(double interval, GraphicsContext context)
        {
            if(ResourcesLoaded)
            {

                

            }
        }

        public void Update(double interval, GraphicsContext context)
        {

        }
    }
}
