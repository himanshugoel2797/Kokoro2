#if OPENGL && PC

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Kokoro2.OpenGL.PC
{
    public class FrameBufferLL : Engine.IEngineObject
    {
        public ulong ID
        {
            get;
            set;
        }

        public Engine.GraphicsContext ParentContext
        {
            get;
            set;
        }

        protected void Generate()
        {
            ID = ParentContext.EngineObjects.RegisterObject(GL.GenFramebuffer());
        }

        protected void Bind()
        {
            BindingManager.BindFramebuffer(ParentContext.EngineObjects[ID, this.GetType()]);
        }

        protected void Unbind()
        {
            BindingManager.UnbindFramebuffer();
        }

        protected void DrawBuffers(Kokoro2.Engine.FrameBufferAttachments[] attachments)
        {
            //TODO Setup DrawBuffer attachments
            DrawBuffersEnum[] dbEnum = new DrawBuffersEnum[attachments.Length];
            for (int i = 0; i < dbEnum.Length; i++) { dbEnum[i] = EnumConverters.EDrawBufferAttachment(attachments[i]); }
            dbEnum = dbEnum.OrderBy(x => x).ToArray();

            if (dbEnum.Length > 0)
            {
                List<DrawBuffersEnum> t = new List<DrawBuffersEnum>();
                for (int i = (int)dbEnum[0]; i <= (int)dbEnum[dbEnum.Length - 1]; i++)
                {
                    if (dbEnum.Contains((DrawBuffersEnum)i)) t.Add((DrawBuffersEnum)i);
                    else t.Add(0);
                }

                GL.DrawBuffers(t.Count(), t.ToArray());
            }
        }

        protected void BlendFunction(Engine.BlendFunc func, int index)
        {
            GL.BlendFunc(index, EnumConverters.EBlendFuncSRC(func.Src), EnumConverters.EBlendFuncDST(func.Dst));
        }

        protected void Delete()
        {
            GL.DeleteFramebuffer(ParentContext.EngineObjects[ID, this.GetType()]);
            ParentContext.EngineObjects.UnregisterObject(ID);
            ID = 0;
        }

        protected void BindTexture(Engine.FrameBufferAttachments texUnit, Engine.Texture t)
        {
            FramebufferAttachment attach = EnumConverters.EFrameBufferAttachment(texUnit);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attach, TextureTarget.Texture2D, ParentContext.EngineObjects[t.ID, t.GetType()], 0);
        }

        protected void CheckError()
        {
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete) throw new Exception(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer).ToString());
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                Delete();
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        ~FrameBufferLL()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}

#endif