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
        List<int> elementCount;
        Kokoro2.Engine.BufferUse[] bufferUses;

        GPUBufferLL ibo;

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

            if (bufferSize == 0) bufferSize = 1;

            //Generate all GPUBuffers
            buffers = new List<GPUBufferLL>(bufferCount);
            for (int i = 0; i < bufferCount; i++)
            {
                buffers.Add(new GPUBufferLL(updateMode, bufferUses[i], bufferSize * elementCount[i] * 8, c));  //NOTE: bufferSize no longer refers to the number of bytes of data, but the number of elements in total
            }

            this.elementCount = new List<int>(elementCount);
            this.bufferUses = bufferUses;

            ID = ParentContext.EngineObjects.RegisterObject(GL.GenVertexArray());
            GL.BindVertexArray(ParentContext.EngineObjects[ID, this.GetType()]);

            if (bufferUses[0] == Engine.BufferUse.Index)
            {
                ibo = buffers[0];
                ibo.Bind();
                bufferCount--;
            }

            for (int j = 0; j < bufferCount; j++)
            {
                int i = j;
                if (bufferUses[0] == Engine.BufferUse.Index)
                {
                    i = j + 1;
                }
                GL.EnableVertexAttribArray(j);
                buffers[i].Bind();
                GL.VertexAttribPointer(j, elementCount[i], VertexAttribPointerType.Float, false, 0, 0);
                GL.VertexAttribDivisor(j, 0);

            }
            GL.EnableVertexAttribArray(0);
            GL.BindVertexArray(0);
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

            GL.BindVertexArray(ParentContext.EngineObjects[ID, this.GetType()]);
            GL.EnableVertexAttribArray(loc);
            buf.Bind();
            GL.VertexAttribPointer(loc, elementCount, VertexAttribPointerType.Float, false, stride, offset);
            GL.VertexAttribDivisor(loc, divisor);

            GL.EnableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }

        //Bind the VAO
        public void Bind()
        {
            GL.BindVertexArray(ParentContext.EngineObjects[ID, this.GetType()]);
            int bufferCount = elementCount.Count;

            if (bufferUses[0] == Engine.BufferUse.Index)
            {
                ibo = buffers[0];
                ibo.Bind();
                bufferCount--;
            }

            for (int j = 0; j < bufferCount; j++)
            {
                int i = j;
                if (bufferUses[0] == Engine.BufferUse.Index)
                {
                    i++;
                }
                GL.EnableVertexAttribArray(j);
                buffers[i].Bind();
                GL.VertexAttribPointer(j, elementCount[i], VertexAttribPointerType.Float, false, 0, 0);
                GL.VertexAttribDivisor(j, 0);

            }

            if (ibo != null) ibo.Bind();

        }

        //Unbind the VAO
        public void UnBind()
        {
            GL.BindVertexArray(0);
            if (ibo != null) ibo.UnBind();
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