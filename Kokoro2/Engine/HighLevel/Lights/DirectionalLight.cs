using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Lights
{
    public class DirectionalLight : BasicLight
    {
        public Vector3 Direction { get; set; }
        public BoundingBox ShadowBoxSize { get; set; }
        public Vector3 ShadowBoxLocation { get; set; }

        public DirectionalLight(GraphicsContext context, Vector3 direction) : base(context)
        {
            Direction = direction;
            ShadowBoxSize = new BoundingBox(new Vector3(-100, -100, -10), new Vector3(100, 100, 100));
            ShadowBoxLocation = Vector3.Zero;
        }

        protected override Matrix4 GetShadowShaderMatrix(GraphicsContext context)
        {
            Matrix4 p = Matrix4.CreateOrthographicOffCenter(ShadowBoxLocation.X + ShadowBoxSize.Min.X, ShadowBoxLocation.X + ShadowBoxSize.Max.X,
                                                            ShadowBoxLocation.Y + ShadowBoxSize.Min.Y, ShadowBoxLocation.Y + ShadowBoxSize.Max.Y,
                                                            ShadowBoxLocation.Z - ShadowBoxSize.Max.Z, ShadowBoxLocation.Z - ShadowBoxSize.Min.Z);

            p = Matrix4.CreateOrthographicOffCenter(ShadowBoxSize.Min.X, ShadowBoxSize.Max.X, ShadowBoxSize.Min.Y, ShadowBoxSize.Max.Y, -ShadowBoxSize.Max.Z, -ShadowBoxSize.Min.Z);

            Matrix4 v = Matrix4.LookAt(ShadowBoxLocation - Direction, ShadowBoxLocation, Vector3.UnitX);
            v = Matrix4.LookAt(Vector3.Zero, -Direction, Vector3.UnitX);

            return p * v;
        }

        protected override Matrix4 GetShadowMapMatrix(GraphicsContext context)
        {
            Matrix4 a = GetShadowShaderMatrix(context);
            Matrix4 b = new Matrix4(new Vector4(0.5f, 0, 0, 0),
                                    new Vector4(0, 0.5f, 0, 0),
                                    new Vector4(0, 0, 0.5f, 0),
                                    new Vector4(0.5f, 0.5f, 0.5f, 1.0f));

            return a;
        }
    }
}
