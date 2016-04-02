using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.Prefabs
{
    /// <summary>
    /// Represents an Axis Aligned Bounding Box
    /// </summary>
    public class OBV : Model
    {
        /// <summary>
        /// Create a new OBV object
        /// </summary>
        /// <param name="box">The bounding box as calculated from another Model</param>
        public OBV(BoundingBox box)
        {
            this.DrawMode = Engine.DrawMode.Lines;

            Vector3 sc = box.Max - box.Min;
            Vector3 tl = (box.Min + box.Max) / 2;

            SetVertices(new float[]{
                -0.5f * sc.X + tl.X, -0.5f * sc.Y + tl.Y, -0.5f * sc.Z + tl.Z,
                0.5f * sc.X + tl.X, -0.5f * sc.Y + tl.Y, -0.5f * sc.Z + tl.Z,
                0.5f * sc.X + tl.X,  0.5f * sc.Y + tl.Y, -0.5f * sc.Z + tl.Z,
                -0.5f * sc.X + tl.X,  0.5f * sc.Y + tl.Y, -0.5f * sc.Z + tl.Z,
                -0.5f * sc.X + tl.X, -0.5f * sc.Y + tl.Y,  0.5f * sc.Z + tl.Z,
                0.5f * sc.X + tl.X, -0.5f * sc.Y + tl.Y,  0.5f * sc.Z + tl.Z,
                0.5f * sc.X + tl.X,  0.5f * sc.Y + tl.Y,  0.5f * sc.Z + tl.Z,
                -0.5f * sc.X + tl.X,  0.5f * sc.Y + tl.Y,  0.5f * sc.Z + tl.Z
            }, 0);

            SetIndices(new uint[] {
                0, 1,
                1, 2,
                2, 3,
                3, 0,
                4, 5,
                5, 6,
                6, 7,
                7, 4,
                0, 4,
                1, 5,
                2, 6,
                3, 7
            }, 0);

            SetUVs(new float[]{
                -0.5f, -0.5f,
                0.5f, -0.5f,
                0.5f,  0.5f,
                -0.5f,  0.5f,
                -0.5f, -0.5f,
                0.5f, -0.5f,
                0.5f,  0.5f,
                -0.5f,  0.5f
            }, 0);

        }
    }
}
