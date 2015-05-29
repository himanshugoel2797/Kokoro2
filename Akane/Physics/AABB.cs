using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akane.Physics
{
    public class AABB : PhysicsObject
    {
        public Vector2 Min, Max;

        public override AABB GenerateAABB()
        {
            return this;
        }
    }
}
