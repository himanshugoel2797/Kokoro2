﻿using Kokoro2.Math;
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

            Vector3 sz = ShadowBoxSize.Max - ShadowBoxSize.Min;
            Vector3 sbL = new Vector3(ShadowBoxLocation.Y, ShadowBoxLocation.X, ShadowBoxLocation.Z);
            p = Matrix4.CreateOrthographic(sz.Z, sz.X, -sz.Y, sz.Y);
            //p = Matrix4.CreateOrthographic(-400, -400, -200, 200);

            Matrix4 v = Matrix4.LookAt(ShadowBoxLocation + Direction, ShadowBoxLocation, Vector3.UnitX);
            v = Matrix4.LookAt(Vector3.Zero + ShadowBoxLocation, Direction + ShadowBoxLocation, Vector3.UnitX);

            return v * p;
        }

        protected override Matrix4 GetShadowMapMatrix(GraphicsContext context)
        {
            Matrix4 a = GetShadowShaderMatrix(context);
            
            return a;
        }
    }
}
