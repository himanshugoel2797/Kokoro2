using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Debug;
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
        protected int id;
        private string file;
        bool loaded = false;

        private static Dictionary<string, int> loadedImages = new Dictionary<string, int>();

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

        public Texture(int width, int height, PixelFormat pf, PixelComponentType pct, PixelType pixelType)
        {
            lock (locker)
            {
                id = base.Create(width, height, pct, pf, pixelType);
                ObjectAllocTracker.NewCreated(this, id, " { " + pf.ToString() + ", " + pct.ToString() + ", " + pixelType.ToString() + "}");
                loaded = true;  //There's nothing to load
            }
        }
        public Texture(string filename, bool delayedLoad = false)
        {
            //TODO make this write to a texture array where the ID returned is the layer, additionally maintain a GPU buffer with the normalized sizes and offsets of the textures in their respective layers
            lock (locker)
            {
                if (loadedImages.ContainsKey(filename))
                {
                    this.id = loadedImages[filename];
                    loaded = true;
                }
                else
                {
                    //If requested, don't load the texture yet
                    if (!delayedLoad)
                    {
                        id = base.Create(filename);
                        loadedImages[filename] = id;
                        loaded = true;
                    }
                    else
                    {
                        this.file = filename;
                        loaded = false;
                    }
                    ObjectAllocTracker.NewCreated(this, id, " " + filename);
                }
            }
        }
        public Texture(int id)
        {
            lock (locker)
            {
                loaded = true;
                this.id = id;
                ObjectAllocTracker.NewCreated(this, id, " Duplicate");
            }
        }
        public Texture(Image img)
        {
            lock (locker)
            {
                loaded = true;
                this.id = base.Create(img);
                ObjectAllocTracker.NewCreated(this, id, "IMAGE" + id);
            }
        }
#if DEBUG
        ~Texture()
        {
            ObjectAllocTracker.ObjectDestroyed(this, id);
        }
#endif

        public Bitmap ToBMP()
        {
            return base.FetchTextureData(id);
        }

        public virtual void Bind(int texUnit)
        {
            //load the texture if it wasn't loaded before
            if (!loaded)
            {
                lock (locker)
                {
                    id = base.Create(file);
                    loadedImages[file] = id;
                    loaded = true;
                }
            }
            base.BindTexture(texUnit, id);
        }

        public static void UnBind(int texUnit)
        {
            UnBindTexture(texUnit);
        }

        public void Dispose()
        {
            base.Delete(id);
        }

    }
}
