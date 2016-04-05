using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Math;

namespace Kokoro2.Engine.HighLevel.Lights
{
    public class PointLight : BasicLight
    {
        public float MaxDistance { get; set; }
        public Vector3 Position { get; set; }
        public float Attenuation { get { return atten; } set { atten = value; RecalculateMaxDistance(); } }
        private float atten;

        const float FalloffClip = 0.01f;

        public PointLight(GraphicsContext c) : base(c)
        {
            ShadowResolution = 0;
        }

        private void RecalculateMaxDistance()
        {
            //Solve the attenuation equation, clipping at a falloff of 0.001f
            float max = System.Math.Max(LightColor.X, System.Math.Max(LightColor.Y, LightColor.Z));
            /*MaxDistance = 25f + (float)(-lAtten + System.Math.Sqrt(lAtten * lAtten - 4 * qAtten * (cAtten - (256.0 / 5.0) * max)))
  / (2 * qAtten);*/
            MaxDistance = (float)System.Math.Sqrt(max / (Attenuation * FalloffClip));
        }

        protected override Matrix4 GetShadowMapMatrix(GraphicsContext context)
        {
            throw new NotImplementedException();
        }

        protected override Matrix4 GetShadowShaderMatrix(GraphicsContext context)
        {
            throw new NotImplementedException();
        }
    }
}
