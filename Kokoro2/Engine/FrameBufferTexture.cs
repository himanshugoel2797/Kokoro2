using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public class FrameBufferTexture : Texture
    {
        public FrameBufferTexture(int width, int height, PixelFormat pf, PixelComponentType pct, PixelType pixelType, GraphicsContext c)
            : base(width, height, pf, pct, pixelType, c)
        {
        }

        public void BindToFrameBuffer(FrameBufferAttachments texUnit)
        {
            base.BindToFBuffer(texUnit);
        }

        public static void UnBindFromFrameBuffer(int texUnit)
        {
            UnBindFromFBuffer(texUnit);
        }

    }
}
