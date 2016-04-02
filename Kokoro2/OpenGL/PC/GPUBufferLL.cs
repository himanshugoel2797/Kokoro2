#if OPENGL && PC
using Kokoro2.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using Kokoro2.Engine.SceneGraph;


namespace Kokoro2.OpenGL.PC
{
    public class GPUBufferLL : IEngineObject
    {
#if GL44
        private IntPtr mappedPtr = IntPtr.Zero;
        private UpdateMode updateMode;
        private BufferTarget target;
        private BufferStorageFlags flags;
        private IntPtr syncObj;
        private int offset, memSize;

        //TODO Setup a mapping system for the offsets so that the buffers can be defragmented if they start to run low on memory without needing elaborate tricks to update all the models

        public GPUBufferLL(UpdateMode mode, BufferUse use, long memSize, GraphicsContext c)
        {
            ParentContext = c;
            ParentContext.Disposing += Dispose;

            updateMode = mode;
            target = EnumConverters.EBufferTarget(use);
            this.memSize = (int)memSize;

            if (use == BufferUse.Array || use == BufferUse.Index || use == BufferUse.Indirect) flags = BufferStorageFlags.MapWriteBit | BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapCoherentBit;

#if DEBUG
            flags |= BufferStorageFlags.MapReadBit;
#endif

            syncObj = GL.FenceSync(SyncCondition.SyncGpuCommandsComplete, WaitSyncFlags.None);  //Initialize the first sync object
            int staticID = GL.GenBuffer();      //Generate the buffer

            ID = ParentContext.EngineObjects.RegisterObject(staticID);

            if (updateMode == UpdateMode.Dynamic)
            {
                //Setup persistent mapping if this is a dynamic buffer
                GL.BindBuffer(target, staticID);
                GL.BufferStorage(target, (IntPtr)(memSize), IntPtr.Zero, flags);
                mappedPtr = GL.MapBuffer(target, BufferAccess.WriteOnly);
                GL.BindBuffer(target, 0);
            }
        }

        public void Bind()
        {
            Bind(target);
        }

        public void Bind(BufferTarget target)
        {
            GL.BindBuffer(target, ParentContext.EngineObjects[ID, this.GetType()]);
        }

        public void UnBind()
        {
            GL.BindBuffer(target, 0);
        }

        public void BindForTransformFeedback(int bindingIndex)
        {
            GL.BindBufferBase(BufferRangeTarget.TransformFeedbackBuffer, bindingIndex, ParentContext.EngineObjects[ID, this.GetType()]);
        }

        public void UnBindForTranformFeedback(int index)
        {
            GL.BindBufferBase(BufferRangeTarget.TransformFeedbackBuffer, index, ParentContext.EngineObjects[ID, this.GetType()]);
        }

        public void FreeAll()
        {
            offset = 0;
        }

        public uint AllocData(int sizeInBytes)
        {
            if (offset + sizeInBytes < memSize) offset += sizeInBytes;
            else throw new OutOfMemoryException("Too much memory requested!");
            return (uint)(offset - sizeInBytes);
        }

        /// <summary>
        /// Append data to the buffer and return the offset it was buffered to
        /// </summary>
        /// <param name="data">The data to append</param>
        /// <returns>The offset it was buffered to</returns>
        public uint AppendData(float[] data)
        {
            BufferData(data, offset, data.Length * sizeof(float));
            offset += data.Length * sizeof(float);
            return (uint)(offset - data.Length * sizeof(float));
        }

        /// <summary>
        /// Append data to the buffer and return the offset it was buffered to
        /// </summary>
        /// <param name="data">The data to append</param>
        /// <returns>The offset it was buffered to</returns>
        public uint AppendData(uint[] data)
        {
            BufferData(data, offset, data.Length * sizeof(uint));
            offset += data.Length * sizeof(uint);
            return (uint)(offset - data.Length * sizeof(uint));
        }

        public uint AppendData(byte[] data)
        {
            BufferData(data, offset, data.Length);
            offset += data.Length;
            return (uint)(offset - data.Length);
        }

        public void PostFence()
        {
            if (updateMode == UpdateMode.Dynamic)
            {
                // lock the buffer:
                GL.DeleteSync(syncObj);
                syncObj = GL.FenceSync(SyncCondition.SyncGpuCommandsComplete, WaitSyncFlags.None);
            }
        }

        /// <summary>
        /// Put data into the buffer
        /// </summary>
        /// <param name="data">The Data</param>
        /// <param name="offset">The offset at which to start writing(only used for dynamic objects)</param>
        /// <param name="length">The amount of data to copy (only used for dynamic objects)</param>
        public void BufferData(float[] data, int offset = 0, int length = -1)
        {
            if (length == -1) length = data.Length * sizeof(float);
            byte[] tmp = new byte[length];
            Buffer.BlockCopy(data, 0, tmp, 0, tmp.Length);
            BufferData(tmp, offset, length);
        }

