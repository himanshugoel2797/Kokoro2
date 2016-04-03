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
        private static readonly object locker = new object();
        private string file;
        bool srgba;

        private static Dictionary<string, ulong> loadedImages = new Dictionary<string, ulong>();
        private static Dictionary<string, int> refCounts = new Dictionary<string, int>();

        public Vector2 Size
        {
            get
            {
                return new Vector2(base.width, base.height);
            }
        }

        public TextureFilter FilterMode
        {
            set
            {
                SetFilterMode(value);
            }
        }

        public bool Compare
        {
            set
            {
                SetCompare(value);
            }
        }

        public bool WrapX
        {
            set
            {
                SetWrapX(value);
            }
        }

        public bool WrapY
        {
            set
            {
                SetWrapY(value);
            }
        }

        public Texture(int width, int height, PixelFormat pf, PixelComponentType pct, PixelType pixelType, GraphicsContext c)
        {
            ParentContext = c;
            c.Disposing += Dispose;
            lock (locker)
            {
                base.Create(width, height, pct, pf, pixelType);
                ObjectAllocTracker.NewCreated(this, " { " + pf.ToString() + ", " + pct.ToString() + ", " + pixelType.ToString() + "}");
            }
        }
        public Texture(string filename, bool srgba, GraphicsContext c)
        {
            ParentContext = c;
            c.Disposing += Dispose;
            file = filename;
            //TODO make this write to a texture array where the ID returned is the layer, additionally maintain a GPU buffer with the normalized sizes and offsets of the textures in their respective layers
            lock (locker)
            {
                if (loadedImages.ContainsKey(filename))
                {
                    ID = loadedImages[filename];
                    refCounts[filename]++;
                }
                else
                {
                    base.Create(filename, srgba);
                    loadedImages[filename] = ID;
                    refCounts[filename] = 1;
                    ObjectAllocTracker.NewCreated(this, filename);
                }
            }
        }

        public Texture(ulong id, GraphicsContext c)
        {
            ParentContext = c;
            c.Disposing += Dispose;

            lock (locker)
            {
                this.ID = id;
                ObjectAllocTracker.NewCreated(this, " Duplicate");
            }
        }
        public Texture(Image img, bool srgba, GraphicsContext c)
        {
            ParentContext = c;
            c.Disposing += Dispose;

            lock (locker)
            {
                base.Create(img, srgba);
                ObjectAllocTracker.NewCreated(this, "IMAGE" + ID);
            }
        }
#if DEBUG
        ~Texture()
        {
            ObjectAllocTracker.ObjectDestroyed(this);
        }
#endif

        public Bitmap ToBMP()
        {
            return base.FetchTextureData();
        }

        public virtual void Bind(int texUnit)
        {
            //load the texture if it wasn't loaded before
            base.BindTexture(texUnit);
        }

        public static void UnBind(int texUnit)
        {
            UnBindTexture(texUnit);
        }

        public new void Dispose()
        {
            if (!string.IsNullOrWhiteSpace(file))
            {
                if (refCounts[file] == 1) base.Dispose();
                else refCounts[file]--;
            }
            else base.Dispose();
        }

    }
}
