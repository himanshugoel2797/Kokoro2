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
    public class InstanceBuffer : IDisposable
    {
        GPUBufferLL vertexData;

        public Model ParentModel { get; private set; }
        public int InstancesPerEntrySet { get; private set; }
        public int EntriesPerInstance { get; private set; }
        public int Stride { get; private set; }
        public int Location { get; private set; }

        private float[] dataCopy;

        public InstanceBuffer(GraphicsContext c)
        {
            vertexData = new GPUBufferLL(UpdateMode.Static, BufferUse.Array, 0, c);
            dataCopy = new float[0];
        }

        public void SetData(float[] vals)
        {
            dataCopy = vals;
            vertexData.BufferData(vals);
        }

        public float this[int i]
        {
            get { return dataCopy[i]; }
            set { dataCopy[i] = value; }
        }

        public int Length { get { return dataCopy.Length; } }

        public void BindToModel(Model m, int entriesPerInstance, int instancesPerEntrySet, int stride, GraphicsContext c)
        {
            if (dataCopy.Length == 0) throw new Exception("Set the instance data first!");

            EntriesPerInstance = entriesPerInstance;
            InstancesPerEntrySet = instancesPerEntrySet;
            Stride = stride;
            ParentModel = m;
            m.GeometryInfo.Buffer.AddInstanceBuffer(vertexData, entriesPerInstance, instancesPerEntrySet, stride, 0, c);
            Location = m.GeometryInfo.Buffer.BufferCount - 1;
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
        // ~InstanceBuffer() {
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
