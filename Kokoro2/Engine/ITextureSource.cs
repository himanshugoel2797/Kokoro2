using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public interface ITextureSource
    {
        TextureTarget GetTextureTarget();
        int GetDimensions();
        int GetWidth();
        int GetHeight();
        int GetDepth();
        int GetLevels();
        PixelComponentType GetInternalFormat();
        PixelFormat GetFormat();
        PixelType GetType();
        IntPtr GetPixelData();
    }
}
