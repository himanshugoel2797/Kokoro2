using Kokoro2.Engine.Prefabs;
using Kokoro2.Engine.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Rendering
{
    public class TextureBlurFilter
    {
        public float BlurRadius
        {
            get; set;
        } = 0.005f;

        private static FullScreenQuad fsq;
        private static ShaderProgram horizontal, vertical;

        private FrameBuffer tmpBuffer;
        private FrameBuffer resultBuffer;

        static TextureBlurFilter()
        {
            fsq = new FullScreenQuad();
            horizontal = new ShaderProgram(VertexShader.Load("FrameBuffer"), FragmentShader.Load("BlurHorizontal"));
            vertical = new ShaderProgram(VertexShader.Load("FrameBuffer"), FragmentShader.Load("BlurVertical"));
        }

        public TextureBlurFilter(int width, int height, PixelComponentType pct, GraphicsContext context)
        {
            tmpBuffer = new FrameBuffer(width, height, PixelComponentType.D32, context);
            tmpBuffer.Add("Hblurred", new FrameBufferTexture(width, height, PixelFormat.BGRA, pct, PixelType.Float), FrameBufferAttachments.ColorAttachment0, context);
            tmpBuffer["Hblurred"].WrapX = false;
            tmpBuffer["Hblurred"].WrapY = false;

            resultBuffer = new FrameBuffer(width, height, PixelComponentType.D32, context);
            resultBuffer.Add("Vblurred", new FrameBufferTexture(width, height, PixelFormat.BGRA, pct, PixelType.Float), FrameBufferAttachments.ColorAttachment0, context);
            resultBuffer["Vblurred"].WrapX = false;
            resultBuffer["Vblurred"].WrapY = false;
        }

        public Texture ApplyBlur(Texture t, GraphicsContext c)
        {
            tmpBuffer.Bind(c);
            c.Clear(0, 0, 0, 0);

            fsq.PushShader(horizontal);
            fsq.AlbedoMap = t;
            horizontal["blurSize"] = vertical["blurSize"] = BlurRadius;
            fsq.Draw(c);
            fsq.PopShader();

            tmpBuffer.UnBind(c);
            resultBuffer.Bind(c);
            c.Clear(0, 0, 0, 0);

            fsq.PushShader(vertical);
            fsq.AlbedoMap = tmpBuffer["Hblurred"];
            fsq.Draw(c);
            fsq.PopShader();

            resultBuffer.UnBind(c);

            return resultBuffer["Vblurred"];
        }
    }
}
