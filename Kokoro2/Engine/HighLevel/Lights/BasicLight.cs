using Kokoro2.Engine.Prefabs;
using Kokoro2.Engine.Shaders;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Lights
{
    public abstract class BasicLight
    {
        public bool CastShadows { get; set; }
        public int ShadowResolution { get; set; }

        public Vector4 AmbientColor { get; set; }
        public Vector4 LightColor { get; set; }

        public Matrix4 ShadowSpace { get; protected set; }

        private FrameBuffer shadowBuffer;
        private static ShaderProgram shadowShader;

        public ShaderProgram ShadowShader
        {
            get
            {
                return shadowShader;
            }
        }

        static BasicLight()
        {
            shadowShader = new ShaderProgram(VertexShader.Load("ShadowMap"), FragmentShader.Load("ShadowMap"));
        }

        protected BasicLight(GraphicsContext context)
        {
            AmbientColor = new Vector4(0.2f, 0.2f, 0.3f, 0.4f);
            LightColor = Vector4.One;
            ShadowResolution = 4096;
            InitializeShadowBuffer(context);
        }

        public void InitializeShadowBuffer(GraphicsContext context)
        {
            if (CastShadows)
            {
                if (shadowBuffer != null) shadowBuffer.Dispose();
                shadowBuffer = new FrameBuffer(ShadowResolution, ShadowResolution, PixelComponentType.D32, context);
                shadowBuffer.Add("Normals", new FrameBufferTexture(ShadowResolution, ShadowResolution, PixelFormat.BGRA, PixelComponentType.RGBA8, PixelType.Float), FrameBufferAttachments.ColorAttachment0, context);
                shadowBuffer.Add("Positions", new FrameBufferTexture(ShadowResolution, ShadowResolution, PixelFormat.BGRA, PixelComponentType.RGBA8, PixelType.Float), FrameBufferAttachments.ColorAttachment1, context);
                shadowBuffer["DepthBuffer"].FilterMode = TextureFilter.Linear;
                shadowBuffer["DepthBuffer"].Compare = true;
            }
        }

        protected abstract Matrix4 GetShadowShaderMatrix(GraphicsContext context);
        protected abstract Matrix4 GetShadowMapMatrix(GraphicsContext context);

        public void SetupShadowPass(GraphicsContext context)
        {
            if (CastShadows)
            {
                context.FaceCulling = CullMode.Off;
                ShadowSpace = GetShadowMapMatrix(context);
                ShadowShader["sWVP"] = GetShadowShaderMatrix(context);
                shadowBuffer.Bind(context);
                context.Clear(1, 1, 1, 1);
            }
        }

        public void EndShadowPass(GraphicsContext context)
        {
            if (CastShadows)
            {
                context.FaceCulling = CullMode.Back;
                shadowBuffer.UnBind(context);
            }
        }

        public FrameBufferTexture GetShadowMap()
        {
            return shadowBuffer["DepthBuffer"];
        }

        public FrameBufferTexture GetNormals()
        {
            return shadowBuffer["Normals"];
        }

        public FrameBufferTexture GetPositions()
        {
            return shadowBuffer["Positions"];
        }
    }
}
