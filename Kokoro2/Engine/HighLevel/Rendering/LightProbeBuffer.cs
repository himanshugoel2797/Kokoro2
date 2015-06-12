using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro2.Math;
using Kokoro2.Engine;
using Kokoro2.Debug;
using Kokoro2.Engine.SceneGraph;
using Kokoro2.Engine.Prefabs;
using Kokoro2.Engine.Shaders;
using Kokoro2.Engine.HighLevel.Cameras;

namespace Kokoro2.Engine.HighLevel.Rendering
{
    public class LightProbeBuffer
    {
        FrameBuffer buffer, GIBuffer;

        public ShaderProgram LightProbeShader;

        public LightProbeBuffer(int width, int height, GraphicsContext context)
        {
            buffer = new FrameBuffer(width, height, PixelComponentType.RGBA16f, context);

            //Create the GBuffer texture targets
            buffer.Add("RGBA0", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA16f, PixelType.Float), FrameBufferAttachments.ColorAttachment0, context);
            buffer.Add("Depth0", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA16f, PixelType.UInt1010102), FrameBufferAttachments.ColorAttachment1, context);
            buffer.Add("Normal0", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA16f, PixelType.Float), FrameBufferAttachments.ColorAttachment2, context);
            buffer.Add("Material0", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA16f, PixelType.Float), FrameBufferAttachments.ColorAttachment3, context);


            GIBuffer = new FrameBuffer(width, height, PixelComponentType.RGBA16f, context);

            GIBuffer.Add("SMAO", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA16f, PixelType.Float), FrameBufferAttachments.ColorAttachment0, context);
            GIBuffer.Add("SMIL", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA16f, PixelType.Float), FrameBufferAttachments.ColorAttachment0, context);

            //SMIL solution is simple, first render scene from light probe PoV, then render everything to texture from camera's PoV, before lighting, run a screen space pass which samples the two and performs a ray trace to calculate indirect lighting, blur and overlay on top

            //TODO setup the new GBufferShader
            LightProbeShader = new ShaderProgram(VertexShader.Load("GBuffer"), FragmentShader.Load("GBuffer")); //TODO fix this
        }

        public void Add(string name, FrameBufferTexture tex, FrameBufferAttachments attachment, GraphicsContext context)
        {
            buffer.Add(name, tex, attachment, context);
        }

        public Texture this[string key]
        {
            get
            {
                return buffer[key];
            }
        }

        public void Bind(GraphicsContext context)
        {
            buffer.Bind(context);
            /*context.Blending = new BlendFunc()
            {
                Src = BlendingFactor.One,
                Dst = BlendingFactor.Zero
            };*/

            //buffer.SetBlendFunc(context.Blending, FrameBufferAttachments.ColorAttachment0);
        }

        public void SetBlendFunc(BlendFunc func)
        {
            buffer.SetBlendFunc(func, FrameBufferAttachments.ColorAttachment0);
            buffer.SetBlendFunc(func, FrameBufferAttachments.ColorAttachment1);
            buffer.SetBlendFunc(func, FrameBufferAttachments.ColorAttachment2);
        }

        public void UnBind(GraphicsContext context)
        {
            buffer.UnBind(context);
            //context.Viewport = new Vector4(0, 0, context.WindowSize.X, context.WindowSize.Y);
        }

    }
}
