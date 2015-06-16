#if OPENGL
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public class PixelBuffer : IDisposable
    {
        public IntPtr Location { get { return memory.Location; } }

        internal MemoryBlock memory;

        public PixelBuffer(uint size)
        {
            memory = new MemoryBlock(size, Engine.BufferUse.Pixel);
        }

        public void Dispose()
        {
            memory.Dispose();
            GC.SuppressFinalize(this);
        }

        ~PixelBuffer()
        {
            System.Diagnostics.Debug.WriteLine($"[WARN] PixelBuffer {memory.ID} was automatically disposed");
            Dispose();
        }
    }
}
#endif