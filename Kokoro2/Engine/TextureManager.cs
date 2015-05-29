using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro.Math;
using Kokoro.Debug;

#if PC
using System.Drawing;
#endif

#if OPENGL
#if PC
using Kokoro.OpenGL.PC;
#endif

#elif OPENGL_AZDO
#if PC
using Kokoro.OpenGL.AZDO;
#endif

#endif

namespace Kokoro.Engine
{
    public class TextureManager : TextureLL, IDisposable
    {
        private int TexID;
        private List<Texture> textures = new List<Texture>();

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

        public TextureManager()
        {
            TexID = base.Create3D(2048, 2048, 2048);
        }


        public Texture Load(string filename, bool delayedLoad = false)
        {
            Texture t = new Texture();
            textures.Add(t);
            Sinus.SinusManager.QueueCommand(() =>
            {
                Load3D(TexID, filename, textures.Count);
                t.id = textures.Count;
                t.file = filename;
                t.width = width;
                t.height = height;

                ObjectAllocTracker.NewCreated(this, t.id, " " + filename);
            });

            return t;
        }

#if DEBUG
        ~TextureManager()
        {
            ObjectAllocTracker.ObjectDestroyed(this, TexID);
        }
#endif

        public Bitmap ToBMP(int layer)
        {
            return base.FetchTextureData2D(TexID, layer);
        }

        public virtual void Bind()
        {
            //TODO load the texture if it wasn't loaded before

            base.BindTexture2DArray(TexID);
        }

        public static void UnBind()
        {
            UnBindTexture2DArray();
        }

        public void Dispose()
        {
            base.Delete(TexID);
        }

    }
}
