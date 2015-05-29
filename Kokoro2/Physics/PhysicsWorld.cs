using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics;
using Kokoro2.Math;

namespace Kokoro2.Physics
{
    public class PhysicsWorld
    {
        Space world;

        public Vector3 Gravity
        {
            get
            {
                return world.ForceUpdater.Gravity;
            }
            set
            {
                world.ForceUpdater.Gravity = value;
            }
        }

        public PhysicsWorld()
        {
            world = new Space();
        }

        public void Update(double interval)
        {
            world.Update();
        }

        public void AddEntity<T>(BEPUBody<T> b) where T : BEPUphysics.Entities.Entity
        {
            world.Add(b.body);
        }


    }
}
