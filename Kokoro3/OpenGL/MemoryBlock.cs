#if OPENGL
using System;
using Kokoro3.Engine;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Kokoro3.OpenGL
{
    public partial class MemoryBlock : IDisposable
    {
        public IntPtr Location { get; private set; }
        internal int ID;
        private BufferStorageFlags flags;
        private BufferTarget target;

        public uint Size { get; private set; }

        public MemoryBlock(uint allocSize, BufferUse use)
        {
            target = EnumConverters.EBufferTarget(use);
            if (use == BufferUse.Array || use == BufferUse.Index || use == BufferUse.Indirect) flags = BufferStorageFlags.MapWriteBit | BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapCoherentBit;

#if DEBUG
            flags |= BufferStorageFlags.MapReadBit;
#endif
            Size = allocSize;
            ID = GL.GenBuffer();
            int prevBuffer = LLDevice.BindBuffer(target, ID);
            GL.BufferStorage(target, (IntPtr)allocSize, IntPtr.Zero, flags);
#if DEBUG
            Location = GL.MapBuffer(target, BufferAccess.ReadWrite);
#else
            Location = GL.MapBuffer(target, BufferAccess.WriteOnly);
#endif
            LLDevice.BindBuffer(target, prevBuffer);
        }

        public void Dispose()
        {
            Location = IntPtr.Zero;
            GL.DeleteBuffer(ID);
            ID = 0;
            GC.SuppressFinalize(this);  //We no longer need to call the finalizer
        }

        ~MemoryBlock()
        {
            System.Diagnostics.Debug.WriteLine($"[WARN] MemoryBlock {ID} was automatically disposed");
            Dispose();
        }
    }
}
#endif