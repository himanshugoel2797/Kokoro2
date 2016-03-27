using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Physics.Prefabs
{
    public class Triangle : PhysicsBody
    {
        public Vector3 Corner0 { get; set; }
        public Vector3 Corner1 { get; set; }
        public Vector3 Corner2 { get; set; }

        public Triangle(Vector3 position, Vector3 p0, Vector3 p1, Vector3 p2) : base()
        {
            Corner0 = p0;
            Corner1 = p1;
            Corner2 = p2;
            State.Position = position;

            float rad = System.Math.Max(Corner0.LengthSquared, System.Math.Max(Corner1.LengthSquared, Corner2.LengthSquared));
            broadphase.Radius = (float)System.Math.Sqrt(rad);

        }
    }
}
