using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akane.Physics
{
    public class PhysicsWorld
    {
        public List<PhysicsObject> WorldObjects;

        private CollisionResolvers.AABBSolver AABBSolver;
        private CollisionResolvers.CircleSolver CircleSolver;

        public PhysicsWorld()
        {
            WorldObjects = new List<PhysicsObject>();
        }

        public void Update(double interval)
        {
            //iterate through all registered world objects, predicting collisions
            
        }

    }
}
