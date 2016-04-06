using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Math;

#if OPENGL
#if PC
using Kokoro2.OpenGL.PC;
#endif
#endif

namespace Kokoro2.Engine.Prefabs
{
    /// <summary>
    /// Creates a new Sphere object
    /// </summary>
    public class Sphere : Model
    {
        /// <summary>
        /// Create a new instance of a sphere object
        /// </summary>
        /// <param name="radius">The radius of the sphere</param>
        /// <param name="step">The amount of detail to put in the sphere</param>
        public Sphere(float radius, int step, GraphicsContext c)
            : base(c)
        {

            this.DrawMode = DrawMode.Triangles;

            List<float> verts = new List<float>();
            List<float> uvs = new List<float>();
            List<float> normals = new List<float>();
            List<uint> indices = new List<uint>();

            float angleStep = 360f / (float)step;
            double toRad = MathHelper.Pi / 180;

            float maxX = 0;
            float maxY = 0;
            float maxZ = 0;

            float minX = 0;
            float minY = 0;
            float minZ = 0;

            uint n = 0;
            for (float aY = 0; aY < 180; aY += angleStep)
            {
                for (float aX = 0; aX < 360; aX += angleStep)
                {
                    float x = (float)(radius * System.Math.Cos(aX * toRad) * System.Math.Sin(aY * toRad));
                    float y = (float)(radius * System.Math.Sin(aX * toRad) * System.Math.Sin(aY * toRad));
                    float z = (float)(radius * System.Math.Cos(aY * toRad));

                    if (x > maxX) maxX = x;
                    if (y > maxY) maxY = y;
                    if (z > maxZ) maxZ = z;

                    if (x < minX) minX = x;
                    if (y < minY) minY = y;
                    if (z < minZ) minZ = z;

                    verts.Add(x);
                    verts.Add(y);
                    verts.Add(z);

                    float uvX = aX / 360;
                    float uvY = (2 * aY) / 360;
                    uvs.Add(uvX);
                    uvs.Add(uvY);

                    var normal = new Vector3(x, y, z);
                    normal.Normalize();
                    normals.Add(normal.X);
                    normals.Add(normal.Y);
                    normals.Add(normal.Z);

                    indices.Add(n);
                    n++;


                    x = (float)(radius * System.Math.Cos(aX * toRad) * System.Math.Sin((aY + angleStep) * toRad));
                    y = (float)(radius * System.Math.Sin(aX * toRad) * System.Math.Sin((aY + angleStep) * toRad));
                    z = (float)(radius * System.Math.Cos((aY + angleStep) * toRad));

                    verts.Add(x);
                    verts.Add(y);
                    verts.Add(z);

                    normal = new Vector3(x, y, z);
                    normal.Normalize();
                    normals.Add(normal.X);
                    normals.Add(normal.Y);
                    normals.Add(normal.Z);

                    uvX = aX / 360;
                    uvY = (2 * (aY + angleStep)) / 360;
                    uvs.Add(uvX);
                    uvs.Add(uvY);

                    indices.Add(n);
                    n++;


                    x = (float)(radius * System.Math.Cos((aX + angleStep) * toRad) * System.Math.Sin(aY * toRad));
                    y = (float)(radius * System.Math.Sin((aX + angleStep) * toRad) * System.Math.Sin(aY * toRad));
                    z = (float)(radius * System.Math.Cos(aY * toRad));

                    verts.Add(x);
                    verts.Add(y);
                    verts.Add(z);

                    normal = new Vector3(x, y, z);
                    normal.Normalize();
                    normals.Add(normal.X);
                    normals.Add(normal.Y);
                    normals.Add(normal.Z);

                    uvX = (aX + angleStep) / 360;
                    uvY = (2 * aY) / 360;
                    uvs.Add(uvX);
                    uvs.Add(uvY);

                    indices.Add(n);
                    n++;

                    x = (float)(radius * System.Math.Cos((aX + angleStep) * toRad) * System.Math.Sin((aY + angleStep) * toRad));
                    y = (float)(radius * System.Math.Sin((aX + angleStep) * toRad) * System.Math.Sin((aY + angleStep) * toRad));
                    z = (float)(radius * System.Math.Cos((aY + angleStep) * toRad));

                    verts.Add(x);
                    verts.Add(y);
                    verts.Add(z);

                    normal = new Vector3(x, y, z);
                    normal.Normalize();
                    normals.Add(normal.X);
                    normals.Add(normal.Y);
                    normals.Add(normal.Z);

                    uvX = (aX + angleStep) / 360;
                    uvY = (2 * (aY + angleStep)) / 360;
                    uvs.Add(uvX);
                    uvs.Add(uvY);

                    indices.Add(n);
                    indices.Add((uint)(n - 1));
                    indices.Add((uint)(n - 2));
                    n++;

                }
            }

            Buffer.Bind();
            SetIndices(indices.ToArray(), 0);
            SetUVs(uvs.ToArray(), 0);
            SetVertices(verts.ToArray(), 0);
            SetNormals(normals.ToArray(), 0);
            Bound = new BoundingBox()
            {
                Max = new Vector3(maxX, maxY, maxZ),
                Min = new Vector3(minX, minY, minZ)
            };
            Buffer.UnBind();
        }
    }
}
