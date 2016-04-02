using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics.Entities;
using Kokoro2.Math;

namespace Kokoro2.Engine.Physics
{
    public class Sphere : BaseEntity
    {
        public Sphere(Vector3 pos, float radius, float mass = 0) : base(Process(pos, radius, mass)) { }

        private static BEPUphysics.Entities.Prefabs.Sphere Process(Vector3 pos, float radius, float mass)
        {
            if (mass != 0) return new BEPUphysics.Entities.Prefabs.Sphere(pos, radius, mass);
            else return new BEPUphysics.Entities.Prefabs.Sphere(pos, radius);
        }
    }
}
