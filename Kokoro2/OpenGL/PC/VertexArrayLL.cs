#if OPENGL && PC
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Kokoro2.OpenGL.PC
{
    public class VertexArrayLL : Engine.IEngineObject
    {
        List<GPUBufferLL> buffers;
        List<int> elementCount, divisor, stride, offset;
        Kokoro2.Engine.BufferUse[] bufferUses;

        /// <summary>
        /// Allows access of the underlying GPU buffers
        /// </summary>
        /// <param name="index">The index of the GPU buffer to access</param>
        /// <returns>Returnss the relevant GPUBufferLL object</returns>
        public GPUBufferLL this[int index]
        {
            get
            {
                return buffers[index];
            }
        }

        public int BufferCount
        {
            get
            {
                return buffers.Count;
            }
        }

        public ulong ID
        {
            get; set;
        }

        public Engine.GraphicsContext ParentContext
        {
            get; set;
        }

        public void PostFence()
        {
            for (int i = 0; i < buffers.Count; i++)
            {
                buffers[i].PostFence();
            }
        }

        public VertexArrayLL(int bufferCount, long bufferSize, Kokoro2.Engine.UpdateMode updateMode, Kokoro2.Engine.BufferUse[] bufferUses, int[] elementCount, Engine.GraphicsContext c)
        {
            ParentContext = c;
            ParentContext.Disposing += Dispose;

            ID = ParentContext.EngineObjects.RegisterObject(GL.GenVertexArray());
            BindingManager.BindVertexArray(ParentContext.EngineObjects[ID, this.GetType()]);

            if (bufferSize == 0) bufferSize = 1;

            //Generate all GPUBuffers
            buffers = new List<GPUBufferLL>(bufferCount);
            for (int i = 0; i < bufferCount; i++)
            {
                buffers.Add(new GPUBufferLL(updateMode, bufferUses[i], bufferSize * elementCount[i] * 8, c));  //NOTE: bufferSize no longer refers to the number of bytes of data, but the number of elements in total
            }

            this.elementCount = new List<int>(elementCount);
            this.divisor = new List<int>();
            this.offset = new List<int>();
            this.stride = new List<int>();
            this.bufferUses = bufferUses;

            for (int j = 0; j < bufferCount; j++)
            {
                divisor.Add(0);
                offset.Add(0);
                stride.Add(0);
            }
            GL.EnableVertexAttribArray(0);
            BindingManager.UnbindVertexArray();
        }

        public VertexArrayLL(int bufferCount, Kokoro2.Engine.UpdateMode updateMode, Kokoro2.Engine.BufferUse[] bufferUses, int[] elementCount, Engine.GraphicsContext c) :
            this(bufferCount, 0, updateMode, bufferUses, elementCount, c)
        {

        }

        public void AddInstanceBuffer(GPUBufferLL buf, int elementCount, int divisor, int stride, int offset, Engine.GraphicsContext c)
        {
            int loc = buffers.Count;
            buffers.Add(buf);
            this.elementCount.Add(elementCount);

            Bind();
            GL.EnableVertexAttribArray(loc);
            buf.Bind();
            GL.VertexAttribPointer(loc, elementCount, VertexAttribPointerType.Float, false, stride, offset);
            GL.VertexAttribDivisor(loc, divisor);

            this.divisor.Add(divisor);
            this.stride.Add(stride);
            this.offset.Add(offset);

            GL.EnableVertexAttribArray(0);
            UnBind();
        }

        //Bind the VAO
        public void Bind()
        {
            BindingManager.BindVertexArray(ParentContext.EngineObjects[ID, this.GetType()]);
            int bufferCount = elementCount.Count;

            int j = 0;
            for (int i = 0; i < elementCount.Count; i++)
            {
                if (bufferUses[i] == Engine.BufferUse.Index)
                {
                    buffers[i].Bind();
                    continue;
                }
                else {

                    GL.EnableVertexAttribArray(j);
                    buffers[i].Bind();
                    GL.VertexAttribPointer(j, elementCount[i], VertexAttribPointerType.Float, false, stride[i], offset[i]);
                    GL.VertexAttribDivisor(j, divisor[i]);
                    j++;
                }
            }
        }

        //Unbind the VAO
        public void UnBind()
        {
            for (int j = 0; j < elementCount.Count; j++)
            {
                buffers[j].UnBind();
            }

            BindingManager.UnbindVertexArray();
        }

        public void Dispose()
        {
            if (ID != 0)
            {
                //Mark all owned GPUBuffers to be erased as well
                for (int i = 0; i < buffers.Count; i++)
                {
                    buffers[i].Dispose();
                }

                Kokoro2.Engine.ObjectAllocTracker.ObjectDestroyed(this);
                GL.DeleteVertexArray(ParentContext.EngineObjects[ID, this.GetType()]);
            }
        }

        public void ResetAll()
        {
            for (int i = 0; i < buffers.Count; i++)
            {
                buffers[i].FreeAll();
            }
        }

        ~VertexArrayLL()
        {

        }

    }
}
#endif