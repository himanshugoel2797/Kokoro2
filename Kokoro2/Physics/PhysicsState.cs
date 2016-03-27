using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Math;

namespace Kokoro2.Physics
{
    /// <summary>
    /// Stores the PhysicsState of a body
    /// </summary>
    public struct PhysicsState
    {
        /// <summary>
        /// The Mass (in kg) of the body
        /// </summary>
        public float Mass;
        /// <summary>
        /// Enable/Disable physics on this body
        /// </summary>
        public bool Enabled;
        /// <summary>
        /// The acceleration due to gravity this body experiences
        /// </summary>
        public Vector3? Gravity;

        /// <summary>
        /// The Position of this object
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The velocity at which this object is currently moving
        /// </summary>
        public Vector3 Velocity;

        /// <summary>
        /// The momentum of this object
        /// </summary>
        public Vector3 Momentum;

        /// <summary>
        /// The orientation of this object
        /// </summary>
        public Quaternion Orientation;

        /// <summary>
        /// The angular velocity of this object
        /// </summary>
        public Quaternion AngularVelocity;

        /// <summary>
        /// The angular momentum of this object
        /// </summary>
        public Quaternion AngularMomentum;
    }
}
