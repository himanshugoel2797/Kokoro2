using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Kokoro2.Physics.Prefabs
{
    public class PhysVertexMesh : ICollisionBody
    {
        #region Model Structures
        [ProtoContract]
        class MeshInfo_m
        {
            [ProtoMember(1)]
            public float[] Vertices;

            [ProtoMember(3)]
            public float[] normals;

            [ProtoMember(4)]
            public uint[] indices;
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

        public VObject VerletObj;
        public PhysSphere Bound;

        public PhysVertexMesh(string filename, bool animated)
        {
            Model_m tmp = Serializer.Deserialize<Model_m>(VFS.FSReader.OpenFile(filename, false));

            //TODO Deconstruct mesh into Verlet representation
        }

    }
}
