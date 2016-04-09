using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Kokoro2.Math;
using Kokoro2.Engine.Shaders;

namespace Kokoro2.Engine
{
    public class Material
    {
        public string Name { get; set; }
        public Texture AlbedoMap { get; set; }
        public Texture SpecularMap { get; set; }
        public Texture RoughnessMap { get; set; }
        public Texture NormalMap { get; set; }
        public Texture PackedMap { get; set; }

        private static ShaderProgram texPackShader;
        private static FrameBuffer texPackBuffer;
        private static Prefabs.FullScreenQuad texPackFSQ;
        public static Texture PackTextures(Texture NormalMap, Texture RoughnessMap, Texture SpecularMap, GraphicsContext c)
        {
            if (texPackShader == null)
            {
                //Load the packing shader and initialize the framebuffer
                texPackShader = new ShaderProgram(c, VertexShader.Load("PackGlossRoughNorm", c), FragmentShader.Load("PackGlossRoughNorm", c));
                texPackFSQ = new Prefabs.FullScreenQuad(c);
                texPackFSQ.RenderInfo.PushShader(texPackShader);
            }

            if (NormalMap == null | RoughnessMap == null | SpecularMap == null) throw new ArgumentNullException();
            //Ensure the dimensions of all three sets of textures are the same and create a texture to pack everything to
            if (NormalMap.Width != RoughnessMap.Width && NormalMap.Width != SpecularMap.Width && NormalMap.Height != RoughnessMap.Height && NormalMap.Height != SpecularMap.Height) throw new ArgumentException("All three textures must be the same size");
            texPackBuffer = new FrameBuffer(NormalMap.Width, NormalMap.Height, c);
            texPackBuffer.Add("Color", FramebufferTextureSource.Create(NormalMap.Width, NormalMap.Height, 0, PixelComponentType.RGBA8, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);

            //Perform the draw
            texPackBuffer.Bind(c);
            texPackFSQ.Material.NormalMap = NormalMap;
            texPackFSQ.Material.RoughnessMap = RoughnessMap;
            texPackFSQ.Material.SpecularMap = SpecularMap;
            c.Draw(texPackFSQ);
            texPackBuffer.UnBind(c);

            //Return the texture, the next call will automatically overwrite the current texture binding
            Texture v = texPackBuffer["Color"];
            texPackBuffer.Dispose();

            return v;
        }

        public static Texture PackTextures(Material m, GraphicsContext c)
        {
            return PackTextures(m.NormalMap, m.RoughnessMap, m.SpecularMap, c);
        }
    }
}
