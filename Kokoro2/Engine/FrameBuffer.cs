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
        /// The resolution of the framebuffer
        /// </summary>
        public Vector2 Size { get; internal set; }
        /// <summary>
        /// The IDs of the associated RenderTargets
        /// </summary>
        public List<string> RenderTargets { get; set; }

        private Dictionary<string, FrameBufferTexture> fbufTextures;
        private Dictionary<string, int> fbufAttachmentsIDs;
        private Dictionary<string, FrameBufferAttachments> attachments;
        private int id;

        /// <summary>
        /// Create a new instance of a FrameBuffer Object and add a Depth Buffer and Color RenderTarget
        /// </summary>
        /// <param name="width">The Width of the FrameBuffer RenderTarget</param>
        /// <param name="height">The Height of the FrameBuffer RenderTarget</param>
        /// <param name="pct">The PixelComponentType of the FrameBuffer RenderTarget</param>
        /// <param name="context">The current GraphicsContext</param>
        public FrameBuffer(int width, int height, PixelComponentType pct, GraphicsContext context)
        {
            RenderTargets = new List<string>();
            fbufTextures = new Dictionary<string, FrameBufferTexture>();
            fbufAttachmentsIDs = new Dictionary<string, int>();
            attachments = new Dictionary<string, FrameBufferAttachments>();

            Size = new Vector2(width, height);

            id = base.Generate();

            Add("Color", new FrameBufferTexture(width, height, PixelFormat.BGRA, pct, PixelType.Float), FrameBufferAttachments.ColorAttachment0, context);
            Add("DepthBuffer", new FrameBufferTexture(width, height, PixelFormat.Depth, PixelComponentType.D32, PixelType.Float), FrameBufferAttachments.DepthAttachment, context); //Attach the depth buffer to the framebuffer

            base.CheckError();

            Kokoro2.Debug.ObjectAllocTracker.NewCreated(this, id, "Framebuffer");
        }

        /// <summary>
        /// Add a new RenderTarget to the FrameBuffer
        /// </summary>
        /// <param name="id">The ID to assign to the RenderTarget</param>
        /// <param name="fbufTex">The FrameBufferTexture to set as the RenderTarget</param>
        /// <param name="attachment">The FrameBufferAttachment to attach it to</param>
        /// <param name="context">The current GraphicsContext</param>
        public void Add(string id, FrameBufferTexture fbufTex, FrameBufferAttachments attachment, GraphicsContext context)
        {

            if (fbufTex.Size != this.Size) throw new Exception("The dimensions of the FrameBufferTexture must be the same as the dimensions of the FrameBuffer");
            base.Bind(this.id);

            RenderTargets.Add(id);

            if (!fbufTextures.ContainsKey(id)) fbufTextures.Add(id, fbufTex);
            else fbufTextures[id] = fbufTex;

            if (attachment != FrameBufferAttachments.DepthAttachment && !attachments.ContainsValue(attachment))
            {
                if (!attachments.ContainsKey(id)) attachments.Add(id, attachment);
                else attachments[id] = attachment;
            }

            fbufTex.BindToFrameBuffer(attachment);
            base.DrawBuffers(attachments.Values.ToArray());
            base.CheckError();

            base.Bind(0);
        }

        /// <summary>
        /// Get/Set the RenderTargets bound to this FrameBuffer
        /// </summary>
        /// <param name="key">The ID of the RenderTarget</param>
        /// <returns>The RenderTarget</returns>
        public FrameBufferTexture this[string key]
        {
            get
            {
                return fbufTextures[key];
            }
            set
            {
                fbufTextures[key] = value;
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
            if (id != -1)
            {

                base.Bind(id);
                base.DrawBuffers(attachments.Values.ToArray());
                currentFBUF = this;
                context.Viewport = new Vector4(0, 0, Size.X, Size.Y);
            }
        }

        /// <summary>
        /// Delete the FrameBuffer Object
        /// </summary>
        /// <remarks>This does not delete the bound RenderTargets</remarks>
        public void Dispose()
        {
            base.Delete(id);
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
            base.Bind(0);
            context.Viewport = new Vector4(0, 0, context.WindowSize.X, context.WindowSize.Y);
        }

#if DEBUG
        ~FrameBuffer()
        {
            Kokoro2.Debug.ObjectAllocTracker.ObjectDestroyed(this, id, "FrameBuffer");
        }
#endif

    }
}
