using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public class DepthTextureSource : ITextureSource
    {
        public static Texture Create(int width, int height, PixelComponentType format, GraphicsContext c)
        {
            DepthTextureSource src = new DepthTextureSource(width, height);
            src.InternalFormat = format;

            Texture t = new Texture(c);
            t.SetData(src);

            return t;
        }

        public int Height { get; set; }
        public int Width { get; set; }
        public PixelComponentType InternalFormat { get; set; }

        public DepthTextureSource(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

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
            return PixelFormat.Depth;
        }

        public int GetHeight()
        {
            return Height;
        }

        public PixelComponentType GetInternalFormat()
        {
            return InternalFormat;
        }

        public int GetLevels()
        {
            return 0;
        }

        public IntPtr GetPixelData()
        {
            return IntPtr.Zero;
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
            return PixelType.Float;
        }
    }

}
