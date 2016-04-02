using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine.Shaders;
using Kokoro2.Engine.Prefabs;
using Kokoro2.Math;

namespace Kokoro2.Engine
{
    public class RenderPass : IDisposable
    {
        public FrameBuffer RenderTarget;
        public ShaderProgram Shader;
        public Texture Diffuse;

        private static FullScreenQuad quad = new FullScreenQuad();

        public RenderPass(int width, int height, PixelComponentType pf, GraphicsContext context)
        {
            RenderTarget = new FrameBuffer(width, height, pf, context);
        }

        public void Clear(GraphicsContext context, float r, float g, float b, float a)
        {
            var tmp = FrameBuffer.GetCurrentFrameBuffer();
            RenderTarget.Bind(context);
            context.Clear(r, g, b, a);
            tmp.Bind(context);
        }

        public void Apply(GraphicsContext context)
        {
            var tmp = FrameBuffer.GetCurrentFrameBuffer();
            RenderTarget.Bind(context);
            quad.RenderInfo.PushShader(Shader);
            quad.Material.AlbedoMap = Diffuse;
            context.Draw(quad);
            tmp.Bind(context);
        }

        public void Dispose()
        {
            Dispose(false, false);
        }

        public void Dispose(bool dispShader, bool dispDiff)
        {
            this.RenderTarget.Dispose();
            if (dispShader) this.Shader.Dispose();
            if (dispDiff) this.Diffuse.Dispose();
        }
    }
}
