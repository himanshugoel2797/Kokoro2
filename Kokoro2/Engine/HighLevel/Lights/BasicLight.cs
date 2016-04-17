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
        public Vector3 LightColor { get; set; }

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
            LightColor = Vector3.One;
            ShadowResolution = 1024;
            InitializeShadowBuffer(context);
        }

        public void InitializeShadowBuffer(GraphicsContext context)
        {
            if (CastShadows)
            {
                if (shadowBuffer != null) shadowBuffer.Dispose();
                shadowBuffer = new FrameBuffer(ShadowResolution, ShadowResolution, context);
                shadowBuffer.Add("Normals", FramebufferTextureSource.Create(ShadowResolution, ShadowResolution, 0, PixelComponentType.RGBA8, PixelType.Float, context), FrameBufferAttachments.ColorAttachment0, context);
                shadowBuffer.Add("Positions", FramebufferTextureSource.Create(ShadowResolution, ShadowResolution, 0, PixelComponentType.RGBA8, PixelType.Float, context), FrameBufferAttachments.ColorAttachment1, context);
                shadowBuffer.Add("DepthBuffer", DepthTextureSource.Create(ShadowResolution, ShadowResolution, PixelComponentType.D32, context), FrameBufferAttachments.DepthAttachment, context);
                shadowBuffer["DepthBuffer"].FilterMode = TextureFilter.Linear;
                shadowBuffer["DepthBuffer"].Compare = false;
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

        public Texture GetShadowMap()
        {
            return shadowBuffer["DepthBuffer"];
        }

        public Texture GetColors()
        {
            return shadowBuffer["Normals"];
        }

        public Texture GetPositions()
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
