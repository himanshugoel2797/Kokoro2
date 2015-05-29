using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Math;

#if OPENGL
using Kokoro2.OpenGL;
#if PC
using Kokoro2.OpenGL.PC;
#endif
#endif

namespace Kokoro2.Engine.Prefabs
{
    public class InputDataMesh : Model
    {
        public int IndexCount;

        int allocSize;

        public InputDataMesh(int vertCount)
        {
            Init(1);
            allocSize = vertCount;
            PreAlloc(UpdateMode.Static, 0, allocSize);
            World = Matrix4.Identity;
        }

        public void GenerateMeshData(float[] vertices, Vector4 color, float[] UVs = null, float[] norms = null, uint[] index = null)
        {
            List<uint> Indices = new List<uint>();
            //Generate indices for all vertices
            if (index == null)
            {
                for (uint n = 0; n < vertices.Length / 3; n++)
                {
                    Indices.Add(n);
                }
            }
            else
            {
                Indices.AddRange(index);
            }

            if (Indices.Count != 0)
            {
                base.UpdateIndices(Indices.ToArray(), 0);
                base.UpdateVertices(vertices, 0);
                if (UVs != null) base.UpdateUVs(UVs, 0);
                if (norms != null) base.UpdateNormals(norms, 0);

                this.Materials[0].Shader["inColor"] = color;
            }
            IndexCount = Indices.Count;
            if (IndexCount > allocSize) throw new OverflowException();
        }

        public void RenderTerrain(GraphicsContext context)
        {
            if (IndexCount > 0)
            {
                Draw(context);
            }
        }
    }
}
