using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Rendering
{
    public class GIObjectGenerator
    {

        public GIObjectGenerator() { }

        public GIObject Generate(BoundingBox bound, float[] verts, float[] uvs, float[] norms, Image tex, int[] indices)
        {
            VoxelTree root = new VoxelTree();
            int voxelCnt = 0;
            float BlockEdge = (bound.Max - bound.Min).Length / 64; //Each mesh is voxelized to a 256x256x256 dataset, keeps the voxelization speed high

            Vector3 bSize = bound.Max - bound.Min;

            #region Mesh Voxelizer
            for (int index = 0; index < indices.Length; index += 9)
            {
                Vector3 a = new Vector3(verts[index], verts[index + 1], verts[index + 2]);
                Vector3 b = new Vector3(verts[index + 3], verts[index + 4], verts[index + 5]);
                Vector3 c = new Vector3(verts[index + 6], verts[index + 7], verts[index + 8]);

                Vector3 n_a = new Vector3(norms[index], norms[index + 1], norms[index + 2]);
                Vector3 n_b = new Vector3(norms[index + 3], norms[index + 4], norms[index + 5]);
                Vector3 n_c = new Vector3(norms[index + 6], norms[index + 7], norms[index + 8]);

                Vector2 uv_a = new Vector2(uvs[index], uvs[index + 1]);
                Vector2 uv_b = new Vector2(uvs[index + 2], uvs[index + 3]);
                Vector2 uv_c = new Vector2(uvs[index + 4], uvs[index + 5]);

                Vector3 fNorm = (n_a + n_b + n_c) / 3;



                //Voxelize this triangle and insert it into the mesh data
                BoundingBox triangleBounds = new BoundingBox(Vector3.ComponentMin(Vector3.ComponentMin(a, b), c), Vector3.ComponentMax(Vector3.ComponentMax(a, b), c));
                Vector3 size = triangleBounds.Max - triangleBounds.Min;

                BoundingBox voxel = new BoundingBox(Vector3.Zero, new Vector3(BlockEdge));

                triangleBounds.Min.X -= triangleBounds.Min.X % BlockEdge;
                triangleBounds.Min.Y -= triangleBounds.Min.Y % BlockEdge;
                triangleBounds.Min.Z -= triangleBounds.Min.Z % BlockEdge;

                size.X = ((int)System.Math.Round((size.X / BlockEdge), MidpointRounding.AwayFromZero) * BlockEdge);
                size.Y = ((int)System.Math.Round((size.Y / BlockEdge), MidpointRounding.AwayFromZero) * BlockEdge);
                size.Z = ((int)System.Math.Round((size.Z / BlockEdge), MidpointRounding.AwayFromZero) * BlockEdge);

                triangleBounds.Max = triangleBounds.Min + size;

                //Search an 8x8 block of voxels for each triangle
                for (float x = triangleBounds.Min.X; x < triangleBounds.Max.X; x += BlockEdge)
                {
                    for (float y = triangleBounds.Min.Y; y < triangleBounds.Max.Y; y += BlockEdge)
                    {
                        for (float z = triangleBounds.Min.Z; z < triangleBounds.Max.Z; z += BlockEdge)
                        {
                            Vector3 vPos = new Vector3(x, y, z);
                            BoundingBox v2 = new BoundingBox(vPos + voxel.Min, vPos + voxel.Max);

                            if (GI_IntersectionTests.IsIntersecting(v2, a, b, c, fNorm))
                            {
                                var f1 = a - vPos;
                                var f2 = b - vPos;
                                var f3 = c - vPos;
                                // calculate the areas and factors (order of parameters doesn't matter):
                                float b_A = Vector3.Cross(f1 - f2, f1 - f3).Length; // main triangle area a
                                float b_B = Vector3.Cross(f2, f3).Length / b_A; // p1's triangle area / a
                                float b_C = Vector3.Cross(f3, f1).Length / b_A; // p2's triangle area / a 
                                float b_D = Vector3.Cross(f1, f2).Length / b_A; // p3's triangle area / a
                                                                                // find the uv corresponding to point f (uv1/uv2/uv3 are associated to p1/p2/p3):
                                Vector2 uv = uv_a * b_B + uv_b * b_C + uv_c * b_D;
                                uv.X *= tex.Width;
                                uv.Y *= tex.Height;
                                var tmpCol = ((Bitmap)tex).GetPixel((int)uv.X % tex.Width, (int)uv.Y % tex.Height);

                                root.AddObject(new VoxelData()
                                {
                                    Position = vPos,
                                    Color = new Vector3(tmpCol.R, tmpCol.G, tmpCol.B),
                                    Normal = n_a * b_B + n_b * b_C + n_c * b_D
                                }, Vector3.Zero, System.Math.Max(bSize.X, System.Math.Max(bSize.Y, bSize.Z)));
                                voxelCnt++;
                            }
                        }
                    }
                }

            }
            #endregion

            root.OptimizeTree();

            //By now we have an octree of all the voxels comprising this mesh
            //Add this mesh to the global GI world
            //The GI world handles GI between objects
            return new GIObject(root, BlockEdge, bound);
        }





    }
}
