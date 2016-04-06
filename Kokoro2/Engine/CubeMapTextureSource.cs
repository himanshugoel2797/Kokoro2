using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public class CubeMapTextureSource : ITextureSource
    {
        public static Texture LoadCubemap(string px, string py, string pz, string nx, string ny, string nz, int mipmapLevs, bool srgba, GraphicsContext c)
        {
            Texture t = new Texture(c);

            ImageTextureSource b0 = new ImageTextureSource(px, mipmapLevs, srgba),
                                b1 = new ImageTextureSource(py, mipmapLevs, srgba),
                                b2 = new ImageTextureSource(pz, mipmapLevs, srgba),
                                b3 = new ImageTextureSource(nx, mipmapLevs, srgba),
                                b4 = new ImageTextureSource(ny, mipmapLevs, srgba),
                                b5 = new ImageTextureSource(nz, mipmapLevs, srgba);

            CubeMapTextureSource c0 = new CubeMapTextureSource(CubeMapFace.PositiveX, b0),
                                 c1 = new CubeMapTextureSource(CubeMapFace.PositiveY, b1),
                                 c2 = new CubeMapTextureSource(CubeMapFace.PositiveZ, b2),
                                 c3 = new CubeMapTextureSource(CubeMapFace.NegativeX, b3),
                                 c4 = new CubeMapTextureSource(CubeMapFace.NegativeY, b4),
                                 c5 = new CubeMapTextureSource(CubeMapFace.NegativeZ, b5);

            t.SetData(c0);
            t.SetData(c1);
            t.SetData(c2);
            t.SetData(c3);
            t.SetData(c4);
            t.SetData(c5);

            return t;
        }

        public enum CubeMapFace
        {
            PositiveX, PositiveY, PositiveZ,
            NegativeX, NegativeY, NegativeZ
        };

        private ITextureSource texSrc;
        private CubeMapFace curFace;

        public CubeMapTextureSource(CubeMapFace face, ITextureSource tex)
        {
            curFace = face;
            texSrc = tex;
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
            return texSrc.GetFormat();
        }

        public int GetHeight()
        {
            return texSrc.GetHeight();
        }

        public PixelComponentType GetInternalFormat()
        {
            return texSrc.GetInternalFormat();
        }

        public int GetLevels()
        {
            return texSrc.GetLevels();
        }

        public IntPtr GetPixelData()
        {
            return texSrc.GetPixelData();
        }


        private int targetCallNum = 0;
        public TextureTarget GetTextureTarget()
        {
            if (targetCallNum == 5) targetCallNum = 0;
            if (targetCallNum++ != 1) return TextureTarget.TextureCubeMap;
            switch (curFace)
            {
                case CubeMapFace.NegativeX:
                    return TextureTarget.TextureCubeMapNegativeX;
                case CubeMapFace.NegativeY:
                    return TextureTarget.TextureCubeMapNegativeY;
                case CubeMapFace.NegativeZ:
                    return TextureTarget.TextureCubeMapNegativeZ;
                case CubeMapFace.PositiveX:
                    return TextureTarget.TextureCubeMapPositiveX;
                case CubeMapFace.PositiveY:
                    return TextureTarget.TextureCubeMapPositiveY;
                case CubeMapFace.PositiveZ:
                    return TextureTarget.TextureCubeMapPositiveZ;
            }
            return TextureTarget.TextureCubeMap;
        }

        public int GetWidth()
        {
            return texSrc.GetWidth();
        }

        PixelType ITextureSource.GetType()
        {
            return texSrc.GetType();
        }
    }

}
