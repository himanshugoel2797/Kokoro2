﻿using System;
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
    public class ArcBallCamera : Camera
    {//TODO setup collisions

        public Vector3 Up;
        public float Radius
        {
            get; set;
        } = 100.0f;
        public Vector3 TargetPosition { get; set; }
        public bool Pannable { get; set; }

        float leftrightRot = MathHelper.PiOver2;
        float updownRot = -MathHelper.Pi / 10.0f;
        public float rotationSpeed = 0.2f;
        public float moveSpeed = 50f;
        Vector2 mousePos;

        public float RotationSpeed { get { return rotationSpeed; } set { rotationSpeed = value; } }
        public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
        public float ZoomSpeed { get; set; } = 1;

        /// <summary>
        /// Create a new First Person Camera
        /// </summary>
        /// <param name="Position">The Position of the Camera</param>
        /// <param name="Direction">The Direction the Camera initially faces</param>
        public ArcBallCamera(GraphicsContext context, Vector3 Position, Vector3 Direction) : base(context)
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
            //interval /= 100;

            if (Mouse.ButtonsDown.Left)
            {
                if (Pannable && Keyboard.IsKeyPressed(Key.LControl))
                {
                    Vector3 right = Vector3.Cross(Vector3.Normalize(Position), Up);
                    right.Normalize();
                    if (System.Math.Abs(mousePos.X - Mouse.MousePos.X) > 0) TargetPosition -= right * (float)MathHelper.DegreesToRadians(moveSpeed * (mousePos.X - Mouse.MousePos.X) * interval / 10000f);
                    if (System.Math.Abs(mousePos.Y - Mouse.MousePos.Y) > 0) TargetPosition -= Up * (float)MathHelper.DegreesToRadians(moveSpeed * Context.WindowSize.X / Context.WindowSize.Y * (mousePos.Y - Mouse.MousePos.Y) * interval / 10000f);

                }
                else {
                    if (System.Math.Abs(mousePos.X - Mouse.MousePos.X) > 0) leftrightRot -= (float)MathHelper.DegreesToRadians(rotationSpeed * (mousePos.X - Mouse.MousePos.X) * interval / 10000f);
                    if (System.Math.Abs(mousePos.Y - Mouse.MousePos.Y) > 0) updownRot -= (float)MathHelper.DegreesToRadians(rotationSpeed * Context.WindowSize.X / Context.WindowSize.Y * (mousePos.Y - Mouse.MousePos.Y) * interval / 10000f);
                }
            }
            else
            {
                mousePos = Mouse.MousePos;
            }

            Radius += Mouse.ScrollDelta * (float)System.Math.Sqrt(Radius) * ZoomSpeed;
            if (Pannable && Radius < 0.1f)
            {
                TargetPosition += Vector3.Normalize(Position) * Mouse.ScrollDelta;
            }
            if (Radius < 0.1f) Radius = 0.1f;

            float sin_Up = (float)System.Math.Sin(updownRot);
            float cos_Up = (float)System.Math.Cos(updownRot);
            float sin_LR = (float)System.Math.Sin(leftrightRot);
            float cos_LR = (float)System.Math.Cos(leftrightRot);


            float sin_Up_ = (float)System.Math.Sin(updownRot + MathHelper.PiOver2);
            float cos_Up_ = (float)System.Math.Cos(updownRot + MathHelper.PiOver2);
            float sin_LR_ = (float)System.Math.Sin(leftrightRot);
            float cos_LR_ = (float)System.Math.Cos(leftrightRot);


            Position = new Vector3(Radius * sin_LR * sin_Up, Radius * cos_Up, Radius * cos_LR * sin_Up);

            Up = new Vector3(sin_LR_ * sin_Up_, cos_Up_, cos_LR_ * sin_Up_);

            View = Matrix4.LookAt(Position + TargetPosition, TargetPosition, Up);
            Direction = Vector3.Normalize(-2 * TargetPosition - Position);

            base.Update(interval, Context);
        }
    }
}
