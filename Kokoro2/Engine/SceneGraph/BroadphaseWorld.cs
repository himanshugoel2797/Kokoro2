using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine.HighLevel;

namespace Kokoro2.Engine.SceneGraph
{
    public class BroadPhaseWorld
    {
        //Check all children narrow phase worlds for dynamic objects, only ones which have dynamic objects can have things which need simulating
        //Then check if any are marked dirty, which means a body in the world was modified
        //Check if any of the dynamic worlds have any objects which haven't settled down yet
        //If none of this happened, we only need to check worlds with dynamic objects which haven't settled down
        //For a narrow phase world, check all dynamic objects for OBV collisions and then resolve them
        //Call all related events, update all connected objects - update draw position when the associated physics body moves, raise collision events

        NarrowPhaseWorld[, ,] Children;
        float cellSide, worldSize;
        int cellCount;

        public int DrawDistance = 10;
        /// <summary>
        /// The number of World units per meter
        /// </summary>
        public float MeterToWorld = 0.05f;
        public event Action<Entity, Entity> CollisionDetected;

        public BroadPhaseWorld(float worldSize, int cells)
        {
            Children = new NarrowPhaseWorld[cells, cells, cells];
            cellSide = worldSize / cells;
            this.worldSize = worldSize;
            cellCount = cells;
            DrawDistance = cells / 2;
        }

        public void Add(Entity e)
        {
            int x = (int)(e.Position.X / cellSide) + cellCount / 2;
            int y = (int)(e.Position.Y / cellSide) + cellCount / 2;
            int z = (int)(e.Position.Z / cellSide) + cellCount / 2;

            if (Children[x, y, z] == null)
            {
                Children[x, y, z] = new NarrowPhaseWorld();
                Children[x, y, z].CollisionDetected += CollisionDetected;
            }
            Children[x, y, z].AddEntity(e);
        }

        public void Update(double interval, GraphicsContext context)
        {
            for (int x = 0; x < cellCount; x++)
                for (int y = 0; y < cellCount; y++)
                    for (int z = 0; z < cellCount; z++)
                    {
                        if (Children[x, y, z] != null && Children[x, y, z].HasDynamicObjects) Children[x, y, z].Update(interval);
                    }
        }

        public void Render(double interval, GraphicsContext context)
        {
            //Get collection of all NarrowPhaseWorlds within the specified radius
            for (int x = (int)((context.Camera.Position.X) / cellSide) - DrawDistance + cellCount / 2; x < (int)((context.Camera.Position.X) / cellSide) + DrawDistance + cellCount / 2; x++)
                for (int y = (int)((context.Camera.Position.Y) / cellSide) - DrawDistance + cellCount / 2; y < (int)((context.Camera.Position.Y) / cellSide) + DrawDistance + cellCount / 2; y++)
                    for (int z = (int)((context.Camera.Position.Z) / cellSide) - DrawDistance + cellCount / 2; z < (int)((context.Camera.Position.Z) / cellSide) + DrawDistance + cellCount / 2; z++)
                    {
                        if (x >= 0 && y >= 0 && z >= 0 && Children[x, y, z] != null) Children[x, y, z].Render(interval, context);
                    }

        }

    }
}
