using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public static class GraphicsDevice
    {
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
        static TextureArray curTexArray;
        public static TextureArray BindTexture(TextureArray tex)
        {
            TextureArray prev = curTexArray;
            curTexArray = tex;
            LLDevice.BindTexArray(curTexArray.ID);
            return prev;
        }
        #endregion

        #region ShaderProgram State

        #endregion
    }
}
