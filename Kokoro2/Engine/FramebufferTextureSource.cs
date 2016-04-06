using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public class FramebufferTextureSource : ITextureSource
    {
        private int width, height, levels;
        private PixelComponentType iF;
        private PixelType ty;

        public static Texture Create(int width, int height, int levels, PixelComponentType internalFormat, PixelType type, GraphicsContext c)
        {
            FramebufferTextureSource src = new FramebufferTextureSource(width, height, levels, internalFormat, type);
            Texture t = new Texture(c);
            t.SetData(src);
            return t;
        }

        public FramebufferTextureSource(int width, int height, int levels, PixelComponentType internalFormat, PixelType type)
        {
            this.width = width;
            this.height = height;
            this.levels = levels;
            ty = type;
            iF = internalFormat;
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
            return PixelFormat.BGRA;
        }

        public int GetHeight()
        {
            return height;
        }

        public PixelComponentType GetInternalFormat()
        {
            return iF;
        }

        public int GetLevels()
        {
            return levels;
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
            return width;
        }

        PixelType ITextureSource.GetType()
        {
            return ty;
        }
    }

}
