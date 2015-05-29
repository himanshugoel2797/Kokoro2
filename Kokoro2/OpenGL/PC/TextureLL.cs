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
    public class TextureLL
    {
        protected int width;
        protected int height;

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

        protected int Create(int width, int height, Kokoro2.Engine.PixelComponentType pfI, Kokoro2.Engine.PixelFormat pf, Kokoro2.Engine.PixelType type, bool multisample = false, int sampleCount = 1)
        {
            this.width = width;
            this.height = height;

            int id = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, id);

            if (!multisample)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, EnumConverters.EPixelComponentType(pfI), width, height, 0,
                    EnumConverters.EPixelFormat(pf), EnumConverters.EPixelType(type), (IntPtr)0);
            }
            else
            {
                GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, sampleCount, EnumConverters.EPixelComponentType(pfI), width, height, false);
            }

            // We haven't uploaded mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // On newer video cards, we can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            return id;
        }

        protected int Create(Image img)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap(img);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            this.width = bmp.Width;
            this.height = bmp.Height;

            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);
            bmp.Dispose();

            // We haven't uploaded mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // On newer video cards, we can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,  (int)TextureWrapMode.Repeat);
            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            return id;
        }

        protected int Create(string filename)
        {

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap(filename);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            this.width = bmp.Width;
            this.height = bmp.Height;

            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

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
            return id;
        }

        protected void BindTexture(int texUnit, int id)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texUnit);
            GL.BindTexture(TextureTarget.Texture2D, id);
        }

        protected static void UnBindTexture(int texUnit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texUnit);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        protected void Delete(int id)
        {
            GL.DeleteTexture(id);
        }

        protected void BindToFBuffer(Engine.FrameBufferAttachments texUnit, int id)
        {
            FramebufferAttachment attach = EnumConverters.EFrameBufferAttachment(texUnit);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attach, TextureTarget.Texture2D, id, 0);
        }

        protected Bitmap FetchTextureData(int id)
        {
            GL.BindTexture(TextureTarget.Texture2D, id);
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
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)(mode ? TextureWrapMode.Repeat : TextureWrapMode.ClampToBorder));

        }

        protected void SetWrapY(bool mode)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)(mode ? TextureWrapMode.Repeat : TextureWrapMode.ClampToBorder));
        }
    }
}

#endif