﻿#if OPENGL
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

        //NOTE This value can be changed to adjust the amount of textures that can be bound
        internal static int slotCount = 16;


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
            curBoundTex = new Dictionary<TextureTarget, int>[slotCount];
            for (int i = 0; i < curBoundTex.Length; i++)
            {
                curBoundTex[i] = new Dictionary<TextureTarget, int>();
                curBoundTex[i][TextureTarget.Texture1D] = 0;
                curBoundTex[i][TextureTarget.Texture2D] = 0;
                curBoundTex[i][TextureTarget.Texture2DArray] = 0;
                curBoundTex[i][TextureTarget.Texture1DArray] = 0;
                curBoundTex[i][TextureTarget.Texture3D] = 0;
            }
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
        static int curSlot = 0;
        static Dictionary<TextureTarget, int>[] curBoundTex;
        internal static int BindTex(TextureTarget t, int id)
        {
            if (id != curBoundTex[curSlot][t]) GL.BindTexture(t, id);
            int tmp = curBoundTex[curSlot][t];
            curBoundTex[curSlot][t] = id;
            return tmp;
        }
        internal static int SetActiveSlot(int slot)
        {
            if (slot >= slotCount) throw new ArgumentException($"The slot #{slot} is invalid, valid values are from 0 to {slotCount - 1}");
            int prev = curSlot;
            curSlot = slot;
            GL.ActiveTexture(TextureUnit.Texture0 + curSlot);
            return prev;
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