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

            #region curBoundTex Initialization
            curBoundTex = new Dictionary<TextureTarget, int>();
            curBoundTex[TextureTarget.Texture1D] = 0;
            curBoundTex[TextureTarget.Texture2D] = 0;
            curBoundTex[TextureTarget.Texture2DArray] = 0;
            curBoundTex[TextureTarget.Texture1DArray] = 0;
            curBoundTex[TextureTarget.Texture3D] = 0;
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
        //NOTE BindBufferBase changes bindingpoint state without reporting it, this might need changing later
        internal static int BindBufferBase(BufferTarget target, int id, int bindingPoint)
        {
            //Mostly the same as BindBuffer except it's constrained for certain purposes
            if (target != BufferTarget.TransformFeedbackBuffer && target != BufferTarget.UniformBuffer && target != BufferTarget.ShaderStorageBuffer && target != BufferTarget.AtomicCounterBuffer)
                throw new ArgumentException("BindBufferBase may only be used with TransformFeedbackBuffer and UniformBuffer");

            int curBuf = currentBoundBuffer[target];
            GL.BindBufferBase((BufferRangeTarget)target, bindingPoint, id);
            currentBoundBuffer[target] = id;
            return curBuf;
        }
        internal static int GetBoundBufferID(BufferTarget target)
        {
            return currentBoundBuffer[target];
        }
        #endregion

        #region Texture Management
        static Dictionary<TextureTarget, int> curBoundTex;
        internal static int BindTex(TextureTarget t, int id)
        {
            if (id != curBoundTex[t]) GL.BindTexture(t, id);
            int tmp = curBoundTex[t];
            curBoundTex[t] = id;
            return tmp;
        }
        #endregion

        #region Framebuffer
        static int curBoundFrameBuffer = 0;
        internal static int BindFrameBuffer(int ID)
        {
            int curFBuf = curBoundFrameBuffer;
            curBoundFrameBuffer = ID;
            if (curFBuf != ID) GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
            return curFBuf;
        }
        #endregion

        #region ShaderProgram
        static int curShaderProgramID = 0;
        internal static int BindShaderProgram(int id)
        {
            int prev = curShaderProgramID;
            curShaderProgramID = id;
            if (prev != id) GL.UseProgram(id);
            return prev;
        }
        #endregion
    }
}
#endif