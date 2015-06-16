#if OPENGL
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public static class GraphicsDevice
    {
        static GraphicsDevice()
        {
            //TODO Implement hardware limit enumeration
            //TODO implement compute shader APIs and MultiDrawIndirect

            //Generate and attach initial VAO object since we do not need it later due to GL_ARB_vertex_attrib_binding
            GL.BindVertexArray(GL.GenVertexArray());
        }

        #region Hardware Information Enumeration
        public static Dictionary<string, int> SystemLimits;
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
        public static int MaxTextureLocations
        {
            get
            {
                return LLDevice.slotCount;
            }
        }

        static Texture curTex;
        public static Texture BindTexture(Texture tex, int location)
        {
            if (location > MaxTextureLocations) throw new ArgumentOutOfRangeException($"Specified location {location} is out of range");
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
    }
}
#endif