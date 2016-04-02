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

        private FullScreenQuad fsq;
        private ShaderProgram horizontal, vertical;

        private FrameBuffer tmpBuffer;
        private FrameBuffer resultBuffer;

        public TextureBlurFilter(int width, int height, PixelComponentType pct, GraphicsContext context)
        {
            fsq = new FullScreenQuad();
            horizontal = new ShaderProgram(context, VertexShader.Load("FrameBuffer", context), FragmentShader.Load("BlurHorizontal", context));
            vertical = new ShaderProgram(context, VertexShader.Load("FrameBuffer", context), FragmentShader.Load("BlurVertical", context));

            tmpBuffer = new FrameBuffer(width, height, PixelComponentType.D32, context);
            tmpBuffer.Add("Hblurred", new FrameBufferTexture(width, height, PixelFormat.BGRA, pct, PixelType.Float, context), FrameBufferAttachments.ColorAttachment0, context);
            tmpBuffer["Hblurred"].WrapX = false;
            tmpBuffer["Hblurred"].WrapY = false;

            resultBuffer = new FrameBuffer(width, height, PixelComponentType.D32, context);
            resultBuffer.Add("Vblurred", new FrameBufferTexture(width, height, PixelFormat.BGRA, pct, PixelType.Float, context), FrameBufferAttachments.ColorAttachment0, context);
            resultBuffer["Vblurred"].WrapX = false;
            resultBuffer["Vblurred"].WrapY = false;
        }

        public Texture ApplyBlur(Texture t, GraphicsContext c)
        {
            tmpBuffer.Bind(c);
            c.Clear(0, 0, 0, 0);

            fsq.RenderInfo.PushShader(horizontal);
            fsq.Material.AlbedoMap = t;
            horizontal["blurSize"] = vertical["blurSize"] = BlurRadius;
            c.Draw(fsq);
            fsq.RenderInfo.PopShader();

            tmpBuffer.UnBind(c);
            resultBuffer.Bind(c);
            c.Clear(0, 0, 0, 0);

            fsq.RenderInfo.PushShader(vertical);
            fsq.Material.AlbedoMap = tmpBuffer["Hblurred"];
            c.Draw(fsq);
            fsq.RenderInfo.PopShader();

            resultBuffer.UnBind(c);

            return resultBuffer["Vblurred"];
        }
    }
}
