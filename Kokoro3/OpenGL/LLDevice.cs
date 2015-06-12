#if OPENGL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Kokoro3.OpenGL
{
    static class LLDevice
    {
        //Contains methods related to managing the opengl state machine so that we can cut down on state changes as much as possible

        static LLDevice()
        {
            #region currentBoundBuffer Initialization
            currentBoundBuffer = new Dictionary<BufferTarget, int>();
            currentBoundBuffer[BufferTarget.ArrayBuffer] = 0;
            currentBoundBuffer[BufferTarget.AtomicCounterBuffer] = 0;
            currentBoundBuffer[BufferTarget.CopyReadBuffer] = 0;
            currentBoundBuffer[BufferTarget.CopyWriteBuffer] = 0;
            currentBoundBuffer[BufferTarget.DispatchIndirectBuffer] = 0;
            currentBoundBuffer[BufferTarget.DrawIndirectBuffer] = 0;
            currentBoundBuffer[BufferTarget.ElementArrayBuffer] = 0;
            currentBoundBuffer[BufferTarget.PixelPackBuffer] = 0;
            currentBoundBuffer[BufferTarget.PixelUnpackBuffer] = 0;
            currentBoundBuffer[BufferTarget.QueryBuffer] = 0;
            currentBoundBuffer[BufferTarget.ShaderStorageBuffer] = 0;
            currentBoundBuffer[BufferTarget.TextureBuffer] = 0;
            currentBoundBuffer[BufferTarget.TransformFeedbackBuffer] = 0;
            currentBoundBuffer[BufferTarget.UniformBuffer] = 0;
            #endregion
        }

        #region Buffer Management
        internal static Dictionary<BufferTarget, int> currentBoundBuffer;
        internal static int BindBuffer(BufferTarget target, int id)
        {
            int curBuf = currentBoundBuffer[target];
            if (id != curBuf) GL.BindBuffer(target, id);
            currentBoundBuffer[target] = id;
            return curBuf;
        }
        internal static int GetBoundBufferID(BufferTarget target)
        {
            return currentBoundBuffer[target];
        }
        #endregion

        #region TextureArrayManagement
        static int curBoundTexArray = 0;
        internal static int BindTexArray(int id)
        {
            if (id != curBoundTexArray) GL.BindTexture(TextureTarget.Texture2DArray, id);
            int tmp = curBoundTexArray;
            curBoundTexArray = id;
            return tmp;
        }
        #endregion

        #region Framebuffer
        static int curBoundFrameBuffer = 0;
        internal static int BindFrameBuffer(int ID)
        {
            int curFBuf = curBoundFrameBuffer;
            curBoundFrameBuffer = ID;
            if (curBoundFrameBuffer != ID) GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
            return curFBuf;
        }
        #endregion
    }
}
#endif