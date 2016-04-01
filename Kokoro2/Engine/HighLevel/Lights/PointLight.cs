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

        public float ConstantAttenuation { get { return cAtten; } set { cAtten = value; RecalculateMaxDistance(); } }
        public float LinearAttenuation { get { return lAtten; } set { lAtten = value; RecalculateMaxDistance(); } }
        public float QuadraticAttenuation { get { return qAtten; } set { qAtten = value; RecalculateMaxDistance(); } }
        private float cAtten = 0, lAtten = 0, qAtten = 0;

        const float FalloffClip = 0.001f;

        public PointLight(GraphicsContext c) : base(c)
        {
            ShadowResolution = 0;
            ConstantAttenuation = 1.0f;
            LinearAttenuation = 0.7f;
            QuadraticAttenuation = 1.8f;
        }

        private void RecalculateMaxDistance()
        {
            //Solve the attenuation equation, clipping at a falloff of 0.001f
            float max = System.Math.Max(LightColor.X, System.Math.Max(LightColor.Y, LightColor.Z));
            float atten = max / FalloffClip;
            MaxDistance = -lAtten + (float)System.Math.Sqrt(lAtten * lAtten - 4 * qAtten * (cAtten - atten)) / (2 * qAtten);
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
