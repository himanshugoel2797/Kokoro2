using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if OPENGL
using Kokoro3.OpenGL;
#endif

namespace Kokoro3.Common
{
    public class RenderSurface
    {
#if OPENGL
        private FrameBuffer renderTarget;
        private TextureView[] boundTex;

        public RenderSurface(int width, int height)
        {
            renderTarget = new FrameBuffer();
            boundTex = new TextureView[FrameBuffer.MinimumAttachments];
        }

        public void AttachRenderTarget(int attachment, TextureView tex)
        {
            renderTarget.AttachTextureView((Engine.FrameBufferAttachments)attachment, tex, 0);
            boundTex[attachment] = tex;
        }

        public void AttachDepthBuffer()
        {
            //TODO create depth rendertarget and attach it to the framebuffer
            //TODO add size verification
            //TODO add framebuffer tests
        }

#endif        
    }
}
