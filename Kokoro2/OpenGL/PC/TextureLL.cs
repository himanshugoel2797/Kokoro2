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
        internal TextureTarget texTarget;
        internal OpenTK.Graphics.OpenGL4.PixelFormat format;

        public int Width { get; internal set; }
        public int Height { get; internal set; }
        public int Depth { get; internal set; }
        public int LevelCount { get; internal set; }


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
            BindTexture(0);
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
            UnBindTexture(0);
        }

        protected void SetData(Engine.ITextureSource src)
        {
            ID = ParentContext.EngineObjects.RegisterObject(GL.GenTexture());

            BindingManager.BindTexture(0, EnumConverters.ETextureTarget(src.GetTextureTarget()), ParentContext.EngineObjects[ID, this.GetType()]);
            switch (src.GetDimensions())
            {
                case 1:
                    GL.TexImage1D(EnumConverters.ETextureTarget(src.GetTextureTarget()), src.GetLevels(), EnumConverters.EPixelComponentType(src.GetInternalFormat()), src.GetWidth(), 0, EnumConverters.EPixelFormat(src.GetFormat()), EnumConverters.EPixelType(src.GetType()), src.GetPixelData());
                    break;
                case 2:
                    GL.TexImage2D(EnumConverters.ETextureTarget(src.GetTextureTarget()), src.GetLevels(), EnumConverters.EPixelComponentType(src.GetInternalFormat()), src.GetWidth(), src.GetHeight(), 0, EnumConverters.EPixelFormat(src.GetFormat()), EnumConverters.EPixelType(src.GetType()), src.GetPixelData());
                    break;
                case 3:
                    GL.TexImage3D(EnumConverters.ETextureTarget(src.GetTextureTarget()), src.GetLevels(), EnumConverters.EPixelComponentType(src.GetInternalFormat()), src.GetWidth(), src.GetHeight(), src.GetDepth(), 0, EnumConverters.EPixelFormat(src.GetFormat()), EnumConverters.EPixelType(src.GetType()), src.GetPixelData());
                    break;
            }

            //GL.GenerateMipmap((GenerateMipmapTarget)EnumConverters.ETextureTarget(src.GetTextureTarget()));
            BindingManager.UnbindTexture(0, EnumConverters.ETextureTarget(src.GetTextureTarget()));

            this.Width = src.GetWidth();
            this.Height = src.GetHeight();
            this.Depth = src.GetDepth();
            this.LevelCount = src.GetLevels();

            this.format = EnumConverters.EPixelFormat(src.GetFormat());
            this.internalformat = EnumConverters.EPixelComponentType(src.GetInternalFormat());
            this.texTarget = EnumConverters.ETextureTarget(src.GetTextureTarget());
        }

        protected void BindTexture(int texUnit)
        {
            BindingManager.BindTexture(texUnit, TextureTarget.Texture2D, ParentContext.EngineObjects[ID, this.GetType()]);
        }

        protected static void UnBindTexture(int texUnit)
        {
            BindingManager.UnbindTexture(texUnit, TextureTarget.Texture2D);
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
            BindTexture(0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int)(a ? TextureCompareMode.CompareRefToTexture : TextureCompareMode.None));
            UnBindTexture(0);
        }

        protected Bitmap FetchTextureData()
        {
            BindTexture(0);
            int width, height;
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out width);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out height);
            Bitmap bmp = new Bitmap(width, height);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.GetTexImage(TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            UnBindTexture(0);
            return bmp;
        }

        protected static void UnBindFromFBuffer(int texUnit)
        {
            FramebufferAttachment attach = (FramebufferAttachment)texUnit;
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, attach, 0, 0);
        }

        protected void SetWrapX(bool mode)
        {
            BindTexture(0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)(mode ? TextureWrapMode.Repeat : TextureWrapMode.ClampToEdge));
            UnBindTexture(0);
        }

        protected void SetWrapY(bool mode)
        {
            BindTexture(0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)(mode ? TextureWrapMode.Repeat : TextureWrapMode.ClampToEdge));
            UnBindTexture(0);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        private PixelInternalFormat internalformat;

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