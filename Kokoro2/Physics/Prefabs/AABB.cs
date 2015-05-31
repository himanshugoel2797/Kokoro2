using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Math;

namespace Kokoro2.Physics.Prefabs
{
    public class AABB : ICollisionBody
    {
        public Vector3 Min;
        public Vector3 Max;
        public Vector3 Position;

        public AABB(BoundingBox bound, Vector3 Position)
        {
            this.Min = bound.Min;
            this.Max = bound.Max;
            this.Position = Position;
        }
    }
}
