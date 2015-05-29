using Kokoro2.Engine.SceneGraph;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public abstract class Entity
    {
        public WorldManager Parent;
        public Vector3 Position;
        public uint ID;
        public abstract void Activate(GraphicsContext context, double interval);
        public abstract void Update(GraphicsContext context, double interval);
    }
}
