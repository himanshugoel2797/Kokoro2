using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine.HighLevel;

namespace Kokoro2.Engine.SceneGraph
{
    public class NarrowPhaseWorld
    {
        public bool HasDynamicObjects;
        public bool IsDirty;
        public event Action<Entity, Entity> CollisionDetected;

        internal Dictionary<int, Entity> Entities = new Dictionary<int, Entity>();

        public void AddEntity(Entity e)
        {
            Entities[e.ID] = e;
            if (e.State.Mass > 0) HasDynamicObjects = true;
        }

        public void RemoveEntity(int ID)
        {
            if (Entities.ContainsKey(ID)) Entities.Remove(ID);
        }

        public void Update(double interval)
        {
            //TODO perform collision detection and response here, raise the CollisionDetected event when a collision is detected
            for(int i = 0; i < Entities.Values.Count; i++)
            {

            }
        }

        public void Render(double interval, GraphicsContext context)
        {
            for (int i = 0; i < Entities.Values.Count; i++)
            {
                Entities.Values.ElementAt(i).Render(interval, context);
            }
        }

    }
}