        /// <summary>
        /// Put data into the buffer
        /// </summary>
        /// <param name="data">The Data</param>
        /// <param name="offset">The offset at which to start writing(only used for dynamic objects)</param>
        /// <param name="length">The amount of data to copy (only used for dynamic objects)</param>
        public void BufferData(byte[] data, int offset = 0, int length = -1)
        {
            if (updateMode == UpdateMode.Dynamic)   //Write to the persistent mapped buffer
            {
                if (offset + length > memSize) throw new OutOfMemoryException("There is a memory overflow here!");

                #region Persistent Mapping
                // waiting for the buffer
                WaitSyncStatus waitReturn = WaitSyncStatus.TimeoutExpired;
                while (waitReturn != WaitSyncStatus.AlreadySignaled && waitReturn != WaitSyncStatus.ConditionSatisfied)
                {
                    waitReturn = GL.ClientWaitSync(syncObj, ClientWaitSyncFlags.SyncFlushCommandsBit, 1);   //TODO depending on how much time this can take, we might want to do other work in the meantime
                }

                //Write the data
                unsafe
                {
                    fixed (byte* SystemMemory = &data[0])
                    {
                        Marshal.Copy(data, 0, (mappedPtr + offset), ((length == -1) ? data.Length : length));
                    }
                }
                #endregion
            }
            else if (updateMode == UpdateMode.Static)
            {
                #region Buffer Data
                GL.BindBuffer(target, ParentContext.EngineObjects[ID, this.GetType()]);
                if (offset == 0) GL.BufferData(target, (IntPtr)((length == -1) ? data.Length : length), data, BufferUsageHint.StaticDraw);
                else GL.BufferSubData(target, (IntPtr)(offset), (IntPtr)((length == -1) ? data.Length : length), data);
                GL.BindBuffer(target, 0);
                #endregion
            }
        }

        public void BufferData<T>(T data, int offset = 0, int length = -1)
        {
            if (updateMode == UpdateMode.Dynamic)   //Write to the persistent mapped buffer
            {
                if (offset + length > memSize) throw new OutOfMemoryException("There is a memory overflow here!");

                #region Persistent Mapping
                // waiting for the buffer
                WaitSyncStatus waitReturn = WaitSyncStatus.TimeoutExpired;
                while (waitReturn != WaitSyncStatus.AlreadySignaled && waitReturn != WaitSyncStatus.ConditionSatisfied)
                {
                    waitReturn = GL.ClientWaitSync(syncObj, ClientWaitSyncFlags.SyncFlushCommandsBit, 1);   //TODO depending on how much time this can take, we might want to do other work in the meantime
                }

                //Write the data
                Marshal.StructureToPtr(data, (mappedPtr + offset), false);
                #endregion
            }
            else if (updateMode == UpdateMode.Static)
            {
                #region Buffer Data
                GL.BindBuffer(target, ParentContext.EngineObjects[ID, this.GetType()]);
                if (offset == 0) GL.BufferData(target, (IntPtr)((length == -1) ? Marshal.SizeOf(data) : length), getBytes(data), BufferUsageHint.StaticDraw);
                else GL.BufferSubData(target, (IntPtr)(offset), (IntPtr)((length == -1) ? Marshal.SizeOf(data) : length), getBytes(data));
                GL.BindBuffer(target, 0);
                #endregion
            }
        }

        byte[] getBytes<T>(T str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        //TODO Update these comments, offsets and lenths are valid for everything
        //TODO several possible bug fixes, the length checks need to be made more robust for general purpose use

        /// <summary>
        /// Put data into the buffer
        /// </summary>
        /// <param name="data">The Data</param>
        /// <param name="offset">The offset at which to start writing(only used for dynamic objects)</param>
        /// <param name="length">The amount of data to copy (only used for dynamic objects)</param>
        public void BufferData(uint[] data, int offset = 0, int length = -1)
        {
            if (length == -1) length = data.Length * sizeof(uint);
            byte[] tmp = new byte[length];
            Buffer.BlockCopy(data, 0, tmp, 0, length);
            BufferData(tmp, offset, length);
        }
#else

#endif

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public ulong ID
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public GraphicsContext ParentContext
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).

                    Kokoro2.Engine.ObjectAllocTracker.ObjectDestroyed(this);
                    if (updateMode == UpdateMode.Dynamic)
                    {
                        //GL.BindBuffer(BufferTarget.ArrayBuffer, staticID);
                        //GL.UnmapBuffer(BufferTarget.ArrayBuffer);
                        GL.DeleteBuffer(ParentContext.EngineObjects[ID, this.GetType()]);
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        ~GPUBufferLL()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

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