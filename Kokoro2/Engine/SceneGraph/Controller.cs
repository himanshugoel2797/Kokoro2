using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.SceneGraph
{
    public abstract class Controller
    {
        public abstract void Update(Entity e, GraphicsContext context, double interval);
    }
}
