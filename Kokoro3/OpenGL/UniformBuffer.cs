#if OPENGL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public class UniformBuffer : IDisposable
    {
        public const uint MaxUniformBufferSize = 16 * 1024;

        public uint Size
        {
            get
            {
                return Storage.Size;
            }
        }

        internal MemoryBlock Storage;
        public UniformBuffer(uint size)
        {
            if (size > MaxUniformBufferSize) throw new ArgumentOutOfRangeException($"'size' must be less than or equal to 'UniformBuffer.MaxUniformBufferSize'({MaxUniformBufferSize} bytes)");
            Storage = new MemoryBlock(size, Engine.BufferUse.Uniform);
        }

        public void AddStruct(object data, int offset)
        {
            //TODO convert struct to UBO data and put it into the buffer
            Marshal.StructureToPtr(data, Storage.Location + offset, false);
        }

        public void Bind(int bindPoint)
        {
            LLDevice.BindBufferBase(OpenTK.Graphics.OpenGL4.BufferTarget.UniformBuffer, Storage.ID, bindPoint);
        }

        public void Dispose()
        {
            Storage.Dispose();
            GC.SuppressFinalize(this);
        }

        ~UniformBuffer()
        {
            System.Diagnostics.Debug.WriteLine($"[WARN] A UniformBuffer object was automatically disposed");
            Dispose();
        }

    }
}
#endif