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
        private bool isBufferTex = false, mipmapsPopulated = false;

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
                GL.TexParameter(texTarget, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                if (LevelCount != 0 && mipmapsPopulated) GL.TexParameter(texTarget, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                else GL.TexParameter(texTarget, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            }
            else
            {
                GL.TexParameter(texTarget, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                if (LevelCount != 0 && mipmapsPopulated) GL.TexParameter(texTarget, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapNearest);
                else GL.TexParameter(texTarget, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            }
            UnBindTexture(0);
        }

        protected void SetData(Engine.ITextureSource src)
        {
            ID = ParentContext.EngineObjects.RegisterObject(GL.GenTexture());

            var ptr = src.GetPixelData();
            int levels = src.GetLevels();
            int lvlCopy = levels;
            levels = (levels < 0) ? 0 : levels;

            BindingManager.BindTexture(0, EnumConverters.ETextureTarget(src.GetTextureTarget()), ParentContext.EngineObjects[ID, this.GetType()]);
            switch (src.GetDimensions())
            {
                case 1:
                    GL.TexImage1D(EnumConverters.ETextureTarget(src.GetTextureTarget()), levels, EnumConverters.EPixelComponentType(src.GetInternalFormat()), src.GetWidth(), 0, EnumConverters.EPixelFormat(src.GetFormat()), EnumConverters.EPixelType(src.GetType()), ptr);
                    break;
                case 2:
                    GL.TexImage2D(EnumConverters.ETextureTarget(src.GetTextureTarget()), levels, EnumConverters.EPixelComponentType(src.GetInternalFormat()), src.GetWidth(), src.GetHeight(), 0, EnumConverters.EPixelFormat(src.GetFormat()), EnumConverters.EPixelType(src.GetType()), ptr);
                    break;
                case 3:
                    GL.TexImage3D(EnumConverters.ETextureTarget(src.GetTextureTarget()), levels, EnumConverters.EPixelComponentType(src.GetInternalFormat()), src.GetWidth(), src.GetHeight(), src.GetDepth(), 0, EnumConverters.EPixelFormat(src.GetFormat()), EnumConverters.EPixelType(src.GetType()), ptr);
                    break;
                case 4:
                    GL.TexBuffer((TextureBufferTarget)EnumConverters.ETextureTarget(src.GetTextureTarget()), EnumConverters.ESizedInternalFormat(src.GetInternalFormat()), src.GetLevels());
                    isBufferTex = true;
                    break;
            }

            if (lvlCopy == -1) GL.GenerateMipmap((GenerateMipmapTarget)EnumConverters.ETextureTarget(src.GetTextureTarget()));
            BindingManager.UnbindTexture(0, EnumConverters.ETextureTarget(src.GetTextureTarget()));

            if (ptr == IntPtr.Zero) mipmapsPopulated = false;

            if (!isBufferTex)
            {
                this.Width = src.GetWidth();
                this.Height = src.GetHeight();
                this.Depth = src.GetDepth();
                this.LevelCount = lvlCopy;

                this.format = EnumConverters.EPixelFormat(src.GetFormat());
                this.internalformat = EnumConverters.EPixelComponentType(src.GetInternalFormat());
                this.texTarget = EnumConverters.ETextureTarget(src.GetTextureTarget());
            }
        }

        protected void UpdateMipMaps()
        {
            if (LevelCount != 0)
            {
                BindingManager.BindTexture(0, texTarget, ParentContext.EngineObjects[ID, this.GetType()]);
                GL.GenerateMipmap((GenerateMipmapTarget)texTarget);
                BindingManager.UnbindTexture(0, texTarget);

                mipmapsPopulated = true;
            }
        }

        protected void BindTexture(int texUnit)
        {
            BindingManager.BindTexture(texUnit, texTarget, ParentContext.EngineObjects[ID, this.GetType()]);
        }

        protected void UnBindTexture(int texUnit)
        {
            BindingManager.UnbindTexture(texUnit, texTarget);
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
            if (isBufferTex) return;
            BindTexture(0);
            GL.TexParameter(texTarget, TextureParameterName.TextureCompareMode, (int)(a ? TextureCompareMode.CompareRefToTexture : TextureCompareMode.None));
            UnBindTexture(0);
        }

        protected Bitmap FetchTextureData()
        {
            if (isBufferTex) return null;
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

        protected void SetWrapX(bool mode)
        {
            if (isBufferTex) return;
            BindTexture(0);
            GL.TexParameter(texTarget, TextureParameterName.TextureWrapS, (int)(mode ? TextureWrapMode.Repeat : TextureWrapMode.ClampToEdge));
            UnBindTexture(0);
        }

        protected void SetWrapY(bool mode)
        {
            if (isBufferTex) return;

            BindTexture(0);
            GL.TexParameter(texTarget, TextureParameterName.TextureWrapT, (int)(mode ? TextureWrapMode.Repeat : TextureWrapMode.ClampToEdge));
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