using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro2.Math;
using Kokoro2.Engine;

using Kokoro2.Engine.SceneGraph;
using Kokoro2.Engine.Prefabs;
using Kokoro2.Engine.Shaders;
using Kokoro2.Engine.HighLevel.Cameras;
using Kokoro2.Engine.HighLevel.Lights;

namespace Kokoro2.Engine.HighLevel.Rendering
{
    public class LightPass
    {
        List<DirectionalLight> dlights;
        List<PointLight> plights;
        Dictionary<int, Tuple<int, int>> idMap;

        FrameBuffer lightBuffer;

        private static Sphere pLightPrim;
        private static ShaderProgram pLightShader;

        private FullScreenQuad dLightPrim;
        private ShaderProgram dLightShader;

        private ShaderProgram outShader;
        private FullScreenQuad outFSQ;

        private TextureBlurFilter bloomPass, shadowPass;
        private int id = 0;

        private FrameBuffer avgSceneColor;

        private ShaderProgram avgSceneShader;
        private FullScreenQuad avgSceneFSQ;

        public CubeMapTexture EnvironmentMap { get; set; }

        public LightPass(int width, int height, GraphicsContext c)
        {
            outShader = new ShaderProgram(c, VertexShader.Load("LightShadowBloom", c), FragmentShader.Load("LightShadowBloom", c));
            avgSceneShader = new ShaderProgram(c, VertexShader.Load("FrameBuffer", c), FragmentShader.Load("FrameBuffer", c));
            dLightShader = new ShaderProgram(c, VertexShader.Load("DirectionalLight", c), FragmentShader.Load("DirectionalLight", c));

            outFSQ = new FullScreenQuad();
            outFSQ.RenderInfo.PushShader(outShader);

            pLightPrim = new Sphere(1, 10);
            pLightPrim.RenderInfo.PushShader(pLightShader);

            dLightPrim = new FullScreenQuad();
            dLightPrim.RenderInfo.PushShader(dLightShader);

            dlights = new List<DirectionalLight>();
            plights = new List<PointLight>();
            idMap = new Dictionary<int, Tuple<int, int>>();

            lightBuffer = new FrameBuffer(width, height, PixelComponentType.D32, c);
            lightBuffer.Add("Lit", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);
            lightBuffer.Add("Bloom", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment1, c);

            bloomPass = new TextureBlurFilter(width, height, PixelComponentType.RGBA16f, c);
            bloomPass.BlurRadius = 0.0015f * 960 / width;
            shadowPass = new TextureBlurFilter(width, height, PixelComponentType.RGBA8, c);
            shadowPass.BlurRadius = 0.0025f * 960 / width;

            avgSceneFSQ = new FullScreenQuad();
            avgSceneFSQ.RenderInfo.PushShader(avgSceneShader);
            avgSceneColor = new FrameBuffer(1, 1, PixelComponentType.RGBA8, c);
            avgSceneColor.Add("AvgColor", new FrameBufferTexture(1, 1, PixelFormat.BGRA, PixelComponentType.RGBA8, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);
        }

        public int AddLight(PointLight l)
        {
            plights.Add(l);
            idMap[++id] = new Tuple<int, int>(1, plights.Count - 1);
            return id;
        }

        public int AddLight(DirectionalLight l)
        {
            dlights.Add(l);
            idMap[++id] = new Tuple<int, int>(2, dlights.Count - 1);
            return id;
        }

        public void Remove(int id)
        {
            var tmp = idMap[id];
            if (tmp.Item1 == 1) plights.RemoveAt(tmp.Item2);
            else if (tmp.Item1 == 0) dlights.RemoveAt(tmp.Item2);
        }

        public BasicLight this[int id]
        {
            get
            {
                var tmp = idMap[id];
                if (tmp.Item1 == 1) return plights[tmp.Item2];
                else if (tmp.Item1 == 0) return dlights[tmp.Item2];

                return null;
            }
            set
            {
                var tmp = idMap[id];
                if (tmp.Item1 == 1) plights[tmp.Item2] = (PointLight)value;
                else if (tmp.Item1 == 0) dlights[tmp.Item2] = (DirectionalLight)value;
            }
        }

        public void ApplyLights(GBuffer g, GraphicsContext c)
        {
            lightBuffer.Bind(c);
            c.Clear(0, 0, 0, 0);

            dLightShader["colorMap"] = g["Color"];
            dLightShader["normData"] = g["Normal"];
            dLightShader["specularData"] = g["Specular"];
            dLightShader["worldData"] = g["WorldPos"];
            dLightShader["eyePos"] = c.Camera.Position;

            //First apply all directional lights
            for (int i = 0; i < dlights.Count; i++)
            {
                dLightShader["lColor"] = dlights[i].LightColor;
                dLightShader["lDir"] = dlights[i].Direction;
                c.Draw(dLightPrim);
            }

            /*
            var p = c.FaceCulling;
            c.FaceCulling = CullMode.Front;
            //Now apply all point lights
            for (int i = 0; i < plights.Count; i++)
            {
                pLightPrim.World = Matrix4.Scale(plights[i].MaxDistance) * Matrix4.CreateTranslation(plights[i].Position);
                pLightPrim.Draw(c);
            }
            c.FaceCulling = p;
            */

            lightBuffer.UnBind(c);

            avgSceneColor.Bind(c);
            avgSceneFSQ.Material.AlbedoMap = g["Color"];
            c.Draw(avgSceneFSQ);
            avgSceneColor.UnBind(c);

            //Now, blur and blend the shadow map on top of the lighting, then blend in the bloom
            outShader["DiffuseMap"] = g["Color"];
            outShader["LitMap"] = lightBuffer["Lit"];
            outShader["BloomMap"] = bloomPass.ApplyBlur(lightBuffer["Bloom"], c);
            outShader["ShadowMap"] = shadowPass.ApplyBlur(g["Shadow"], c);
            outShader["AvgColor"] = avgSceneColor["AvgColor"];
            c.Draw(outFSQ);

        }

    }
}
