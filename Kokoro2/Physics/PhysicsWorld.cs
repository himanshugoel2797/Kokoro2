using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics;
using Kokoro2.Math;
using BEPUphysics.BroadPhaseEntries;

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

        public bool RayCast(Vector3 o, Vector3 d, out float distance, out Vector3 normal)
        {
            RayCastResult res;
            bool intersection = world.RayCast(new BEPUutilities.Ray(o, d), out res);

            normal = res.HitData.Normal;
            distance = res.HitData.T;

            return intersection;
        }

        public bool RayCast(Vector3 o, Vector3 d, out float distance, out Vector3 normal, out BroadPhaseEntry target)
        {
            RayCastResult res;
            bool intersection = world.RayCast(new BEPUutilities.Ray(o, d), out res);

            normal = res.HitData.Normal;
            distance = res.HitData.T;
            target = res.HitObject;

            return intersection;
        }

        public void Update(double interval)
        {
            world.Update();
        }

        public void AddEntity<T>(BEPUBody<T> b) where T : BEPUphysics.Entities.Entity
        {
            world.Add(b.body);
        }

        public void AddEntity(ISpaceObject m)
        {
            world.Add(m);
        }

    }
}
