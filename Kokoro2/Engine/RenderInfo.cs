using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public struct RenderInfo
    {
        public Matrix4 World;
        public DrawMode DrawMode;
        public Material Material;

        #region Shader Management
        public Shaders.ShaderProgram Shader
        {
            get
            {
                if (ShaderQueue != null && ShaderQueue.Count > 0)
                    return ShaderQueue.Peek();
                else return null;
            }
        }

        private Stack<Shaders.ShaderProgram> ShaderQueue;

        public void PushShader(Shaders.ShaderProgram s)
        {
            if (ShaderQueue == null) ShaderQueue = new Stack<Shaders.ShaderProgram>();
            ShaderQueue.Push(s);
        }

        public Shaders.ShaderProgram PopShader()
        {
            if (ShaderQueue != null && ShaderQueue.Count > 0) return ShaderQueue.Pop();
            else return null;
        }
        #endregion

    }
}
