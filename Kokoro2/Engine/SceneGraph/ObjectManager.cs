using Kokoro2.Engine.HighLevel.Rendering;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.SceneGraph
{
    public class ObjectManager : Entity
    {
        public Octree<Entity> ObjectTree;

        public Octree<LightSource> Lights;

        //TODO setup a system that maintains a mirror of 'Lights' in a GPU buffer which can then be passed into a compute shader for GI calculation
        //The compute shader performs ray casts onto a spherical grid tree, evaluating the flux at a coarse resolution on each sphere on the same level, each sphere then distributes the flux onto its own children

        public ObjectManager(int worldSize, int depth)
        {
            ObjectTree = new Octree<Entity>(worldSize, depth);
            Lights = new Octree<LightSource>(worldSize, depth);
        }

        #region Entity Octree Management
        public void Add(Entity e)
        {
            e.Parent = this;
            ObjectTree.Add(e, e.Position, e.Radius);
        }

        public void Remove(Entity e)
        {
            ObjectTree.Remove(e);
            e.Parent = null;
        }
        #endregion

        #region Light Octree Management
        public void Add(LightSource e)
        {
            e.Parent = this;
            Lights.Add(e, e.Position, e.Radius);
        }

        public void Remove(LightSource e)
        {
            e.Parent = null;
            Lights.Remove(e);
        }
        #endregion

        public override void Activate(GraphicsContext context, double interval)
        {
            ObjectTree.GetVisibleObjects(context.Camera.Frustum, context.Camera.View, context.Camera.Projection, ContainmentType.Contains);
        }

        public override void Update(GraphicsContext context, double interval)
        {

        }
    }
}
