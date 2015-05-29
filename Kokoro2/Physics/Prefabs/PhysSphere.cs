using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Math;

namespace Kokoro2.Physics.Prefabs
{
    /// <summary>
    /// Creates a new Sphere object
    /// </summary>
    public class PhysSphere : ICollisionBody
    {
        public Vector3 Center;
        public float Radius;

        /// <summary>
        /// Create a new instance of a sphere object
        /// </summary>
        /// <param name="radius">The radius of the sphere</param>
        public PhysSphere(float radius)
        {
            Center = Vector3.Zero;
            Radius = radius;
        }
    }
}
