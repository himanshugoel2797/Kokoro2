#if OPENGL && PC

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Kokoro2.OpenGL.PC
{
    public class FrameBufferLL
    {
        protected int Generate()
        {
            return GL.GenFramebuffer();
        }

        protected void Bind(int id)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
            GL.Enable(EnableCap.Blend);
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

        protected void Delete(int id)
        {
            GL.DeleteFramebuffer(id);
        }

        protected void CheckError()
        {
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete) throw new Exception(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer).ToString());
        }

    }
}

#endif