using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.Physics
{
    public class MobileMesh : BaseEntity
    {
        public enum Solidity
        {
            Clockwise = BEPUphysics.CollisionShapes.MobileMeshSolidity.Clockwise,
            CounterClockwise = BEPUphysics.CollisionShapes.MobileMeshSolidity.Counterclockwise,
            DoubleSided = BEPUphysics.CollisionShapes.MobileMeshSolidity.DoubleSided,
            Solid = BEPUphysics.CollisionShapes.MobileMeshSolidity.Solid
        };

        public MobileMesh(float[] verts, int[] indices, float Scale, Solidity s, float mass = 0) : base(Process(verts, indices, Scale, s, mass)) { }

        private static BEPUphysics.Entities.Entity Process(float[] verts, int[] indices, float sc, Solidity s, float mass)
        {
            List<BEPUutilities.Vector3> v = new List<BEPUutilities.Vector3>();
            for (int i = 0; i < verts.Length; i += 3)
            {
                v.Add(new BEPUutilities.Vector3(verts[i] * sc, verts[i + 1] * sc, verts[i + 2] * sc));
            }

            if (mass != 0) return new BEPUphysics.Entities.Prefabs.MobileMesh(v.ToArray(), indices, BEPUutilities.AffineTransform.Identity, (BEPUphysics.CollisionShapes.MobileMeshSolidity)s, mass);
            else return new BEPUphysics.Entities.Prefabs.MobileMesh(v.ToArray(), indices, BEPUutilities.AffineTransform.Identity, (BEPUphysics.CollisionShapes.MobileMeshSolidity)s);
        }
    }
}
