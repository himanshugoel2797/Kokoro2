using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro2.Math;

namespace Kokoro2.Engine.HighLevel.Cameras
{
    /// <summary>
    /// Represents a Camera in the scene graph
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// The Camera's View Matrix
        /// </summary>
        public Matrix4 View { get; internal set; }

        /// <summary>
        /// The Camera's Projection Matrix
        /// </summary>
        public Matrix4 Projection { get; internal set; }

        public Model.BoundingVolume Frustum { get; internal set; }

        Vector3 pos;
        /// <summary>
        /// The 3D Position of the Camera
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
            }
        }

        /// <summary>
        /// Create a new Camera object
        /// </summary>
        public Camera()
        {
            View = Matrix4.LookAt(new Vector3(-1, 0, 0), Vector3.Zero, Vector3.UnitY);
            Position = -Vector3.UnitX;
            SetProjection(0.7853f, 16f / 9f, 0.1f, 1000f);
        }

        /// <summary>
        /// Update the camera instance
        /// </summary>
        /// <param name="interval">The time elapsed in ticks since the last update</param>
        /// <param name="Context">The current GraphicsContext</param>
        public virtual void Update(double interval, GraphicsContext Context)
        {
            Context.View = View;
            Context.Projection = Projection;
        }

        public void SetProjection(float fov, float aspectRatio, float nearClip, float farClip)
        {
            Projection = Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, nearClip, farClip);
            CalculateFrustum(fov, aspectRatio, nearClip, farClip);
        }

        public void CalculateFrustum(float fov, float aspectRatio, float nearClip, float farClip)
        {
            float preComp = 2 * (float)System.Math.Tan(fov / 2);
            float hNear = preComp * nearClip;
            float wNear = aspectRatio * hNear;
            float hFar = preComp * farClip;
            float wFar = hFar * aspectRatio;

            Frustum = new Model.BoundingVolume()
            {
                Max = new Vector3(wFar, hFar, farClip),
                Min = new Vector3(wNear, hNear, nearClip)
            };
        }
    }
}
