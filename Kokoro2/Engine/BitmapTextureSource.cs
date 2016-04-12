using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public class ImageTextureSource : ITextureSource, IDisposable
    {
        public static Texture Create(string file, int levels, bool srgba, GraphicsContext c)
        {
            while (true)
            {
                try {
                    var tmp = Image.FromFile(file);
                    var toRet = Create(tmp, levels, srgba, c);
                    tmp.Dispose();
                    return toRet;
                }catch(Exception err)
                {
                }
            }
        }

        public static Texture Create(Image bmp, int levels, bool srgba, GraphicsContext c)
        {
            ImageTextureSource src = new ImageTextureSource(bmp, levels, srgba);
            Texture t = new Texture(c);
            t.SetData(src);
            src.Dispose();
            return t;
        }

        Bitmap srcBmp;
        BitmapData bmpData;

        public int Width { get; internal set; }
        public int Height { get; internal set; }
        public int Levels { get; internal set; }
        public bool sRGBASupported { get; private set; }

        public ImageTextureSource(Image bmp, int mipmapLevels, bool SRGBA)
        {
            srcBmp = (Bitmap)bmp.Clone();
            srcBmp.RotateFlip(RotateFlipType.Rotate180FlipX);
            Width = bmp.Width;
            Height = bmp.Height;
            Levels = mipmapLevels;
            sRGBASupported = SRGBA;

            bmpData = srcBmp.LockBits(new Rectangle(0, 0, Width, Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        public ImageTextureSource(string path, int mipmapLevels, bool srgba) : this(new Bitmap(path), mipmapLevels, srgba) { }


        public int GetDepth()
        {
            return 0;
        }

        public int GetDimensions()
        {
            return 2;
        }

        public PixelFormat GetFormat()
        {
            return PixelFormat.BGRA;
        }

        public int GetHeight()
        {
            return Height;
        }

        public PixelComponentType GetInternalFormat()
        {
            if (!sRGBASupported) return PixelComponentType.RGBA8;
            else return PixelComponentType.SRGBA8;
        }

        public int GetLevels()
        {
            return Levels;
        }

        public IntPtr GetPixelData()
        {
            return bmpData.Scan0;
        }

        public TextureTarget GetTextureTarget()
        {
            return TextureTarget.Texture2D;
        }

        public int GetWidth()
        {
            return Width;
        }

        PixelType ITextureSource.GetType()
        {
            return PixelType.UnsignedByte;
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
                    srcBmp.UnlockBits(bmpData);
                    srcBmp.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BitmapTextureSource() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

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
