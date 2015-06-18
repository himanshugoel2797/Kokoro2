#if OPENGL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Kokoro3.Math;

namespace Kokoro3.OpenGL
{
    public class TextureStorage : IDisposable
    {
        //TODO Allocate a texture array of maximum layer count, at 512px resolution per tile, storing a max of 512 tiles at a time, with a post processing step to prepare a 3d model to handle
        // the tile mapping, additionally, texture storage is technically unlimited through the use of memory mapped files

        public Vector3 Size;

        internal int ID;
        public Engine.TextureType TexType;

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

        protected Engine.PixelComponentType pct;

        public TextureStorage(int TextureSide, int layers, int levels, Engine.PixelComponentType pct)
        {
            this.pct = pct;

            int prevID = LLDevice.BindTex(TextureTarget.Texture2DArray, ID);
            GL.TexStorage3D(TextureTarget3d.Texture2DArray, levels, EnumConverters.ESizedInternalFormat(pct), TextureSide, TextureSide, layers);

            Size = new Vector3(TextureSide, TextureSide, layers);
            TexType = Engine.TextureType.Texture2DArray;
            LLDevice.BindTex(TextureTarget.Texture2DArray, prevID);
        }

        public void SetData(PixelBuffer pbo, int layer, int mipmapLevel, Vector4 Rect, Engine.PixelFormat pf, Engine.PixelType pt)
        {
            int prevID = LLDevice.BindBuffer(BufferTarget.PixelUnpackBuffer, pbo.memory.ID);
            int preTex = LLDevice.BindTex(TextureTarget.Texture2DArray, ID);

            GL.TexSubImage3D(TextureTarget.Texture2DArray, mipmapLevel, (int)Rect.X, (int)Rect.Y, layer, (int)Rect.Z, (int)Rect.W, 1, EnumConverters.EPixelFormat(pf), EnumConverters.EPixelType(pt), IntPtr.Zero);

            LLDevice.BindTex(TextureTarget.Texture2DArray, preTex);
            LLDevice.BindBuffer(BufferTarget.PixelUnpackBuffer, prevID);
        }

        public Texture CreateView(int layer, int minLevel, int levelCount)
        {
            Texture t = new Texture(GL.GenTexture(), Engine.TextureType.Texture2D, pct);
            GL.TextureView(t.ID, TextureTarget.Texture2D, ID, EnumConverters.EPixelComponentType(pct), minLevel, levelCount, layer, 1);

            return t;
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

        ~TextureStorage()
        {
            System.Diagnostics.Debug.WriteLine($"[WARN] TextureStorage {ID} was automatically disposed");
            Dispose();
        }
    }
}
#endif