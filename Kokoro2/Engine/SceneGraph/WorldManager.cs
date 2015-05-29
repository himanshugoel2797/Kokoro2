using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.SceneGraph
{
    public class WorldManager
    {
        public Octree<Entity> GameWorld;

        public WorldManager(int worldSize, int depth)
        {
            GameWorld = new Octree<Entity>(worldSize, 0, depth);
        }
    }
}
