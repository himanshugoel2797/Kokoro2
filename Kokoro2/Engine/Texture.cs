using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro2.Math;

#if PC
using System.Drawing;
#endif

#if OPENGL
#if PC
using Kokoro2.OpenGL.PC;
#endif

#elif OPENGL_AZDO
#if PC
using Kokoro2.OpenGL.AZDO;
#endif

#endif

namespace Kokoro2.Engine
{
    public enum TextureFilter
    {
        Linear, Nearest
    }

    /// <summary>
    /// Texture
    /// </summary>
    public class Texture : TextureLL, IDisposable
    {

        private static Dictionary<string, ulong> loadedImages = new Dictionary<string, ulong>();
        private static Dictionary<string, int> refCounts = new Dictionary<string, int>();
        bool loaded = false;

        public TextureFilter FilterMode
        {
            set
            {
                if (!loaded) throw new InvalidOperationException();
                SetFilterMode(value);
            }
        }

        public bool Compare
        {
            set
            {
                if (!loaded) throw new InvalidOperationException();
                SetCompare(value);
            }
        }

        public bool WrapX
        {
            set
            {
                if (!loaded) throw new InvalidOperationException();
                SetWrapX(value);
            }
        }

        public bool WrapY
        {
            set
            {
                if (!loaded) throw new InvalidOperationException();
                SetWrapY(value);
            }
        }


        public Texture(GraphicsContext c)
        {
            ParentContext = c;
        }

        public new void UpdateMipMaps()
        {
            base.UpdateMipMaps();
        }

        public new void SetData(ITextureSource src)
        {
            loaded = true;
            ParentContext.Disposing += Dispose;
            base.SetData(src);
            FilterMode = TextureFilter.Linear;
            Compare = false;
            WrapX = true;
            WrapY = true;
        }

#if DEBUG
        ~Texture()
        {
            ObjectAllocTracker.ObjectDestroyed(this);
        }
#endif

        public Bitmap ToBMP()
        {
            if (!loaded) throw new InvalidOperationException();
            return base.FetchTextureData();
        }

        public virtual void Bind(int texUnit)
        {
            //load the texture if it wasn't loaded before
            if (!loaded) throw new InvalidOperationException();
            base.BindTexture(texUnit);
        }

        public void UnBind(int texUnit)
        {
            if (!loaded) throw new InvalidOperationException();
            UnBindTexture(texUnit);
        }

        public new void Dispose()
        {
            if (loaded)
            {
                base.Dispose();
            }
        }

    }
}
