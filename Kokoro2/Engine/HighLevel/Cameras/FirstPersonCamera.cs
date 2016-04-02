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
    public class FirstPersonCamera : Camera
    {//TODO setup collisions

        public Vector3 Up;

        float leftrightRot = MathHelper.PiOver2;
        float updownRot = -MathHelper.Pi / 10.0f;
        public float rotationSpeed = 0.2f;
        public float moveSpeed = 50f;
        Vector2 mousePos;
        Vector3 cameraRotatedUpVector;

#if DEBUG
        double moveSpeedGradient = 1;
#endif

        /// <summary>
        /// Create a new First Person Camera
        /// </summary>
        /// <param name="Position">The Position of the Camera</param>
        /// <param name="Direction">The Direction the Camera initially faces</param>
        public FirstPersonCamera(GraphicsContext context, Vector3 Position, Vector3 Direction) : base(context)
        {
            this.Position = Position;
            this.Direction = Direction;
            this.Up = Vector3.UnitY;
            View = Matrix4.LookAt(Position, Position + Direction, Up);
        }

        private Matrix4 UpdateViewMatrix()
        {
            Matrix4 cameraRotation = Matrix4.CreateRotationX(updownRot) * Matrix4.CreateRotationY(leftrightRot);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);

            Direction = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = Position + Direction;

            cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            return Matrix4.LookAt(Position, cameraFinalTarget, cameraRotatedUpVector);
        }

        /// <summary>
        /// Update the camera instance
        /// </summary>
        /// <param name="interval">The time elapsed in ticks since the last update</param>
        /// <param name="Context">The current GraphicsContext</param>
        public override void Update(double interval, GraphicsContext Context)
        {
            //interval /= 100;

            if (Mouse.ButtonsDown.Left)
            {
                if (System.Math.Abs(mousePos.X - Mouse.MousePos.X) > 0) leftrightRot -= (float)MathHelper.DegreesToRadians(rotationSpeed * (mousePos.X - Mouse.MousePos.X) * interval / 10000f);
                if (System.Math.Abs(mousePos.Y - Mouse.MousePos.Y) > 0) updownRot -= (float)MathHelper.DegreesToRadians(rotationSpeed * (mousePos.Y - Mouse.MousePos.Y) * interval / 10000f);
            }
            else
            {
                mousePos = Mouse.MousePos;
            }
            UpdateViewMatrix();
            Vector3 Right = Vector3.Cross(cameraRotatedUpVector, Direction);

            if (Keyboard.IsKeyPressed(Key.Up))
            {
                Position += Direction * (float)(moveSpeed * interval / 10000f);
            }
            else if (Keyboard.IsKeyPressed(Key.Down))
            {
                Position -= Direction * (float)(moveSpeed * interval / 10000f);
            }

            if (Keyboard.IsKeyPressed(Key.Left) || Keyboard.IsKeyPressed(Key.A))
            {
                Position -= Right * (float)(moveSpeed * interval / 10000f);
            }
            else if (Keyboard.IsKeyPressed(Key.Right) || Keyboard.IsKeyPressed(Key.D))
            {
                Position += Right * (float)(moveSpeed * interval / 10000f);
            }

#if DEBUG
            if (Keyboard.IsKeyPressed(Key.PageDown) || Keyboard.IsKeyPressed(Key.S))
            {
                Position -= cameraRotatedUpVector * (float)(moveSpeed * interval / 10000f);
            }
            else if (Keyboard.IsKeyPressed(Key.PageUp) || Keyboard.IsKeyPressed(Key.W))
            {
                Position += cameraRotatedUpVector * (float)(moveSpeed * interval / 10000f);
            }

            if (Keyboard.IsKeyPressed(Key.LBracket))
            {
                moveSpeedGradient += 5;
            }
            else if (Keyboard.IsKeyPressed(Key.RBracket))
            {
                moveSpeedGradient -= 5;
            }

            if (Keyboard.IsKeyPressed(Key.Home))
            {
                moveSpeed += (float)moveSpeedGradient / 1000f;
                ErrorLogger.AddMessage(0, "[INPUT] moveSpeed = " + moveSpeed, DebugType.Marker, Severity.Notification);
            }
            else if (Keyboard.IsKeyPressed(Key.End))
            {
                moveSpeed -= (float)moveSpeedGradient / 1000f;
                ErrorLogger.AddMessage(0, "[INPUT] moveSpeed = " + moveSpeed, DebugType.Marker, Severity.Notification);
            }
#endif
            //View = UpdateViewMatrix();
            this.Up = cameraRotatedUpVector;
            View = Matrix4.LookAt(Position, Position + Direction, cameraRotatedUpVector);
            base.Update(interval, Context);
        }
    }
}
