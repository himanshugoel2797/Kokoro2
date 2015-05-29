using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akane.Physics
{
    public class CollisionResolvers
    {
        public class AABBSolver : BaseCollisionSolver<AABB>
        {
            public override bool Solve(AABB a, AABB b)
            {
                // Exit with no intersection if found separated along an axis
                if (a.Max.X < b.Min.X || a.Min.X > b.Max.X) return false;
                if (a.Max.Y < b.Min.Y || a.Min.Y > b.Max.Y) return false;
                // No separating axis found, therefor there is at least one overlapping axis
                return true;
            }
        }

        public class CircleSolver : BaseCollisionSolver<Circle>
        {
            public static float Distance(Vector2 a, Vector2 b)
            {
                return (float)System.Math.Sqrt(((a.X - b.X) * (a.X - b.X)) + ((a.Y - b.Y) * (a.Y - b.Y)));
            }

            public override bool Solve(Circle a, Circle b)
            {
                float r = a.Radius + b.Radius;
                return r < Distance(a.Center, b.Center);
            }

            public override bool SolveOptimized(Circle a, Circle b)
            {
                float r = a.Radius + b.Radius;
                r *= r;
                return r < ((a.Center.X + b.Center.X) * (a.Center.X + b.Center.X)) + ((a.Center.Y + b.Center.Y) * (a.Center.Y + b.Center.Y));
            }
        }


    }
}
