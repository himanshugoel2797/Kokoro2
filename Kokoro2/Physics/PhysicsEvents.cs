using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Physics
{
    public struct PhysicsEvents
    {
        public delegate void CollisionDetectedHandler(PhysicsBody a, PhysicsBody b);
        public event CollisionDetectedHandler CollisionDetected;


    }
}
