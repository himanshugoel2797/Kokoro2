using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akane.Physics
{
    public class Circle : PhysicsObject
    {
        public Vector2 Center;
        public float Radius;

        public override AABB GenerateAABB()
        {
            Vector2 min = new Vector2(Center.X - Radius, Center.Y - Radius);
            Vector2 max = new Vector2(Center.X + Radius, Center.Y + Radius);

            return new AABB()
            {
                Max = max,
                Min = min
            };
        }
    }
}
