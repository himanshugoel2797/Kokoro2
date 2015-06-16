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
    public class TextureStorage : Texture
    {
        //TODO Allocate a texture array of maximum layer count, at 512px resolution per tile, storing a max of 512 tiles at a time, with a post processing step to prepare a 3d model to handle
        // the tile mapping, additionally, texture storage is technically unlimited through the use of memory mapped files

        public Vector3 Size;

        public TextureStorage(int TextureSide, int layers, int levels, Engine.PixelComponentType pct)
        {
            this.pct = pct;

            int prevID = LLDevice.BindTex(TextureTarget.Texture2DArray, ID);
            GL.TexStorage3D(TextureTarget3d.Texture2DArray, levels, EnumConverters.ESizedInternalFormat(pct), TextureSide, TextureSide, layers);

            Size = new Vector3(TextureSide, TextureSide, layers);

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

        public TextureView CreateView(int layer, int minLevel, int levelCount)
        {
            return new TextureView(this, pct, minLevel, levelCount, layer);
        }

        ~TextureStorage()
        {
            System.Diagnostics.Debug.WriteLine($"[WARN] TextureStorage {ID} was automatically disposed");
            Dispose();
        }
    }
}
#endif