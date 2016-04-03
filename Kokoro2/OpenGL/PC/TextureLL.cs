#if OPENGL && PC

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kokoro2.OpenGL.PC
{
    public class TextureLL : Engine.IEngineObject
    {
        protected int width;
        protected int height;

        public ulong ID
        {
            get; set;
        }

        public Engine.GraphicsContext ParentContext
        {
            get; set;
        }

        protected void SetFilterMode(Engine.TextureFilter filter)
        {
            if (filter == Engine.TextureFilter.Linear)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }
            else
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            }
        }

        protected void Create(int src, int layer, Engine.PixelComponentType pfI)
        {
            int id = GL.GenTexture();
            GL.TextureView(id, TextureTarget.Texture2D, src, EnumConverters.EPixelComponentType(pfI), 0, 0, layer, 1);
            ID = ParentContext.EngineObjects.RegisterObject(id);
        }

        protected void Create(int width, int height, Kokoro2.Engine.PixelComponentType pfI, Kokoro2.Engine.PixelFormat pf, Kokoro2.Engine.PixelType type, bool multisample = false, int sampleCount = 1)
        {
            this.width = width;
            this.height = height;

            int id = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, id);

            if (!multisample)
            {
                //if (pfI != Engine.PixelComponentType.D32S8 && pfI != Engine.PixelComponentType.D32 && pfI != Engine.PixelComponentType.D24S8)
                //{
                //    GL.TexStorage2D(TextureTarget2d.Texture2D, 1, (SizedInternalFormat)EnumConverters.EPixelComponentType(pfI), width, height);
                //    GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, EnumConverters.EPixelFormat(pf), EnumConverters.EPixelType(type), (IntPtr)0);
                //}else
                {
                    GL.TexImage2D(TextureTarget.Texture2D, 0, EnumConverters.EPixelComponentType(pfI), width, height, 0, EnumConverters.EPixelFormat(pf), EnumConverters.EPixelType(type), (IntPtr)0);
                }
            }
            else
            {
                GL.TexStorage2DMultisample(TextureTargetMultisample2d.Texture2DMultisample, sampleCount, (SizedInternalFormat)EnumConverters.EPixelComponentType(pfI), width, height, false);
            }

            // We haven't uploaded mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // On newer video cards, we can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            ID = ParentContext.EngineObjects.RegisterObject(id);
        }

        protected void Create(Image img, bool srgb)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap(img);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            this.width = bmp.Width;
            this.height = bmp.Height;

            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            /*GL.TexStorage2D(TextureTarget2d.Texture2D, 1, SizedInternalFormat.Rgba8, bmp_data.Width, bmp_data.Height);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, bmp_data.Width, bmp_data.Height, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            */
            GL.TexImage2D(TextureTarget.Texture2D, 0, srgb ? PixelInternalFormat.Srgb8Alpha8 : PixelInternalFormat.Rgba8, width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);
            bmp.Dispose();

            // We haven't uploaded mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // On newer video cards, we can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            ID = ParentContext.EngineObjects.RegisterObject(id);
        }

        protected void Create(string filename, bool srgb)
        {
            Bitmap bmp = new Bitmap(filename);
            Create(bmp, srgb);
            bmp.Dispose();
        }

        protected void BindTexture(int texUnit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texUnit);
            GL.BindTexture(TextureTarget.Texture2D, ParentContext.EngineObjects[ID, this.GetType()]);
        }

        protected static void UnBindTexture(int texUnit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texUnit);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        protected void Delete()
        {
            if (ID != 0)
            {
                GL.DeleteTexture(ParentContext.EngineObjects[ID, this.GetType()]);
                ParentContext.EngineObjects.UnregisterObject(ID);
                ID = 0;
            }
        }

        protected void SetCompare(bool a)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int)(a ? TextureCompareMode.CompareRefToTexture : TextureCompareMode.None));
        }

        protected void BindToFBuffer(Engine.FrameBufferAttachments texUnit)
        {
            FramebufferAttachment attach = EnumConverters.EFrameBufferAttachment(texUnit);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attach, TextureTarget.Texture2D, ParentContext.EngineObjects[ID, this.GetType()], 0);
        }

        protected Bitmap FetchTextureData()
        {
            GL.BindTexture(TextureTarget.Texture2D, ParentContext.EngineObjects[ID, this.GetType()]);
            int width, height;
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out width);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out height);
            Bitmap bmp = new Bitmap(width, height);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.GetTexImage(TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            return bmp;
        }

        protected static void UnBindFromFBuffer(int texUnit)
        {
            FramebufferAttachment attach = (FramebufferAttachment)texUnit;
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, attach, 0, 0);
        }

        protected void SetWrapX(bool mode)
        {
            GL.BindTexture(TextureTarget.Texture2D, ParentContext.EngineObjects[ID, this.GetType()]);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)(mode ? TextureWrapMode.Repeat : TextureWrapMode.ClampToEdge));
        }

        protected void SetWrapY(bool mode)
        {
            GL.BindTexture(TextureTarget.Texture2D, ParentContext.EngineObjects[ID, this.GetType()]);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)(mode ? TextureWrapMode.Repeat : TextureWrapMode.ClampToEdge));
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
                    Delete();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        ~TextureLL()
        {
            //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
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