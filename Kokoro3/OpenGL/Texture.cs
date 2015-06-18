#if OPENGL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Kokoro3.Math;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Kokoro3.OpenGL
{
    public class Texture : IDisposable
    {
        #region Texture Factory
        public static Texture Load2D(int width, int height, int levels, Engine.PixelComponentType pct, Engine.PixelFormat pf, Engine.PixelType pt, PixelBuffer pbo = null)
        {
            Texture t = new Texture(GL.GenTexture(), Engine.TextureType.Texture2D, pct);
            t.Size = new Vector2(width, height);

            int prevID = LLDevice.BindTex(TextureTarget.Texture2D, t.ID);
            GL.TexStorage2D(TextureTarget2d.Texture2D, levels, EnumConverters.ESizedInternalFormat(pct), width, height);

            if (pbo != null)
            {
                int prevPBO = LLDevice.BindBuffer(BufferTarget.PixelUnpackBuffer, pbo.memory.ID);
                GL.TexImage2D(TextureTarget.Texture2D, 0, EnumConverters.EPixelComponentType(pct), width, height, 0, EnumConverters.EPixelFormat(pf), EnumConverters.EPixelType(pt), IntPtr.Zero);
                LLDevice.BindBuffer(BufferTarget.PixelUnpackBuffer, prevPBO);
            }

            t.WrapMode = Engine.TextureWrapMode.Repeat;
            t.FilterMode = Engine.TextureFilter.Linear;

            LLDevice.BindTex(TextureTarget.Texture2D, prevID);
            return t;
        }
        public static Texture Load2D(int levels, Engine.PixelComponentType pct, Engine.PixelFormat pf, Engine.PixelType pt, Image img)
        {
            Texture t = new Texture(GL.GenTexture(), Engine.TextureType.Texture2D, pct);
            t.Size = new Vector2(img.Width, img.Height);

            int prevID = LLDevice.BindTex(TextureTarget.Texture2D, t.ID);
            Bitmap bmp = new Bitmap(img);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);
            bmp.Dispose();

            t.WrapMode = Engine.TextureWrapMode.Repeat;
            t.FilterMode = Engine.TextureFilter.Linear;

            LLDevice.BindTex(TextureTarget.Texture2D, prevID);
            return t;
        }

        public static Texture Load2D(string filename)
        {
            Texture t = new Texture(GL.GenTexture(), Engine.TextureType.Texture2D, 0);
            int prevID = LLDevice.BindTex(TextureTarget.Texture2D, t.ID);

            //Load a DDS file
            if (Path.GetExtension(filename).ToLower() == ".dds")
            {
                TextureTarget tmp;
                int width, height;
                PixelInternalFormat pct;
                ImageDDS.LoadFromDisk(filename, out tmp, out width, out height, out pct);
                t.Size = new Vector2(width, height);
                t.pct = (Engine.PixelComponentType)pct;
            }
            else
            {
                //Load all other kinds of files
                Image img = Image.FromFile(filename);
                t.Size = new Vector2(img.Width, img.Height);

                Bitmap bmp = new Bitmap(img);
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                    OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

                bmp.UnlockBits(bmp_data);
                bmp.Dispose();
            }
            t.WrapMode = Engine.TextureWrapMode.Repeat;
            t.FilterMode = Engine.TextureFilter.Linear;

            LLDevice.BindTex(TextureTarget.Texture2D, prevID);
            return t;
        }
        #endregion

        internal int ID;
        public Engine.TextureType TexType;
        public Vector2 Size;

        #region Wrap and filter mode
        private Engine.TextureFilter filter;
        private Engine.TextureWrapMode wrapMode;


        public Engine.TextureFilter FilterMode
        {
            get
            {
                return filter;
            }
            set
            {
                SetFilterMode(value);
            }
        }
        public Engine.TextureWrapMode WrapMode
        {
            get
            {
                return wrapMode;
            }
            set
            {
                SetWrapMode(value);
            }
        }

        protected void SetFilterMode(Engine.TextureFilter filter)
        {
            this.filter = filter;
            int prevID = LLDevice.BindTex(EnumConverters.ETextureTarget(TexType), ID);

            GL.TexParameter(EnumConverters.ETextureTarget(TexType), TextureParameterName.TextureMinFilter, EnumConverters.ETextureFilter(filter));
            GL.TexParameter(EnumConverters.ETextureTarget(TexType), TextureParameterName.TextureMagFilter, EnumConverters.ETextureFilter(filter));

            LLDevice.BindTex(EnumConverters.ETextureTarget(TexType), prevID);
        }

        protected void SetWrapMode(Engine.TextureWrapMode wrap)
        {
            this.wrapMode = wrap;
            int prevID = LLDevice.BindTex(EnumConverters.ETextureTarget(TexType), ID);
            GL.TexParameter(EnumConverters.ETextureTarget(TexType), TextureParameterName.TextureWrapS, EnumConverters.ETextureWrapMode(wrap));
            GL.TexParameter(EnumConverters.ETextureTarget(TexType), TextureParameterName.TextureWrapT, EnumConverters.ETextureWrapMode(wrap));
            LLDevice.BindTex(EnumConverters.ETextureTarget(TexType), prevID);
        }
        #endregion

        internal Engine.PixelComponentType pct;

        protected Texture()
        {
            ID = GL.GenTexture();
        }

        internal Texture(int id, Engine.TextureType texType, Engine.PixelComponentType pct)
        {
            ID = id;
            TexType = texType;
            this.pct = pct;
        }

        public void GenerateMipMaps()
        {
            int prevID = LLDevice.BindTex(EnumConverters.ETextureTarget(TexType), ID);
            GL.GenerateMipmap((GenerateMipmapTarget)EnumConverters.ETextureTarget(TexType));
            LLDevice.BindTex(EnumConverters.ETextureTarget(TexType), prevID);
        }

        public void Dispose()
        {
            GL.DeleteTexture(ID);
            ID = 0;
            GC.SuppressFinalize(this);
        }

        ~Texture()
        {
            System.Diagnostics.Debug.WriteLine($"[WARN] Texture {ID} was disposed automatically");
            Dispose();
        }
    }
}
#endif