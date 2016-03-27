using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Physics.Prefabs
{
    public class Box : PhysicsBody
    {
        public Vector3 Dimensions;
        private Vector3 HalfDimensions;

        public Box(Vector3 Position, float w, float h, float l) : base()
        {
            this.State.Position = Position;
            Dimensions = new Vector3(w, h, l);
            HalfDimensions = Dimensions / 2.0f;

            broadphase.Radius = System.Math.Max(w, System.Math.Max(h, l));

        }

        internal override SphereOctree Voxelize()
        {
            float side = System.Math.Max(Dimensions.X, System.Math.Max(Dimensions.Y, Dimensions.Z));
            SphereOctree s = new SphereOctree(side, 5);

            float step = side / (1 << 5);

            

            return s;
        }
    }
}
