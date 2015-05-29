using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine.Shaders;

#if OPENGL
#if PC
using Kokoro2.OpenGL.PC;
#endif
#endif

namespace Kokoro2.Engine.Prefabs
{
    /// <summary>
    /// Represents a Full Screen Quad
    /// </summary>
    public class FullScreenQuad : Model
    {
        /// <summary>
        /// Creates a new FullScreenQuad object
        /// </summary>
        public FullScreenQuad()
            : base()
        {
            Init(1);

            this.DrawMode = DrawMode.Triangles;

            SetIndices(UpdateMode.Static, new uint[] { 3, 2, 0, 0, 2, 1 }, 0);
            SetUVs(UpdateMode.Static, new float[] {
                0,1,
                1,1,
                1,0,
                0,0
            }, 0);

            SetVertices(UpdateMode.Static, new float[]{
                -1, 1, 0.5f,
                1, 1, 0.5f,
                1, -1,0.5f,
                -1, -1,0.5f
            }, 0);

            World = Math.Matrix4.Identity;
        }
    }
}
