﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using Kokoro2.Math;

namespace Kokoro2.Engine.Prefabs
{
    public class VertexMesh : Model
    {
        #region Model Structures
        [ProtoContract]
        class MeshInfo_m
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
            public Weight_m[] Weights;

            [ProtoMember(7)]
            public Bone_m[] Bones;

            [ProtoMember(8)]
            public Skeleton_m[] skeleton;


            [ProtoMember(9)]
            public bool isLine;
        }

        [ProtoContract]
        class Weight_m
        {
            [ProtoMember(1)]
            public float[] Weights;
        }

        [ProtoContract]
        class Bone_m
        {
            [ProtoMember(1)]
            public int[] Bones;
        }

        [ProtoContract]
        class Skeleton_m
        {
            [ProtoMember(1)]
            public float[] Skeletons;
        }


        [ProtoContract]
        class Model_m
        {
            [ProtoMember(1)]
            public MeshInfo_m[] Mesh;

            [ProtoMember(2)]
            public float[] BoundingBox;

        }
        #endregion

        private bool animatedMesh;

        public VertexMesh(string filename, bool animated, GraphicsContext c, bool useVFS = false) : base(c)
        {
            Model_m tmp = null;
            if (useVFS)
            {
                tmp = Serializer.Deserialize<Model_m>(VFS.FSReader.OpenFile(filename, false));
            }
            else
            {
                tmp = Serializer.Deserialize<Model_m>(System.IO.File.OpenRead(filename));
            }

            animatedMesh = animated;

            Buffer.Bind();
            for (int i = 0; i < tmp.Mesh.Length; i++)
            {
                SetIndices(tmp.Mesh[i].indices, i);
                SetUVs(tmp.Mesh[i].uvs, i);
                SetVertices(tmp.Mesh[i].Vertices, i);
                SetNormals(tmp.Mesh[i].normals, i);

                if (tmp.Mesh[i].isLine) DrawMode = DrawMode.Lines;
            }
            Buffer.UnBind();

            Bound = new BoundingBox()
            {
                Min = new Math.Vector3(tmp.BoundingBox[0], tmp.BoundingBox[1], tmp.BoundingBox[2]),
                Max = new Math.Vector3(tmp.BoundingBox[3], tmp.BoundingBox[4], tmp.BoundingBox[5])
            };

        }

        public static int[] GetIndices(string filename, bool useVFS = false)
        {
            Model_m tmp = null;
            if (useVFS)
            {
                tmp = Serializer.Deserialize<Model_m>(VFS.FSReader.OpenFile(filename, false));
            }
            else
            {
                tmp = Serializer.Deserialize<Model_m>(System.IO.File.OpenRead(filename));
            }

            int[] a = new int[tmp.Mesh[0].indices.Length];

            for (int i = 0; i < a.Length; i++) a[i] = (int)tmp.Mesh[0].indices[i];

            return a;
        }

        public static float[] GetVertices(string filename, bool useVFS = false)
        {
            Model_m tmp = null;
            if (useVFS)
            {
                tmp = Serializer.Deserialize<Model_m>(VFS.FSReader.OpenFile(filename, false));
            }
            else
            {
                tmp = Serializer.Deserialize<Model_m>(System.IO.File.OpenRead(filename));
            }
            return tmp.Mesh[0].Vertices;
        }

        public static float[] GetNormals(string filename, bool useVFS = false)
        {
            Model_m tmp = null;
            if (useVFS)
            {
                tmp = Serializer.Deserialize<Model_m>(VFS.FSReader.OpenFile(filename, false));
            }
            else
            {
                tmp = Serializer.Deserialize<Model_m>(System.IO.File.OpenRead(filename));
            }
            return tmp.Mesh[0].normals;
        }

    }
}
