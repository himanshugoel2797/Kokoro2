﻿using Assimp;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Kokoro.ContentPipeline
{
    public class ModelConvert
    {
        private static Matrix4 ToCustom(Matrix4x4 r)
        {
            Matrix4 n0 = new Matrix4();

            n0.M11 = r.A1;
            n0.M12 = r.B1;
            n0.M13 = r.C1;
            n0.M14 = r.D1;

            n0.M21 = r.A2;
            n0.M22 = r.B2;
            n0.M23 = r.C2;
            n0.M24 = r.D2;

            n0.M31 = r.A3;
            n0.M32 = r.B3;
            n0.M33 = r.C3;
            n0.M34 = r.D3;

            n0.M41 = r.A4;
            n0.M42 = r.B4;
            n0.M43 = r.C4;
            n0.M44 = r.D4;

            return n0;
        }

        public static byte[] Process(string filename)
        {
            List<float[][]> bounds = new List<float[][]>();

            AssimpContext context = new AssimpContext();
            Scene model = context.ImportFile(filename, PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.ImproveCacheLocality | PostProcessSteps.OptimizeMeshes | PostProcessSteps.RemoveRedundantMaterials);
            SceneGraph sc = null;

            if (model.MeshCount > 1)
            {
                sc = new SceneGraph();

                Action<SceneGraph, Node> scBuild = null;
                scBuild = (a, n) =>
                {
                    a.Transform = ToCustom(n.Transform);

                    if (n.HasMeshes)
                    {
                        a.Objects = new int[n.MeshCount];

                        for (int i = 0; i < n.MeshCount; i++)
                        {
                            a.Objects[i] = n.MeshIndices[i];
                        }
                    }

                    if (n.HasChildren)
                    {
                        a.Children = new SceneGraph[n.ChildCount];
                        for (int i = 0; i < n.ChildCount; i++)
                        {
                            a.Children[i] = new SceneGraph();
                            scBuild(a.Children[i], n.Children[i]);
                        }
                    }
                };

                scBuild(sc, model.RootNode);
            }

            string baseDir = Path.GetDirectoryName(filename);

            List<string> texs = new List<string>();
            List<float[]> Vertices = new List<float[]>();
            List<float[]> UV = new List<float[]>();
            List<float[]> Normals = new List<float[]>();
            List<uint[]> Indices = new List<uint[]>();
            List<List<float>[]> VertexWeights = new List<List<float>[]>();
            List<List<int>[]> VertexBones = new List<List<int>[]>();
            List<float[][]> SkeletonBones = new List<float[][]>();
            List<bool> isLine = new List<bool>();

            //TODO Eventually we will want to parse information from another file regarding which shaders to load and the parameters and add that information here (by means of reference to another file and a material DB class?)


            for (int a = 0; a < model.MeshCount; a++)
            {
                string t;
                Mesh m = model.Meshes[a];

                #region Textures
                if (m.MaterialIndex >= 0 && model.Materials[m.MaterialIndex].TextureDiffuse.FilePath != null) t = model.Materials[m.MaterialIndex].TextureDiffuse.FilePath;
                else
                {
                    t = null;
                }
                texs.Add(t);
                #endregion

                bounds.Add(new float[2][]);
                bounds[bounds.Count - 1][0] = new float[] { m.Vertices[0].X, m.Vertices[0].Y, m.Vertices[0].Z };
                bounds[bounds.Count - 1][1] = new float[] { m.Vertices[0].X, m.Vertices[0].Y, m.Vertices[0].Z };

                isLine.Add(m.FaceCount == 0);

                #region Vertices
                float[] vertices = new float[m.VertexCount * 3];
                for (int v = 0; v < m.VertexCount * 3; v += 3)
                {
                    vertices[v] = m.Vertices[(v - (v % 3)) / 3].X;
                    vertices[v + 1] = m.Vertices[(v - (v % 3)) / 3].Y;
                    vertices[v + 2] = m.Vertices[(v - (v % 3)) / 3].Z;

                    if (vertices[v] < bounds[bounds.Count - 1][0][0]) bounds[bounds.Count - 1][0][0] = vertices[v];
                    if (vertices[v + 1] < bounds[bounds.Count - 1][0][1]) bounds[bounds.Count - 1][0][1] = vertices[v + 1];
                    if (vertices[v + 2] < bounds[bounds.Count - 1][0][2]) bounds[bounds.Count - 1][0][2] = vertices[v + 2];
                    if (vertices[v] > bounds[bounds.Count - 1][1][0]) bounds[bounds.Count - 1][1][0] = vertices[v];
                    if (vertices[v + 1] > bounds[bounds.Count - 1][1][1]) bounds[bounds.Count - 1][1][1] = vertices[v + 1];
                    if (vertices[v + 2] > bounds[bounds.Count - 1][1][2]) bounds[bounds.Count - 1][1][2] = vertices[v + 2];
                }
                Vertices.Add(vertices);
                #endregion

                //Handle bones
                #region Bones
                List<float>[] Weights = new List<float>[m.VertexCount];
                List<int>[] Bones = new List<int>[m.VertexCount];
                float[][] skBones = null;
                if (m.HasBones)
                {
                    for (int b = 0; b < m.BoneCount; b++)
                    {
                        //Save the offset matrix
                        skBones =
                            new float[][] {
                                new float[]{ m.Bones[b].OffsetMatrix.A1,m.Bones[b].OffsetMatrix.A2,m.Bones[b].OffsetMatrix.A3,m.Bones[b].OffsetMatrix.A4  },
                                new float[]{ m.Bones[b].OffsetMatrix.B1,m.Bones[b].OffsetMatrix.B2,m.Bones[b].OffsetMatrix.B3,m.Bones[b].OffsetMatrix.B4,},
                                new float[]{ m.Bones[b].OffsetMatrix.C1,m.Bones[b].OffsetMatrix.C2,m.Bones[b].OffsetMatrix.C3,m.Bones[b].OffsetMatrix.C4,},
                                new float[]{ m.Bones[b].OffsetMatrix.D1,m.Bones[b].OffsetMatrix.D2,m.Bones[b].OffsetMatrix.D3,m.Bones[b].OffsetMatrix.D4,}
                            };


                        foreach (VertexWeight w in m.Bones[b].VertexWeights)
                        {
                            //Save the vertex weight
                            if (Weights[w.VertexID] == null) Weights[w.VertexID] = new List<float>();
                            Weights[w.VertexID].Add(w.Weight);

                            //Save the bone id
                            if (Bones[w.VertexID] == null) Bones[w.VertexID] = new List<int>();
                            Bones[w.VertexID].Add(b);
                        }

                    }
                }
                VertexBones.Add(Bones);
                VertexWeights.Add(Weights);
                SkeletonBones.Add(skBones);
                #endregion

                //Handle indices
                #region Indices
                if (m.GetIndices() != null)
                {
                    uint[] indices = Array.ConvertAll(m.GetIndices(), x => (uint)x);
                    Indices.Add(indices);
                }
                else
                {
                    uint z = 0;
                    uint[] indices = Array.ConvertAll<Vector3D, uint>(m.Vertices.ToArray(), x => z++);  //Just generate a series of numbers for each vertex
                    Indices.Add(indices);
                }
                #endregion

                #region Texture Coordinates
                if (m.TextureCoordinateChannels.Length > 0 && m.TextureCoordinateChannels[0].Count > 0)
                {
                    float[] texcoords = new float[m.VertexCount * 2];
                    for (int v = 0; v < m.VertexCount * 2; v += 2)
                    {
                        texcoords[v] = m.TextureCoordinateChannels[0][v / 2].X;
                        texcoords[v + 1] = m.TextureCoordinateChannels[0][v / 2].Y;
                    }
                    UV.Add(texcoords);
                }
                else
                {
                    float[] texcoords = new float[m.VertexCount * 2];
                    for (int v = 0; v < m.VertexCount * 2; v += 2)
                    {
                        texcoords[v] = m.Vertices[(v - (v % 2)) / 2].X;
                        texcoords[v + 1] = m.Vertices[(v - (v % 2)) / 2].Y;
                    }
                    UV.Add(texcoords);
                }
                #endregion

                #region Normals
                float[] normals = new float[m.VertexCount * 3];
                if (m.HasNormals)
                {
                    int v = 0;
                    foreach (Vector3D n in m.Normals)
                    {
                        normals[v++] = n.X;
                        normals[v++] = n.Y;
                        normals[v++] = n.Z;
                    }
                }
                Normals.Add(normals);
                #endregion
            }

            //Now that we have all the data in a closer to what we want format, restructure it so that it can be drawn in a single draw call
            #region Final conversion steps
            float[][][] vweights = new float[VertexWeights.Count][][];
            for (int a0 = 0; a0 < VertexWeights.Count; a0++)
            {
                vweights[a0] = new float[VertexBones[a0].Length][];

                for (int a1 = 0; a1 < VertexWeights[a0].Length; a1++)
                {
                    if (VertexWeights[a0][a1] != null) vweights[a0][a1] = VertexWeights[a0][a1].ToArray();
                }
            }

            int[][][] vbones = new int[VertexBones.Count][][];
            for (int a0 = 0; a0 < VertexBones.Count; a0++)
            {
                vbones[a0] = new int[VertexBones[a0].Length][];

                for (int a1 = 0; a1 < VertexBones[a0].Length; a1++)
                {
                    if (VertexBones[a0][a1] != null) vbones[a0][a1] = VertexBones[a0][a1].ToArray();
                }
            }

            return FinalProcess(texs.ToArray(), Vertices.ToArray(), UV.ToArray(), Normals.ToArray(),
                Indices.ToArray(), texs.ToArray(), bounds.ToArray(), vweights, vbones, SkeletonBones.ToArray(), isLine.ToArray(), sc);
            #endregion
        }

        [ProtoContract]
        class MeshInfo
        {
            [ProtoMember(1)]
            public float[] Vertices;

            [ProtoMember(2)]
            public float[] uvs;

            [ProtoMember(4)]
            public float[] normals;

            [ProtoMember(3)]
            public uint[] indices;

            [ProtoMember(5)]
            public string tex;

            [ProtoMember(6)]
            public Weight[] Weights;

            [ProtoMember(7)]
            public Bone[] Bones;

            [ProtoMember(8)]
            public Skeleton[] skeleton;

            [ProtoMember(9)]
            public bool isLine;

            [ProtoMember(10)]
            public float[] BoundingBox;
        }

        [ProtoContract]
        class Weight
        {
            [ProtoMember(1)]
            public float[] Weights;
        }

        [ProtoContract]
        class Bone
        {
            [ProtoMember(1)]
            public int[] Bones;
        }

        [ProtoContract]
        class Skeleton
        {
            [ProtoMember(1)]
            public float[] Skeletons;
        }

        [ProtoContract]
        class SceneGraph
        {
            [ProtoMember(1)]
            public SceneGraph[] Children;

            [ProtoMember(2)]
            public Matrix4 Transform;

            [ProtoMember(3)]
            public int[] Objects;
        }

        [ProtoContract]
        class Model
        {
            [ProtoMember(3)]
            public SceneGraph Scene;

            [ProtoMember(1)]
            public MeshInfo[] Mesh;

        }

        private static byte[] FinalProcess(string[] tex, float[][] verts, float[][] uvs, float[][] norms,
            uint[][] indices, string[] texPaths, float[][][] boundingbox, float[][][] weights = null, int[][][] bones = null, float[][][] skeleton = null, bool[] isLine = null, SceneGraph sc = null)
        {
            byte[] outdata;

            Model m = new Model()
            {
                Scene = sc,
                Mesh = new MeshInfo[indices.Length]
            };

            for (int i = 0; i < indices.Length; i++)
            {
                m.Mesh[i] = new MeshInfo()
                {
                    Vertices = verts[i],
                    indices = indices[i],
                    normals = norms[i],
                    tex = texPaths[i],
                    uvs = uvs[i],
                    isLine = isLine[i],
                    BoundingBox = new float[6]
                };

                m.Mesh[i].BoundingBox[0] = boundingbox[i][0][0];
                m.Mesh[i].BoundingBox[1] = boundingbox[i][0][1];
                m.Mesh[i].BoundingBox[2] = boundingbox[i][0][2];
                m.Mesh[i].BoundingBox[3] = boundingbox[i][1][0];
                m.Mesh[i].BoundingBox[4] = boundingbox[i][1][1];
                m.Mesh[i].BoundingBox[5] = boundingbox[i][1][2];

                if (weights[i] != null && weights[i].Length > 0)
                {
                    m.Mesh[i].Weights = new Weight[weights[i].Length];
                    for (int i2 = 0; i2 < weights[i].Length; i2++)
                    {
                        m.Mesh[i].Weights[i2] = new Weight()
                        {
                            Weights = weights[i][i2]
                        };
                    }
                }

                if (bones[i] != null && bones[i].Length > 0)
                {
                    m.Mesh[i].Bones = new Bone[bones[i].Length];
                    for (int i2 = 0; i2 < bones[i].Length; i2++)
                    {
                        m.Mesh[i].Bones[i2] = new Bone()
                        {
                            Bones = bones[i][i2]
                        };
                    }
                }

                if (skeleton[i] != null && skeleton[i].Length > 0)
                {
                    m.Mesh[i].skeleton = new Skeleton[skeleton[i].Length];
                    for (int i2 = 0; i2 < skeleton[i].Length; i2++)
                    {
                        m.Mesh[i].skeleton[i2] = new Skeleton()
                        {
                            Skeletons = skeleton[i][i2]
                        };
                    }
                }
            }


            using (MemoryStream strm = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(strm, m);
                outdata = strm.ToArray();
            }

            return outdata;
        }

    }
}
