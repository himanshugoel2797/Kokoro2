using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akane.Physics
{
    public abstract class PhysicsObject
    {
        private float invMass;

        public float Restitution;

        public float InverseMass
        {
            get
            {
                return invMass;
            }
            set
            {
                if (value != 0) invMass = 1f / value;
                else invMass = 0;
            }
        }

        public Vector2 MotionNormal;
        public float Velocity;

        public abstract AABB GenerateAABB();

    }
}
