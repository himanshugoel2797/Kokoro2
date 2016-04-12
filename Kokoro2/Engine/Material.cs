using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Kokoro2.Math;
using Kokoro2.Engine.Shaders;
using System.Xml.Serialization;

namespace Kokoro2.Engine
{
    public struct TextureSet
    {
        public string Name { get; set; }
        public string File { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public bool AlbedoMap { get; set; }
        public bool ReflectivityMap { get; set; }
        public bool RoughnessMap { get; set; }
        public bool NormalMap { get; set; }
        public bool EmissiveMap { get; set; }

        public string AlbedoMapFile { get; set; }
        public string ReflectivityMapFile { get; set; }
        public string RoughnessMapFile { get; set; }
        public string NormalMapFile { get; set; }
        public string EmissiveMapFile { get; set; }
    }

    public class Material
    {
        public string Name { get; set; }
        public Texture AlbedoMap { get; set; }
        public Texture SpecularMap { get; set; }
        public Texture RoughnessMap { get; set; }
        public Texture NormalMap { get; set; }
        public Texture PackedMap { get; set; }
        public Texture EmissionMap { get; set; }

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

        public static Material Load(string file, GraphicsContext c)
        {
            XmlSerializer s = new XmlSerializer(typeof(TextureSet));
            TextureSet s0;
            using (Stream strm = File.OpenRead(file)) s0 = (TextureSet)s.Deserialize(strm);

            Material m = new Material();
            m.Name = s0.Name;

            if (s0.AlbedoMap) m.AlbedoMap = ImageTextureSource.Create("Resources/Proc/Tex/" + Path.GetFileName(s0.AlbedoMapFile), 0, true, c);
            if (s0.ReflectivityMap) m.SpecularMap = ImageTextureSource.Create("Resources/Proc/Tex/" + Path.GetFileName(s0.ReflectivityMapFile), 0, false, c);
            if (s0.NormalMap) m.NormalMap = ImageTextureSource.Create("Resources/Proc/Tex/" + Path.GetFileName(s0.NormalMapFile), 0, false, c);
            if (s0.RoughnessMap) m.RoughnessMap = ImageTextureSource.Create("Resources/Proc/Tex/" + Path.GetFileName(s0.RoughnessMapFile), 0, false, c);
            if (s0.EmissiveMap) m.EmissionMap = ImageTextureSource.Create("Resources/Proc/Tex/" + Path.GetFileName(s0.EmissiveMapFile), 0, false, c);

            return m;
        }
    }
}
