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
            this.World = Matrix4.CreateTranslation((box.Min + box.Max) / 2) * Matrix4.Scale(box.Max - box.Min);
            this.DrawMode = Engine.DrawMode.Lines;
            Init(1);


            SetVertices(UpdateMode.Static, new float[]{
                -0.5f, -0.5f, -0.5f,
                0.5f, -0.5f, -0.5f,
                0.5f,  0.5f, -0.5f,
                -0.5f,  0.5f, -0.5f,
                -0.5f, -0.5f,  0.5f,
                0.5f, -0.5f,  0.5f,
                0.5f,  0.5f,  0.5f,
                -0.5f,  0.5f,  0.5f
            }, 0);

            SetIndices(UpdateMode.Static, new uint[] {
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

            SetUVs(UpdateMode.Static, new float[]{
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
