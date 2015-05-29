using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro2.Engine.Shaders;
using Kokoro2.Engine.Prefabs;

namespace Kokoro2.Engine
{
    /// <summary>
    /// A Pipeline executes a set of RenderPasses
    /// </summary>
    public class Pipeline
    {
        /// <summary>
        /// The GBuffer for the rendering pipeline
        /// </summary>
        public FrameBuffer GBuffer { get; set; }

        private Dictionary<string, RenderPass> Passes;
        private ShaderProgram compositor;
        private static FullScreenQuad fsq = new FullScreenQuad();

        public Pipeline(Dictionary<string, RenderPass> passes, ShaderProgram compositor)
        {
            Passes = passes;
            this.compositor = compositor;
        }

        public void Apply(GraphicsContext Context)
        {
            var tmp = Passes.Values.ToArray();
            for (int i = 0; i < Passes.Count; i++)
            {
                RenderPass cur = tmp[i];

                //Clear the render target
                cur.Clear(Context, 0, 0, 0, 0);

                //Bind the GBuffer textures
                foreach (string key in GBuffer.RenderTargets)
                {
                    cur.Shader.SetTexture(key, GBuffer[key]);
                }

                //Draw the effect
                cur.Apply(Context);
            }

            //Bind all render passes to compositor and render
            foreach (KeyValuePair<string, RenderPass> pass in Passes)
            {
                compositor.SetTexture(pass.Key, pass.Value.RenderTarget["Color"]);
            }
            fsq.Materials[0].Shader = compositor;
            fsq.Draw(Context);

        }
    }
}
