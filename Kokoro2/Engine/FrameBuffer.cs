using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Math;

#if OPENGL
#if PC
using Kokoro2.OpenGL.PC;
#endif

#elif OPENGL_AZDO
#if PC
using Kokoro2.OpenGL.AZDO;
#endif

#endif

namespace Kokoro2.Engine
{
    /// <summary>
    /// The available FrameBuffer attachments
    /// </summary>
    public enum FrameBufferAttachments
    {
        ColorAttachment0,
        ColorAttachment1,
        ColorAttachment2,
        ColorAttachment3,
        ColorAttachment4,
        ColorAttachment5,
        ColorAttachment6,
        ColorAttachment7,
        ColorAttachment8,
        ColorAttachment9,
        ColorAttachment10,
        ColorAttachment11,
        ColorAttachment12,
        ColorAttachment13,
        ColorAttachment14,
        ColorAttachment15,
        DepthAttachment,
        DepthStencilAttachment,
        StencilAttachment
    }

    /// <summary>
    /// Represents a FrameBuffer object
    /// </summary>
    public class FrameBuffer : FrameBufferLL, IDisposable
    {
        /// <summary>
        /// The Width of the framebuffer
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The Height of the framebuffer
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The IDs of the associated RenderTargets
        /// </summary>
        public List<string> RenderTargets { get; set; }

        private Dictionary<string, Texture> fbufTextures;
        private Dictionary<string, int> fbufAttachmentsIDs;
        private Dictionary<string, FrameBufferAttachments> attachments;

        /// <summary>
        /// Create a new instance of a FrameBuffer Object and add a Depth Buffer and Color RenderTarget
        /// </summary>
        /// <param name="width">The Width of the FrameBuffer RenderTarget</param>
        /// <param name="height">The Height of the FrameBuffer RenderTarget</param>
        /// <param name="pct">The PixelComponentType of the FrameBuffer RenderTarget</param>
        /// <param name="context">The current GraphicsContext</param>
        public FrameBuffer(int width, int height, GraphicsContext context)
        {
            ParentContext = context;
            ParentContext.Disposing += Dispose;

            RenderTargets = new List<string>();
            fbufTextures = new Dictionary<string, Texture>();
            fbufAttachmentsIDs = new Dictionary<string, int>();
            attachments = new Dictionary<string, FrameBufferAttachments>();

            Width = width;
            Height = height;

            base.Generate();
            base.CheckError();

            Kokoro2.Engine.ObjectAllocTracker.NewCreated(this, "Framebuffer");
        }

        /// <summary>
        /// Add a new RenderTarget to the FrameBuffer
        /// </summary>
        /// <param name="id">The ID to assign to the RenderTarget</param>
        /// <param name="fbufTex">The FrameBufferTexture to set as the RenderTarget</param>
        /// <param name="attachment">The FrameBufferAttachment to attach it to</param>
        /// <param name="context">The current GraphicsContext</param>
        public void Add(string id, Texture fbufTex, FrameBufferAttachments attachment, GraphicsContext context)
        {
            if (fbufTex.Width != this.Width && fbufTex.Height != this.Height) throw new Exception("The dimensions of the FrameBufferTexture must be the same as the dimensions of the FrameBuffer");

            base.Bind();

            RenderTargets.Add(id);

            if (!fbufTextures.ContainsKey(id)) fbufTextures.Add(id, fbufTex);
            else fbufTextures[id] = fbufTex;

            if (attachment != FrameBufferAttachments.DepthAttachment && !attachments.ContainsValue(attachment))
            {
                if (!attachments.ContainsKey(id)) attachments.Add(id, attachment);
                else attachments[id] = attachment;
            }

            base.BindTexture(attachment, fbufTex);
            base.DrawBuffers(attachments.Values.ToArray());
            base.CheckError();

            base.Unbind();
        }

        /// <summary>
        /// Get/Set the RenderTargets bound to this FrameBuffer
        /// </summary>
        /// <param name="key">The ID of the RenderTarget</param>
        /// <returns>The RenderTarget</returns>
        public Texture this[string key]
        {
            get
            {
                return fbufTextures[key];
            }
            set
            {
                if (fbufTextures.ContainsKey(key)) fbufTextures[key] = value;
                else throw new InvalidOperationException("Call 'Add' to register a new framebuffer texture");
            }
        }

        /// <summary>
        /// Set the Blend Function for a RenderTarget
        /// </summary>
        /// <param name="func">The blend function</param>
        /// <param name="attachment">The FrameBufferAttachment to set the blend function to</param>
        public void SetBlendFunc(BlendFunc func, FrameBufferAttachments attachment)
        {
            int index = int.Parse(attachment.ToString().Replace("ColorAttachment", ""));
            base.BlendFunction(func, index);
        }

        /// <summary>
        /// Bind the FrameBuffer to the pipeline
        /// </summary>
        /// <param name="context">The current GraphicsContext</param>
        public void Bind(GraphicsContext context)
        {
            base.Bind();
            base.DrawBuffers(attachments.Values.ToArray());
            currentFBUF = this;
            context.Viewport = new Vector4(0, 0, Width, Height);
        }

        /// <summary>
        /// Delete the FrameBuffer Object
        /// </summary>
        /// <remarks>This does not delete the bound RenderTargets</remarks>
        public new void Dispose()
        {
            base.Dispose();
        }

        private static FrameBuffer currentFBUF;
        /// <summary>
        /// Get the currently set FrameBuffer
        /// </summary>
        /// <returns>The current FrameBuffer</returns>
        public static FrameBuffer GetCurrentFrameBuffer()
        {
            return currentFBUF;
        }

        /// <summary>
        /// Unbind the FrameBuffer from the pipeline
        /// </summary>
        public void UnBind(GraphicsContext context)
        {
            base.Unbind();
            context.Viewport = new Vector4(0, 0, context.WindowSize.X, context.WindowSize.Y);
        }

#if DEBUG
        ~FrameBuffer()
        {
            Kokoro2.Engine.ObjectAllocTracker.ObjectDestroyed(this);
        }
#endif

    }
}
