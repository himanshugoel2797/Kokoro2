using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Rendering
{
    internal class GI_IntersectionTests
    {
        internal static bool intersect(BoundingBox b, Vector3 ro, Vector3 rd)
        {
            Vector3 invdir = new Vector3(1f / rd.X, 1f / rd.Y, 1 / rd.Z);

            double tmin, tmax;
            
            double tx1 = (b.Min.X - ro.X) * invdir.X;
            double ty1 = (b.Min.Y - ro.Y) * invdir.Y;
            double tz1 = (b.Min.Z - ro.Z) * invdir.Z;


            double tx2 = (b.Max.X - ro.X) * invdir.X;
            double ty2 = (b.Max.Y - ro.Y) * invdir.Y;
            double tz2 = (b.Max.Z - ro.Z) * invdir.Z;

            double txmin = System.Math.Min(tx1, tx2);
            double txmax = System.Math.Max(tx1, tx2);

            double tymin = System.Math.Min(ty1, ty2);
            double tymax = System.Math.Max(ty1, ty2);

            double tzmin = System.Math.Min(tz1, tz2);
            double tzmax = System.Math.Max(tz1, tz2);

            tmin = System.Math.Max(txmin, System.Math.Max(tymin, tzmin));
            tmax = System.Math.Min(txmax, System.Math.Min(tymax, tzmax));

            return (tmax < tmin && tmax >= 0);
        }

        internal static bool IsIntersecting(BoundingBox box, Vector3 a, Vector3 b, Vector3 c, Vector3 n)
        {
            double triangleMin, triangleMax;
            double boxMin, boxMax;

            Vector3[] boxVertices = new Vector3[] { box.Min, box.Max };
            Vector3[] triVertices = new Vector3[] { a, b, c };

            // Test the box normals (x-, y- and z-axes)
            var boxNormals = new Vector3[] {
                new Vector3(1,0,0),
                new Vector3(0,1,0),
                new Vector3(0,0,1)
            };

            for (int i = 0; i < 3; i++)
            {
                Project(triVertices, boxNormals[i], out triangleMin, out triangleMax);
                if (triangleMax < box.Min[i] || triangleMin > box.Max[i])
                    return false; // No intersection possible.
            }

            // Test the triangle normal
            double triangleOffset = Vector3.Dot(n, a);
            Project(boxVertices, n, out boxMin, out boxMax);
            if (boxMax < triangleOffset || boxMin > triangleOffset)
                return false; // No intersection possible.

            // Test the nine edge cross-products
            Vector3[] triangleEdges = new Vector3[] {
                a - b,
                b - c,
                c - a
            };

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    // The box normals are the same as it's edge tangents
                    Vector3 axis = Vector3.Cross(triangleEdges[i], boxNormals[j]);
                    Project(boxVertices, axis, out boxMin, out boxMax);
                    Project(triVertices, axis, out triangleMin, out triangleMax);
                    if (boxMax < triangleMin || boxMin > triangleMax)
                        return false; // No intersection possible
                }

            // No separating axis found.
            return true;
        }

        static void Project(IEnumerable<Vector3> points, Vector3 axis,
                out double min, out double max)
        {
            min = double.PositiveInfinity;
            max = double.NegativeInfinity;
            foreach (var p in points)
            {
                double val = Vector3.Dot(axis, p);
                if (val < min) min = val;
                if (val > max) max = val;
            }
        }
    }
}
