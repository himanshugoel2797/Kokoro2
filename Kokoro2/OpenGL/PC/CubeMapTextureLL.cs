using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.OpenGL.PC
{
    public class CubeMapTextureLL
    {
        protected int width, height;
        protected int id;

        private void FillEntry(Bitmap bmp, TextureTarget t, Engine.PixelComponentType pIf)
        {
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(t, 0, EnumConverters.EPixelComponentType(pIf), bmp_data.Width, bmp_data.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);
        }

        protected int Create(string px, string nx, string py, string ny, string pz, string nz, Engine.PixelComponentType pct)
        {
            Bitmap px_b, nx_b, py_b, ny_b, pz_b, nz_b;

            px_b = new Bitmap(px);
            nx_b = new Bitmap(nx);

            py_b = new Bitmap(py);
            ny_b = new Bitmap(ny);

            pz_b = new Bitmap(pz);
            nz_b = new Bitmap(nz);

            bool w_eq = (px_b.Width == nx_b.Width) && (py_b.Width == ny_b.Width) && (pz_b.Width == nz_b.Width) && (px_b.Width == py_b.Width) && (py_b.Width == pz_b.Width);
            bool h_eq = (px_b.Height == nx_b.Height) && (py_b.Height == ny_b.Height) && (pz_b.Height == nz_b.Height) && (px_b.Height == py_b.Height) && (py_b.Height == pz_b.Height);

            if (!w_eq | !h_eq) throw new ArgumentException("All textures must be the same dimensions");

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, id);

            FillEntry(px_b, TextureTarget.TextureCubeMapPositiveX, pct);
            FillEntry(nx_b, TextureTarget.TextureCubeMapNegativeX, pct);
            FillEntry(py_b, TextureTarget.TextureCubeMapPositiveY, pct);
            FillEntry(ny_b, TextureTarget.TextureCubeMapNegativeY, pct);
            FillEntry(pz_b, TextureTarget.TextureCubeMapPositiveZ, pct);
            FillEntry(nz_b, TextureTarget.TextureCubeMapNegativeZ, pct);

            width = px_b.Width;
            height = px_b.Height;

            px_b.Dispose();
            nx_b.Dispose();
            py_b.Dispose();
            ny_b.Dispose();
            pz_b.Dispose();
            nz_b.Dispose();

            return id;
        }

        protected void BindTexture(int texUnit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texUnit);
            GL.BindTexture(TextureTarget.TextureCubeMap, id);
        }

        protected void UnbindTexture(int texUnit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texUnit);
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        }
    }
}
