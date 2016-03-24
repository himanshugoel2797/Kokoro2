using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics.Entities.Prefabs;
using Kokoro2.Math;
using BEPUphysics.BroadPhaseEntries;

namespace Kokoro2.Physics
{
    public class BEPUBody<T> : ICollisionBody where T : BEPUphysics.Entities.Entity
    {
        internal BEPUphysics.Entities.Entity body;

        public Vector3 Position
        {
            get
            {
                return new Vector3(body.Position.X, body.Position.Y, body.Position.Z);
            }
            set
            {
                body.Position = value;
            }
        }

        public Matrix4 World
        {
            get
            {
                return body.WorldTransform;
            }
        }

        public BEPUBody(T b)
        {
            this.body = b;
        }

        public static BEPUBody<ConvexHull> Load(string file)
        {
            float[] verts = Kokoro2.Engine.Prefabs.VertexMesh.GetVertices(file);

            List<BEPUutilities.Vector3> vertices = new List<BEPUutilities.Vector3>();

            for (int i = 0; i < verts.Length; i += 3)
            {
                vertices.Add(new BEPUutilities.Vector3(verts[i], verts[i + 1], verts[i + 2]));
            }

            return new BEPUBody<ConvexHull>(new ConvexHull(vertices));
        }
        
    }
}
