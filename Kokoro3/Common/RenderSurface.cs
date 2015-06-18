using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro3.Math;
#if OPENGL
using Kokoro3.OpenGL;
#endif

namespace Kokoro3.Common
{
    public class RenderSurface
    {
        public Vector2 Size;

#if OPENGL
        private FrameBuffer renderTarget;
        private Texture[] boundTex;

        public RenderSurface(int width, int height)
        {
            renderTarget = new FrameBuffer();
            boundTex = new Texture[FrameBuffer.MinimumAttachments];
            Size = new Vector2(width, height);
        }

        public void AttachRenderTarget(int attachment, Texture tex)
        {
            if (Size.X != tex.Size.X | Size.Y != tex.Size.Y) throw new ArgumentException("The Texture must be the same size as the RenderTarget");
            renderTarget.AttachTexture((Engine.FrameBufferAttachments)attachment, tex, 0);
            boundTex[attachment] = tex;
        }

        public void AttachDepthBuffer()
        {
            Texture tmp = Texture.Load2D((int)Size.X, (int)Size.Y, 0, Engine.PixelComponentType.D32, Engine.PixelFormat.Depth, Engine.PixelType.Float);
            renderTarget.AttachTexture(Engine.FrameBufferAttachments.DepthAttachment, tmp, 0);
        }

#endif        
    }
}
