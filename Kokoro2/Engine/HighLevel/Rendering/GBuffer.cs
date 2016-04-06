using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro2.Math;
using Kokoro2.Engine;
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
            buffer = new FrameBuffer(width, height, context);

            //Create the GBuffer texture targets
            buffer.Add("Shadow", FramebufferTextureSource.Create(width, height, 0, PixelComponentType.RGBA8, PixelType.Float, context), FrameBufferAttachments.ColorAttachment0, context);
            buffer.Add("WorldPos", FramebufferTextureSource.Create(width, height, 0, PixelComponentType.RGBA16f, PixelType.Float, context), FrameBufferAttachments.ColorAttachment1, context);
            buffer.Add("Normal", FramebufferTextureSource.Create(width, height, 0, PixelComponentType.RGBA8, PixelType.Float, context), FrameBufferAttachments.ColorAttachment2, context);
            buffer.Add("Color", FramebufferTextureSource.Create(width, height, 0, PixelComponentType.RGBA16f, PixelType.Float, context), FrameBufferAttachments.ColorAttachment3, context);
            buffer.Add("Specular", FramebufferTextureSource.Create(width, height, 0, PixelComponentType.RGBA8, PixelType.Float, context), FrameBufferAttachments.ColorAttachment4, context);
            buffer.Add("DepthBuffer", DepthTextureSource.Create(width, height, PixelComponentType.D32, context), FrameBufferAttachments.DepthAttachment, context);

            //TODO setup the new GBufferShader
            GBufferShader = new ShaderProgram(context, VertexShader.Load("Shadowed", context), FragmentShader.Load("Shadowed", context));
        }

        public void Add(string name, Texture tex, FrameBufferAttachments attachment, GraphicsContext context)
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
        }

    }
}
