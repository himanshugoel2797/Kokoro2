using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if OPENGL && PC
using Kokoro2.OpenGL.PC;
#endif

namespace Kokoro2.Engine
{
    public struct GeometryInfo
    {
        internal VertexArrayLL Buffer;

        public ulong StorageID { get { return Buffer.ID; } }
        public long Length;
        public long PrimitiveCount;
    }
}
