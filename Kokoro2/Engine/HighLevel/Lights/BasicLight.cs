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
    public abstract class BasicLight : IEngineObject
    {
        public bool CastShadows { get; set; }
        public int ShadowResolution { get; set; }

        public Vector4 AmbientColor { get; set; }
        public Vector4 LightColor { get; set; }

        public Matrix4 ShadowSpace { get; protected set; }

        private FrameBuffer shadowBuffer;
        private ShaderProgram shadowShader;

        public ShaderProgram ShadowShader
        {
            get
            {
                return shadowShader;
            }
        }

        public ulong ID
        {
            get; set;
        }

        public GraphicsContext ParentContext
        {
            get; set;
        }

        protected BasicLight(GraphicsContext context)
        {
            shadowShader = new ShaderProgram(context, VertexShader.Load("ShadowMap", context), FragmentShader.Load("ShadowMap", context));
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
                shadowBuffer.Add("Normals", new FrameBufferTexture(ShadowResolution, ShadowResolution, PixelFormat.BGRA, PixelComponentType.RGBA8, PixelType.Float, context), FrameBufferAttachments.ColorAttachment0, context);
                shadowBuffer.Add("Positions", new FrameBufferTexture(ShadowResolution, ShadowResolution, PixelFormat.BGRA, PixelComponentType.RGBA8, PixelType.Float, context), FrameBufferAttachments.ColorAttachment1, context);
                shadowBuffer["DepthBuffer"].FilterMode = TextureFilter.Linear;
                shadowBuffer["DepthBuffer"].Compare = true;
                shadowBuffer["DepthBuffer"].WrapX = false;
                shadowBuffer["DepthBuffer"].WrapY = false;
            }
        }

        protected abstract Matrix4 GetShadowShaderMatrix(GraphicsContext context);
        protected abstract Matrix4 GetShadowMapMatrix(GraphicsContext context);

        private bool passSetup = false;
        private CullMode prevCullMode;
        public void SetupShadowPass(GraphicsContext context)
        {
            if (CastShadows)
            {
                passSetup = true;
                prevCullMode = context.FaceCulling;
                context.FaceCulling = CullMode.Front;
                ShadowSpace = GetShadowMapMatrix(context);
                ShadowShader["sWVP"] = GetShadowShaderMatrix(context);
                shadowBuffer.Bind(context);
                context.ClearColor(1, 1, 1, 1);
                context.ClearDepth();
            }
        }

        public void EndShadowPass(GraphicsContext context)
        {
            if (CastShadows)
            {
                if (!passSetup) throw new InvalidOperationException("Setup the shadow pass first!");
                passSetup = false;
                shadowBuffer.UnBind(context);
                context.FaceCulling = prevCullMode;
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BasicLight() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
