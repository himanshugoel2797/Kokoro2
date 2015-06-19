#if OPENGL
using Kokoro3.Math;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public static class GraphicsDevice
    {
        static GraphicsDevice()
        {
            //TODO implement compute shader APIs and MultiDrawIndirect

            SystemLimits = new Dictionary<HardwareLimits, int>();
            InitializeHWLimits();

            GL.DebugMessageCallback(debugCallback, IntPtr.Zero);

            //Generate and attach initial VAO object since we do not need it later due to GL_ARB_vertex_attrib_binding
            GL.BindVertexArray(GL.GenVertexArray());
        }

        private static void debugCallback(DebugSource src, DebugType type, int id, DebugSeverity severity, int length, IntPtr msg, IntPtr uParam)
        {
            System.Diagnostics.Debug.WriteLine($"[{severity.ToString().Replace("DebugSeverity", "")}][{src.ToString().Replace("DebugSource", "")}][{id}]{Marshal.PtrToStringAnsi(msg, length)}");
        }

        #region Hardware Information Enumeration
        public static Dictionary<HardwareLimits, int> SystemLimits;

        internal static void InitializeHWLimits()
        {
            Action<HardwareLimits> limitCal = (h) => SystemLimits[h] = GL.GetInteger((GetPName)h);

            limitCal(HardwareLimits.MajorVersion);
            limitCal(HardwareLimits.Max3DTextureSize);
            limitCal(HardwareLimits.MaxArrayTextureLayers);
            limitCal(HardwareLimits.MaxColorAttachments);
            limitCal(HardwareLimits.MaxIndices);
            limitCal(HardwareLimits.MaxTextureSize);
            limitCal(HardwareLimits.MaxTextureUnits);
            limitCal(HardwareLimits.MaxVertices);
            limitCal(HardwareLimits.MaxViewports);
            limitCal(HardwareLimits.MaxViewportSize);
            limitCal(HardwareLimits.MinorVersion);
        }
        #endregion

        #region Vertex Buffer State Machine
        public static void BindVertexBuffer(MemoryBlock block, int bindingPoint, int offset, int stride)
        {
            GL.BindVertexBuffer(bindingPoint, block.ID, (IntPtr)offset, stride);
        }
        public static void BindAttribute(int attributeNum, int bindingPoint, int valsPerVertex, int valsPerInstance)
        {
            GL.EnableVertexAttribArray(attributeNum);
            GL.VertexAttribBinding(attributeNum, bindingPoint);
            GL.VertexAttribFormat(attributeNum, valsPerVertex, VertexAttribType.Float, false, 0);
            GL.VertexAttribDivisor(attributeNum, valsPerInstance);
        }
        #endregion

        #region FrameBuffer State
        static FrameBuffer curFBuf;
        public static FrameBuffer BindFrameBuffer(FrameBuffer frameBuffer)
        {
            FrameBuffer prev = curFBuf;
            curFBuf = frameBuffer;
            LLDevice.BindFrameBuffer(curFBuf.ID);
            return prev;
        }
        #endregion

        #region Texture State
        static Texture curTex;
        public static Texture BindTexture(Texture tex, int location)
        {
            if (location > SystemLimits[HardwareLimits.MaxTextureUnits]) throw new ArgumentOutOfRangeException($"Specified location {location} is out of range");
            Texture prev = curTex;
            curTex = tex;
            int prevLoc = LLDevice.SetActiveSlot(location);
            LLDevice.BindTex(OpenGL.EnumConverters.ETextureTarget(tex.TexType), tex.ID);
            LLDevice.SetActiveSlot(prevLoc);
            return prev;
        }
        #endregion

        #region ShaderProgram State
        static ShaderProgram curShader;
        public static ShaderProgram BindShader(ShaderProgram shader)
        {
            ShaderProgram prev = curShader;
            curShader = shader;
            LLDevice.BindShaderProgram(shader.ID);
            return prev;
        }
        #endregion

        #region OpenGL General functions
        public static void SetClearColor(Vector4 color)
        {
            GL.ClearColor(color.X, color.Y, color.Z, color.W);
        }
        public static void ClearColorBuffer()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
        public static void ClearDepthBuffer()
        {
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }
        public static void CheckError()
        {
            var err = GL.GetError();
            if (err != ErrorCode.NoError) throw new Exception(err.ToString());
        }
        #endregion
    }
}
#endif