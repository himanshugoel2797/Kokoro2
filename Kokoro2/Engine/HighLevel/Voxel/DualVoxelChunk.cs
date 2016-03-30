using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Voxel
{
    public class DualVoxelChunk
    {
        public int Side = 64;

        public float[,,] Volume;

        public void InitDataStore(Func<float, float, float, float> f)
        {
            Volume = new float[Side, Side, Side];

            for (float x = 0; x < Side; x++)
                for (float y = 0; y < Side; y++)
                    for (float z = 0; z < Side; z++)
                    {
                        Volume[(int)x, (int)y, (int)z] = f(x, y, z);
                    }
        }

        public void GenerateMesh(Func<float, float, float, float> f, out float[] vertsO, out int[] indices, out float[] norms)
        {

            #region Naive Surface Nets

            int[] cube_edges = new int[24];
            int[] edgeTable = new int[256];

            int k = 0;
            for (int i = 0; i < 8; i++)
                for (int j = 1; j <= 4; j <<= 1)
                {
                    int p = i ^ j;
                    if (i <= p)
                    {
                        cube_edges[k++] = i;
                        cube_edges[k++] = p;
                    }
                }

            for (int i = 0; i < 256; ++i)
            {
                int em = 0;
                for (int j = 0; j < 24; j += 2)
                {
                    bool a = (i & (1 << cube_edges[j])) != 0;
                    bool b = (i & (1 << cube_edges[j + 1])) != 0;

                    em |= (a != b) ? (1 << (j >> 1)) : 0;
                }
                edgeTable[i] = em;
            }

            List<float> verts = new List<float>();
            List<int> faces = new List<int>();

            int n = 0;
            int[] x = new int[3];
            int[] R = new int[] { 1, Side + 1, (Side + 1) * (Side + 1) };
            float[] grid = new float[8];
            int buf_no = 1;
            int[] buffer = new int[R[2] * 2];

            for (x[2] = 0; x[2] < Side - 1; ++x[2], n += Side, buf_no ^= 1, R[2] = -R[2])
            {
                int m = 1 + (Side + 1) * (1 + buf_no * (Side + 1));

                for (x[1] = 0; x[1] < Side - 1; ++x[1], ++n, m += 2)
                    for (x[0] = 0; x[0] < Side - 1; ++x[0], ++n, ++m)
                    {
                        int mask = 0, g = 0, idx = n;

                        for (k = 0; k < 2; ++k, idx += Side * (Side - 2))
                            for (int j = 0; j < 2; ++j, idx += Side - 2)
                                for (int i = 0; i < 2; ++i, ++g, ++idx)
                                {
                                    int x0 = idx % Side;
                                    int y0 = idx / Side % Side;
                                    int z0 = idx / (Side * Side) % Side;

                                    float p = Volume[x0, y0, z0];
                                    grid[g] = p;
                                    mask |= (p < 0) ? (1 << g) : 0;
                                }

                        if (mask == 0 || mask == 0xFF) continue;

                        int edge_mask = edgeTable[mask];
                        float[] v = new float[3];
                        int e_count = 0;

                        for (int i = 0; i < 12; ++i)
                        {
                            if ((edge_mask & 1 << i) == 0) continue;

                            ++e_count;

                            int e0 = cube_edges[i << 1];
                            int e1 = cube_edges[(i << 1) + 1];
                            float g0 = grid[e0];
                            float g1 = grid[e1];
                            float t = g0 - g1;

                            if (System.Math.Abs(t) > 1e-6)
                            {
                                t = g0 / t;
                            }
                            else
                            {
                                continue;
                            }

                            for (int j = 0, k0 = 1; j < 3; ++j, k0 <<= 1)
                            {
                                int a = e0 & k0;
                                int b = e1 & k0;
                                if (a != b) v[j] += (a != 0) ? 1.0f - t : t;
                                else v[j] += (a != 0) ? 1 : 0;
                                k = k0;
                            }
                        }

                        float s = 1.0f / e_count;
                        for (int i = 0; i < 3; ++i)
                        {
                            v[i] = x[i] + s * v[i];
                        }

                        buffer[m] = verts.Count / 4;
                        verts.AddRange(new float[] { v[0], v[1], v[2], 0 });

                        for (int i = 0; i < 3; ++i)
                        {
                            if ((edge_mask & (1 << i)) == 0) continue;

                            int iu = (i + 1) % 3;
                            int iv = (i + 2) % 3;

                            if (x[iu] == 0 || x[iv] == 0) continue;

                            int du = R[iu];
                            int dv = R[iv];

                            if ((mask & 1) == 1)
                            {
                                faces.Add(buffer[m]);
                                faces.Add(buffer[m - du]);
                                faces.Add(buffer[m - du - dv]);
                                faces.Add(buffer[m]);
                                faces.Add(buffer[m - du - dv]);
                                faces.Add(buffer[m - dv]);
                            }
                            else
                            {
                                faces.Add(buffer[m]);
                                faces.Add(buffer[m - dv]);
                                faces.Add(buffer[m - du - dv]);
                                faces.Add(buffer[m]);
                                faces.Add(buffer[m - du - dv]);
                                faces.Add(buffer[m - du]);
                            }
                        }
                    }
            }

            float[] normal = new float[verts.Count];

            /*
            for (int i = 0; i < faces.Count; i += 6)
            {
                Vector3 v0 = new Vector3(verts[faces[i] * 4], verts[faces[i] * 4 + 1], verts[faces[i] * 4 + 2]);
                Vector3 v1 = new Vector3(verts[faces[i + 1] * 4], verts[faces[i + 1] * 4 + 1], verts[faces[i + 1] * 4 + 2]);
                Vector3 v2 = new Vector3(verts[faces[i + 2] * 4], verts[faces[i + 2] * 4 + 1], verts[faces[i + 2] * 4 + 2]);
                Vector3 v3 = new Vector3(verts[faces[i + 5] * 4], verts[faces[i + 5] * 4 + 1], verts[faces[i + 5] * 4 + 2]);
                //Calculate normals
                Vector3 t0 = v2 - v1;
                Vector3 t1 = v0 - v1;
                Vector3 norm = Vector3.Cross(t0, t1).Normalized();
                norm.X = (float)Math.Round(norm.X);
                norm.Y = (float)Math.Round(norm.Y);
                norm.Z = (float)Math.Round(norm.Z);
                if (norm == Vector3.Zero)
                {
                    t0 = v3 - v2;
                    t1 = v0 - v2;
                    norm = Vector3.Cross(t0, t1).Normalized();
                    norm.X = (float)Math.Round(norm.X);
                    norm.Y = (float)Math.Round(norm.Y);
                    norm.Z = (float)Math.Round(norm.Z);
                }
                normal[faces[i] * 4] += norm.X;
                normal[faces[i] * 4 + 1] += norm.Y;
                normal[faces[i] * 4 + 2] += norm.Z;
                normal[faces[i + 1] * 4] += norm.X;
                normal[faces[i + 1] * 4 + 1] += norm.Y;
                normal[faces[i + 1] * 4 + 2] += norm.Z;
                normal[faces[i + 2] * 4] += norm.X;
                normal[faces[i + 2] * 4 + 1] += norm.Y;
                normal[faces[i + 2] * 4 + 2] += norm.Z;
                normal[faces[i + 3] * 4] += norm.X;
                normal[faces[i + 3] * 4 + 1] += norm.Y;
                normal[faces[i + 3] * 4 + 2] += norm.Z;
                normal[faces[i + 4] * 4] += norm.X;
                normal[faces[i + 4] * 4 + 1] += norm.Y;
                normal[faces[i + 4] * 4 + 2] += norm.Z;
                normal[faces[i + 5] * 4] += norm.X;
                normal[faces[i + 5] * 4 + 1] += norm.Y;
                normal[faces[i + 5] * 4 + 2] += norm.Z;
            }
            for (int i = 0; i < normal.Length; i+=4)
            {
                Vector3 nor = new Vector3(normal[i], normal[i + 1], normal[i + 2]);
                nor.Normalize();
                normal[i] = nor.X;
                normal[i + 1] = nor.Y;
                normal[i + 2] = nor.Z;
            }*/

            for (int i = 0; i < normal.Length; i += 4)
            {
                float x0 = (f(verts[i] + 1f, verts[i + 1], verts[i + 2]) - f(verts[i] - 1f, verts[i + 1], verts[i + 2])) / 2f;
                float y0 = (f(verts[i], verts[i + 1] + 1f, verts[i + 2]) - f(verts[i], verts[i + 1] - 1f, verts[i + 2])) / 2f;
                float z0 = (f(verts[i], verts[i + 1], verts[i + 2] + 1f) - f(verts[i], verts[i + 1], verts[i + 2] - 1f)) / 2f;

                Vector3 n00 = new Vector3(x0, y0, z0);
                n00.Normalize();

                normal[i] = n00.X;
                normal[i + 1] = n00.Y;
                normal[i + 2] = n00.Z;
            }

            #endregion

            vertsO = verts.ToArray();
            indices = faces.ToArray();
            norms = normal.ToArray();

        }
    }
}
