using System;
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
        internal class MeshInfo_m
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

            [ProtoMember(10)]
            public float[] BoundingBox;
        }

        [ProtoContract]
        internal class Weight_m
        {
            [ProtoMember(1)]
            public float[] Weights;
        }

        [ProtoContract]
        internal class Bone_m
        {
            [ProtoMember(1)]
            public int[] Bones;
        }

        [ProtoContract]
        internal class Skeleton_m
        {
            [ProtoMember(1)]
            public float[] Skeletons;
        }

        [ProtoContract]
        internal class SceneGraph_m
        {
            [ProtoMember(1)]
            public SceneGraph_m[] Children;

            [ProtoMember(2)]
            public Matrix4 Transform;

            [ProtoMember(3)]
            public int[] Objects;
        }


        [ProtoContract]
        internal class Model_m
        {
            [ProtoMember(3)]
            public SceneGraph_m Scene;

            [ProtoMember(1)]
            public MeshInfo_m[] Mesh;

        }
        #endregion

        private bool animatedMesh;

        internal VertexMesh(MeshInfo_m m, GraphicsContext c) : base(c)
        {
            Buffer.Bind();
            SetIndices(m.indices, 0);
            SetUVs(m.uvs, 0);
            SetVertices(m.Vertices, 0);
            SetNormals(m.normals, 0);

            if (m.isLine) DrawMode = DrawMode.Lines;
            Buffer.UnBind();

            Bound = new BoundingBox()
            {
                Min = new Math.Vector3(m.BoundingBox[0], m.BoundingBox[1], m.BoundingBox[2]),
                Max = new Math.Vector3(m.BoundingBox[3], m.BoundingBox[4], m.BoundingBox[5])
            };
        }


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
                Min = new Math.Vector3(tmp.Mesh[0].BoundingBox[0], tmp.Mesh[0].BoundingBox[1], tmp.Mesh[0].BoundingBox[2]),
                Max = new Math.Vector3(tmp.Mesh[0].BoundingBox[3], tmp.Mesh[0].BoundingBox[4], tmp.Mesh[0].BoundingBox[5])
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

        public static float[] GetUVs(string filename, bool useVFS = false)
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
            return tmp.Mesh[0].uvs;
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
