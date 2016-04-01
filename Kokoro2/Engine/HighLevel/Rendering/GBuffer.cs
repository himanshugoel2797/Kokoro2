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
    public class GBuffer
    {
        FrameBuffer buffer;

        public ShaderProgram GBufferShader;

        public GBuffer(int width, int height, GraphicsContext context)
        {
            buffer = new FrameBuffer(width, height, PixelComponentType.RGBA16f, context);

            //Create the GBuffer texture targets
            buffer.Add("Shadow", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA8, PixelType.Float), FrameBufferAttachments.ColorAttachment0, context);
            buffer.Add("WorldPos", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA8, PixelType.UInt1010102), FrameBufferAttachments.ColorAttachment1, context);
            buffer.Add("Normal", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA8, PixelType.Float), FrameBufferAttachments.ColorAttachment2, context);
            buffer.Add("Color", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA16f, PixelType.Float), FrameBufferAttachments.ColorAttachment3, context);
            buffer.Add("Specular", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA8, PixelType.Float), FrameBufferAttachments.ColorAttachment4, context);

            //TODO setup the new GBufferShader
            GBufferShader = new ShaderProgram(VertexShader.Load("Shadowed"), FragmentShader.Load("Shadowed"));
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
            buffer.SetBlendFunc(func, FrameBufferAttachments.ColorAttachment3);
            buffer.SetBlendFunc(func, FrameBufferAttachments.ColorAttachment4);
        }

        public void UnBind(GraphicsContext context)
        {
            buffer.UnBind(context);
            //context.Viewport = new Vector4(0, 0, context.WindowSize.X, context.WindowSize.Y);
        }

    }
}
