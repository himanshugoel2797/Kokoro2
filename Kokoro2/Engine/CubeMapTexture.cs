using Kokoro2.OpenGL.PC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public class CubeMapTexture : CubeMapTextureLL
    {
        public CubeMapTexture(string px, string nx, string py, string ny, string pz, string nz, Engine.PixelComponentType pct)
        {
            id = Create(px, nx, py, ny, pz, nz, pct);
        }

        public void Bind(int texUnit)
        {
            BindTexture(texUnit);
        }

        public void UnBind(int texUnit)
        {
            UnbindTexture(texUnit);
        }
    }
}
