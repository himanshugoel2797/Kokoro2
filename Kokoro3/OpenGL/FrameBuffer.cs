#if OPENGL
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public class FrameBuffer : IDisposable
    {
        internal int ID;
        internal List<DrawBuffersEnum> fbufAttachments;

        public const int MinimumAttachments = 8;

        public FrameBuffer()
        {
            ID = GL.GenFramebuffer();
            fbufAttachments = new List<DrawBuffersEnum>();
        }

        public void AttachTexture(Engine.FrameBufferAttachments attachment, Texture t, int level)
        {
            //Maintain state
            int prevFBuf = LLDevice.BindFrameBuffer(ID);

            //Update DrawBuffers storage
            DrawBuffersEnum att = EnumConverters.EDrawBufferAttachment(attachment);
            if (!fbufAttachments.Contains(att)) fbufAttachments.Add(att);
            fbufAttachments.Sort();

            GL.FramebufferTexture(FramebufferTarget.Framebuffer, EnumConverters.EFrameBufferAttachment(attachment), t.ID, level);
            GL.DrawBuffers(fbufAttachments.Count, fbufAttachments.ToArray());
#if DEBUG
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete) throw new Exception(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer).ToString());
#endif
            LLDevice.BindFrameBuffer(prevFBuf);
        }

        public void RemoveAttachment(Engine.FrameBufferAttachments attachment, int level)
        {
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, EnumConverters.EFrameBufferAttachment(attachment), 0, level);
            fbufAttachments.Remove(EnumConverters.EDrawBufferAttachment(attachment));
            fbufAttachments.Sort();
            GL.DrawBuffers(fbufAttachments.Count, fbufAttachments.ToArray());
        }

        public void Dispose()
        {
            GL.DeleteFramebuffer(ID);
            ID = 0;
            GC.SuppressFinalize(this);
        }

        ~FrameBuffer()
        {
            System.Diagnostics.Debug.WriteLine($"[WARN] The FrameBuffer object {ID} was automatically disposed");
            Dispose();
        }
    }
}
#endif