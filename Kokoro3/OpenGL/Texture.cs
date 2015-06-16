#if OPENGL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Kokoro3.OpenGL
{
    public class Texture : IDisposable
    {
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
        
        protected Texture()
        {
            ID = GL.GenTexture();
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