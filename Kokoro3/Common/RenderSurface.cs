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

        public RenderSurface(int width, int height)
        {
            renderTarget = new FrameBuffer();
        }

        public void AttachRenderTarget(string id)

#endif        
    }
}
