using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public struct PhysicsInfo
    {
        public Physics.BaseEntity Entity;
        public Vector3 Position { get { return Entity.Position; } set { Entity.Position = value; } }
        public Quaternion Orientation { get { return Entity.Orientation; } set { Entity.Orientation = value; } }
    }
}
