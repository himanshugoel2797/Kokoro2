using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Physics
{
    public abstract class PhysicsBody
    {
        internal BroadphaseSphere broadphase;

        public long ID { get; internal set; }
        public PhysicsState State;
        public PhysicsEvents Events;
        public PhysicsBody Parent;

        static long baseID = 0;
        public PhysicsBody()
        {
            ID = baseID++;
        }
        //Doesn't really implement anything, the main engine is responsible for determining exactly what kind of primitive this is

        internal abstract SphereOctree Voxelize();
    }
}
