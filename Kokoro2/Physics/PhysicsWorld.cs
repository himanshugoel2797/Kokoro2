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

namespace Kokoro2.Physics
{
    public class PhysicsWorld
    {
        Space world;
        int tagCnt = 1;

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
            var p = new ParallelLooper();
            for (int i = 1; i < Environment.ProcessorCount - 2; i++) p.AddThread();

            world = new Space(p);
            //CollisionResponseSettings.MaximumPenetrationRecoverySpeed = 2;
        }

        public bool RayCast(Vector3 o, Vector3 d, out float distance, out Vector3 normal)
        {
            RayCastResult res;
            bool intersection = world.RayCast(new BEPUutilities.Ray(o, d), out res);

            normal = res.HitData.Normal;
            distance = res.HitData.T;

            return intersection;
        }

        public bool RayCast(Vector3 o, Vector3 d, out float[] distance, out Vector3[] normal, out BroadPhaseEntry[] target)
        {
            List<RayCastResult> res = new List<RayCastResult>();
            bool intersection = world.RayCast(new BEPUutilities.Ray(o, d), 1000f, res);

            normal = new Vector3[res.Count];
            distance = new float[res.Count];
            target = new BroadPhaseEntry[res.Count];
            for (int i = 0; i < res.Count; i++)
            {
                normal[i] = res[i].HitData.Normal;
                distance[i] = res[i].HitData.T;
                target[i] = res[i].HitObject;
            }

            return intersection;
        }

        public void Update(double interval)
        {
            world.Update();
        }

        public void AddEntity<T>(BEPUBody<T> b) where T : BEPUphysics.Entities.Entity
        {
            b.body.Tag = tagCnt++;
            world.Add(b.body);
        }

        public void AddEntity(ISpaceObject m)
        {
            m.Tag = tagCnt++;
            world.Add(m);
        }

    }
}
