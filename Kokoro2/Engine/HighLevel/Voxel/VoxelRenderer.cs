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

namespace Kokoro2.Engine.HighLevel.Voxel
{
    public class VoxelRenderer : Model
    {
        public int IndexCount;

        int allocSize;
        VoxelCollection voxelInfo;

        internal List<float> Vertices = new List<float>();
        internal List<float> Normals = new List<float>();
        internal List<float> UVs = new List<float>();
        internal List<uint> Indices = new List<uint>();

        public VoxelRenderer(int side, VoxelCollection VoxelInfo)
        {
            Init(1);
            allocSize = side * side * side * 56;
            PreAlloc(UpdateMode.Static, 0, allocSize);
            World = Matrix4.Identity;
            voxelInfo = VoxelInfo;
        }

        public void GenerateMeshData(VoxelChunk data)
        {
            Vertices.Clear();
            Normals.Clear();
            UVs.Clear();
            Indices.Clear();
            //Starting from 0,0,0 move along the surface and build the mesh
            uint i = 0;
            for (int x = 0; x < data.Side; x++)
                for (int y = 0; y < data.Side; y++)
                    for (int z = 0; z < data.Side; z++)
                    {
                        if (voxelInfo[data.Info[x, y, z].VoxelType].Visible && data.IsSurface(x, y, z))    //Check if the specified voxel is a surface voxel, in which case, generate data for it
                        {
                            //For now just generate cubes for each, don't try to figure out which surfaces are visible
                            var vType = voxelInfo[data.Info[x, y, z].VoxelType];

                            //TODO come back to this later to see if it needs any optimization (which it can use but is it necessary?)
                            Vertices.AddRange(new float[]{
                x + 0.5f, y - 0.5f, z - 0.5f,     //2
                x - 0.5f, y - 0.5f, z - 0.5f,      //3
                x - 0.5f, y + 0.5f, z - 0.5f,      //4
                x - 0.5f, y + 0.5f, z + 0.5f,        //8
                x - 0.5f, y - 0.5f, z + 0.5f,     //7
                x + 0.5f, y - 0.5f, z + 0.5f,      //6
                x + 0.5f, y + 0.5f, z + 0.5f,     //5
                x + 0.5f, y - 0.5f, z + 0.5f,      //6
                x - 0.5f, y - 0.5f, z + 0.5f,     //7
                x - 0.5f, y - 0.5f, z + 0.5f,     //7
                x - 0.5f, y + 0.5f, z + 0.5f,        //8
                x + 0.5f, y + 0.5f, z - 0.5f,    //1
                x - 0.5f, y + 0.5f, z + 0.5f,        //8
                x + 0.5f, y + 0.5f, z + 0.5f,     //5
            });
                            Normals.AddRange(new float[]{
                x + 0.5f, y + 0.5f, z - 0.5f,    //0
                x + 0.5f, y - 0.5f, z - 0.5f,     //1
                x - 0.5f, y - 0.5f, z - 0.5f,      //2
                x - 0.5f, y + 0.5f, z - 0.5f,      //3
                x + 0.5f, y + 0.5f, z + 0.5f,     //4
                x + 0.5f, y - 0.5f, z + 0.5f,      //5
                x - 0.5f, y - 0.5f, z + 0.5f,     //6
                x - 0.5f, y + 0.5f, z + 0.5f,        //7
                x + 0.5f, y + 0.5f, z + 0.5f,     //8
                x - 0.5f, y - 0.5f, z + 0.5f,     //9
                x - 0.5f, y - 0.5f, z + 0.5f,     //10
                x + 0.5f, y + 0.5f, z + 0.5f,     //11
                x + 0.5f, y - 0.5f, z + 0.5f,      //12
                x + 0.5f, y + 0.5f, z + 0.5f,     //13
            });
                            float Xd = vType.AtlasDimensions.X, Yd = vType.AtlasDimensions.Y, Xl = vType.AtlasDimensions.Z, Yl = vType.AtlasDimensions.W;

                            UVs.AddRange(new float[]{
                                Xd + 0.5f*Xl, Yd + 0.33f * Yl,
                                Xd + 0.25f*Xl, Yd +  0.33f * Yl,
                                Xd + 0.25f*Xl, Yd +  0.66f * Yl,
                                Xd + 1f*Xl, Yd +  0.66f * Yl,
                                Xd + 1f*Xl,  Yd + 0.33f * Yl,
                                Xd + 0.75f*Xl, Yd +  0.33f * Yl,
                                Xd + 0.75f*Xl, Yd +  0.66f * Yl,
                                Xd + 0.5f*Xl, Yd +  0.0f * Yl,
                                Xd + 0.25f*Xl, Yd + 0f * Yl,
                                Xd + 0.0f*Xl, Yd + 0.33f * Yl,
                                Xd + 0*Xl, Yd + 0.66f * Yl,
                                Xd + 0.5f*Xl, Yd + 0.66f * Yl,
                                Xd + 0.25f*Xl, Yd + 1f * Yl,
                                Xd + 0.5f*Xl, Yd + 1f * Yl
                            });

                            Indices.AddRange(new uint[] { 
                i, i + 1, i+ 2,
                i + 3, i+4, i + 5,

                i + 6, i+5, i,
                i+7,i+8, i+1,
                
                i+9,i+10,i+2,
                i+11,i+2,i+12,
                
                i+11,i,i+2,
                i+6,i+3,i+5,
                
                i+11,i+6,i+0,
                i,i+7,i+1,
                
                i+1,i+9,i+2,
                i+13,i+11,i+12
                            });
                            i += 14;
                        }
                    }

            //lengths[0] = (uint)Indices.Count;

            if (Indices.Count != 0)
            {
                base.UpdateIndices(Indices.ToArray(), 0);
                base.UpdateVertices(Vertices.ToArray(), 0);
                base.UpdateNormals(Normals.ToArray(), 0);
                base.UpdateUVs(UVs.ToArray(), 0);

                this.Materials[0].ColorMap = voxelInfo.Atlas;
            }
            IndexCount = Indices.Count;
            if (IndexCount > allocSize) throw new OverflowException();
        }

        public void RenderTerrain(GraphicsContext context)
        {
            if (IndexCount > 0) Draw(context);
        }
    }
}
