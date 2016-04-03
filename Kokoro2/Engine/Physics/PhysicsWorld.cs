using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics;
using Kokoro2.Math;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.Settings;
using BEPUutilities.Threading;

namespace Kokoro2.Engine.Physics
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

        public bool RayCast(Vector3 o, Vector3 d, out float[] distance, out Vector3[] normal, out ulong[] target)
        {
            List<RayCastResult> res = new List<RayCastResult>();
            bool intersection = world.RayCast(new BEPUutilities.Ray(o, d), 1000f, res);

            normal = new Vector3[res.Count];
            distance = new float[res.Count];
            target = new ulong[res.Count];
            for (int i = 0; i < res.Count; i++)
            {
                normal[i] = res[i].HitData.Normal;
                distance[i] = res[i].HitData.T;
                target[i] = (ulong)res[i].HitObject.Tag;
            }

            return intersection;
        }

        public void Update(double interval)
        {
            world.Update((float)interval);
        }

        public void AddEntity(BaseEntity b)
        {
            b.ParentSpace = this;
            world.Add(b.physEntity);
        }

    }
}
