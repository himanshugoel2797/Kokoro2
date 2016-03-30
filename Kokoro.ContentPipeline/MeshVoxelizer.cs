using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using Kokoro2.Engine.HighLevel.Voxel;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro.ContentPipeline
{
    class MeshVoxelizer
    {
        private static long gcd(long a, long b)
        {
            while (b != 0)
            {
                long t = b;
                b = a % b;
                a = t;
            }
            return a;
        }

        public static void Voxelize(ModelConvert.Model m)
        {
            Vector3 min = new Vector3(m.BoundingBox[0], m.BoundingBox[1], m.BoundingBox[2]);
            Vector3 max = new Vector3(m.BoundingBox[3], m.BoundingBox[4], m.BoundingBox[5]);
            Vector3 dims = max - min;

            //find the best number of cubes to split this into by finding the 'gcd' of the three dimensions
            //dims *= 10000;
            //dims.Round();

            List<BEPUutilities.Vector3> verts = new List<BEPUutilities.Vector3>();
            for (int i = 0; i < m.Mesh[0].Vertices.Length; i += 3)
            {
                verts.Add(new BEPUutilities.Vector3(m.Mesh[0].Vertices[i], m.Mesh[0].Vertices[i + 1], m.Mesh[0].Vertices[i + 2]));
            }

            int[] indices = new int[m.Mesh[0].indices.Length];
            for (int i = 0; i < indices.Length; i++) indices[i] = (int)m.Mesh[0].indices[i];

            Space s = new Space();
            StaticMesh sm = new StaticMesh(verts.ToArray(), indices);
            s.Add(sm);

            //Calculate the gcd of the three dimensions
            double gcd_0 = gcd((long)dims.X, (long)dims.Y);
            double gcd_1 = gcd((long)gcd_0, (long)dims.Z);

            //dims /= (float)gcd_1;

            Vector3 trueDims = max - min;

            BinaryVoxelTree t = new BinaryVoxelTree();
            t.Center = t;

            int ids = 1;

            int lod = (int)BinaryVoxelChunkLOD.LOD0;



            Dictionary<Vector4, BinaryVoxelChunk> chunks = new Dictionary<Vector4, BinaryVoxelChunk>();

            for (int i = 0; i < 3; i++)
            {
                lod = (int)(BinaryVoxelChunkLOD)Enum.Parse(typeof(BinaryVoxelChunkLOD), "LOD" + i);
                float step = 1f / (float)lod;

                BinaryVoxelChunk chunk = new BinaryVoxelChunk()
                {
                    ID = ids++,
                    LoD = (BinaryVoxelChunkLOD)lod,
                    Present = new bool[lod, lod, lod]
                };

                float x0 = 0, y0 = 0, z0 = 0;
                float fac = 1;//(float)gcd_1 / 10000f;

                #region LOD0 Voxelization
                for (float x = 0; x < dims.X; x += step)
                    for (float y = 0; y < dims.Y; y += step)
                        for (float z = 0; z < dims.Z; z += step)
                        {
                            Vector3 a = new Vector3(min.X + x, min.Y + y, min.Z + z) * fac;
                            Vector3 b = new Vector3(min.X + x + step, min.Y + y + step, min.Z + z + step) * fac;

                            Vector3 c = (a + b) / 2.0f;

                            RayCastResult r = new RayCastResult();
                            BEPUutilities.Ray ray = new BEPUutilities.Ray()
                            {
                                Direction = BEPUutilities.Vector3.UnitX,
                                Position = new BEPUutilities.Vector3(c.X, c.Y, c.Z)
                            };

                            BEPUutilities.Ray ray2 = new BEPUutilities.Ray()
                            {
                                Direction = BEPUutilities.Vector3.UnitY,
                                Position = new BEPUutilities.Vector3(c.X, c.Y, c.Z)
                            };

                            BEPUutilities.Ray ray3 = new BEPUutilities.Ray()
                            {
                                Direction = BEPUutilities.Vector3.UnitZ,
                                Position = new BEPUutilities.Vector3(c.X, c.Y, c.Z)
                            };

                            BEPUutilities.Ray ray4 = new BEPUutilities.Ray()
                            {
                                Direction = -BEPUutilities.Vector3.UnitX,
                                Position = new BEPUutilities.Vector3(c.X, c.Y, c.Z)
                            };

                            BEPUutilities.Ray ray5 = new BEPUutilities.Ray()
                            {
                                Direction = -BEPUutilities.Vector3.UnitY,
                                Position = new BEPUutilities.Vector3(c.X, c.Y, c.Z)
                            };

                            BEPUutilities.Ray ray6 = new BEPUutilities.Ray()
                            {
                                Direction = -BEPUutilities.Vector3.UnitZ,
                                Position = new BEPUutilities.Vector3(c.X, c.Y, c.Z)
                            };
                            bool present = false;
                            if (s.RayCast(ray, out r) && s.RayCast(ray2, out r) && s.RayCast(ray3, out r) && s.RayCast(ray4, out r) && s.RayCast(ray5, out r) && s.RayCast(ray6, out r))
                            {
                                present = true;
                            }

                            if ((x - x0) == 1)
                            {
                                x0++;
                                chunks.Add(new Vector4(x, y, z, lod), chunk);
                                chunk = new BinaryVoxelChunk()
                                {
                                    ID = ids++,
                                    LoD = (BinaryVoxelChunkLOD)lod,
                                    Present = new bool[lod, lod, lod]
                                };
                            }

                            if ((y - y0) == 1)
                            {
                                y0++;
                                chunks.Add(new Vector4(x, y, z, lod), chunk);
                                chunk = new BinaryVoxelChunk()
                                {
                                    ID = ids++,
                                    LoD = (BinaryVoxelChunkLOD)lod,
                                    Present = new bool[lod, lod, lod]
                                };
                            }

                            if ((z - z0) == 1)
                            {
                                z0++;
                                chunks.Add(new Vector4(x, y, z, lod), chunk);
                                chunk = new BinaryVoxelChunk()
                                {
                                    ID = ids++,
                                    LoD = (BinaryVoxelChunkLOD)lod,
                                    Present = new bool[lod, lod, lod]
                                };
                            }

                            chunk.Present[(int)((x - x0) * lod), (int)((y - y0) * lod), (int)((z - z0) * lod)] = present;
                        }
                #endregion
            }

            Console.WriteLine("Wait");

        }
    }
}
