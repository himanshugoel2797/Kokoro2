#if OPENGL && PC

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine;
using Kokoro2.Math;
using OpenTK.Audio.OpenAL;

namespace Kokoro2.OpenGL.PC
{
    public class AudioSourceLL : Engine.IEngineObject
    {
        public ulong ID
        {
            get; set;
        }

        public GraphicsContext ParentContext
        {
            get; set;
        }


        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public int Volume { get; set; }
        public bool Loop { get; set; }

        public AudioSourceLL(AudioBufferLL buffer, GraphicsContext c)
        {
            ParentContext = c;
            ParentContext.Disposing += Dispose;
            ID = c.EngineObjects.RegisterObject(AL.GenSource());

            AL.Source(c.EngineObjects[ID, this.GetType()], ALSourcei.Buffer, c.EngineObjects[buffer.ID, buffer.GetType()]);
            
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
                    AL.DeleteSource(ParentContext.EngineObjects[ID, this.GetType()]);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AudioSourceLL() {
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
#endif