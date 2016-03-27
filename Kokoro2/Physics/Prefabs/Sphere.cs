using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Physics.Prefabs
{
    public class Sphere : PhysicsBody
    {
        public float Radius { get; set; }
        private const float HalfRoot3 = 0.86602540378f; //Sqrt(3)/2
        private const float InvHalfRoot3 = 1 / HalfRoot3;

        public Sphere(Vector3 pos, float radius) : base()
        {
            State.Position = pos;
            Radius = radius;
            broadphase.Radius = radius;
        }

        internal override SphereOctree Voxelize()
        {
            SphereOctree oct = new SphereOctree(Radius * InvHalfRoot3, 1);
            oct.Add(Vector3.Zero);
            return oct;
        }
    }
}
