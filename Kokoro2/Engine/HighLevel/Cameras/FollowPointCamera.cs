using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro2.Math;
using Kokoro2.Engine.Input;

namespace Kokoro2.Engine.HighLevel.Cameras
{
    /// <summary>
    /// Represents a First Person Camera
    /// </summary>
    public class FollowPointCamera : Camera
    {//TODO setup collisions

        public Vector3 Up;

#if DEBUG
        double moveSpeedGradient = 1;
#endif

        /// <summary>
        /// Create a new First Person Camera
        /// </summary>
        /// <param name="Position">The Position of the Camera</param>
        /// <param name="Direction">The Direction the Camera initially faces</param>
        public FollowPointCamera(GraphicsContext context, Vector3 Position, Vector3 Direction) : base(context)
        {
            this.Position = Position;
            this.Direction = Direction;
            this.Up = Vector3.UnitY;
            View = Matrix4.LookAt(Position, Position + Direction, Up);
        }

        /// <summary>
        /// Update the camera instance
        /// </summary>
        /// <param name="interval">The time elapsed in ticks since the last update</param>
        /// <param name="Context">The current GraphicsContext</param>
        public override void Update(double interval, GraphicsContext Context)
        {
            View = Matrix4.LookAt(Position, Position + Direction, Up);
            base.Update(interval, Context);
        }
    }
}
