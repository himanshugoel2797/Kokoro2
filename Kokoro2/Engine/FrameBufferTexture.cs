using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro2.Debug;

namespace Kokoro2.Engine
{
    public class FrameBufferTexture : Texture
    {
        public FrameBufferTexture(int width, int height, PixelFormat pf, PixelComponentType pct, PixelType pixelType)
            : base(width, height, pf, pct, pixelType)
        {
        }

        public void BindToFrameBuffer(FrameBufferAttachments texUnit)
        {
            base.BindToFBuffer(texUnit, id);
        }

        public static void UnBindFromFrameBuffer(int texUnit)
        {
            UnBindFromFBuffer(texUnit);
        }

    }
}
