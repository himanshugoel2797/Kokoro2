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
    public class VertexBatch : IDisposable
    {
        internal VertexArrayLL Buffer;
        long[] BufferLength;

        public VertexBatch(int maxVertexCount, GraphicsContext c)
        {
            /*
             * 0: Index
             * 1: Vertex
             * 2: Normal
             * 3: UV
             */

            BufferLength = new long[4] { maxVertexCount * 1, maxVertexCount * 3, maxVertexCount * 3, maxVertexCount * 2 };

            Buffer = new VertexArrayLL(BufferLength.Length, BufferLength[0], UpdateMode.Static,
                new BufferUse[] { BufferUse.Index, BufferUse.Array, BufferUse.Array, BufferUse.Array },
                new int[] { 1, 3, 3, 2 }, c);
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~VertexBatch() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
