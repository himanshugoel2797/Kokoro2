using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if OPENGL
#if PC
using Kokoro2.OpenGL.PC;
#endif
#endif

namespace Kokoro2.Engine.HighLevel.Voxel
{
    public class VoxelMesh
    {
        VertexArrayLL vArray;

        //TODO implement draw command for this separately as this is separate from the main draw buffer

        public Material MeshMaterial;

        public VoxelMesh(int vertexCount, int indexCount)
        {
            vArray = new VertexArrayLL(4, vertexCount, indexCount, UpdateMode.Static, new BufferUse[] { BufferUse.Index, BufferUse.Array, BufferUse.Array, BufferUse.Array }, new int[] { 1, 3, 3, 2 });
        }

        public void SetIndices(uint[] indices)
        {
            vArray[0].BufferData(indices);
        }

        public void SetVertices(float[] vertices)
        {
            vArray[1].BufferData(vertices);
        }

        public void SetNormals(float[] normals)
        {
            vArray[2].BufferData(normals);
        }

        public void SetUVs(float[] uvs)
        {
            vArray[3].BufferData(uvs);
        }



    }
}
