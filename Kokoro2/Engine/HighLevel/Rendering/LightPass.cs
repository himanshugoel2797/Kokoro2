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

        private FrameBuffer ssrBuffer;
        private FullScreenQuad ssrFSQ;
        private ShaderProgram ssrShader;

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

        private InstanceBuffer giLightInstanceData;
        private Sphere giLightPrim;
        private ShaderProgram giShader;

        public Texture EnvironmentMap { get; set; }
        public DirectionalLight GILight { get; set; }   //The light that sources the global illumination data

        public LightPass(int width, int height, GraphicsContext c)
        {
            outShader = new ShaderProgram(c, VertexShader.Load("LightShadowBloom", c), FragmentShader.Load("LightShadowBloom", c));
            avgSceneShader = new ShaderProgram(c, VertexShader.Load("FrameBuffer", c), FragmentShader.Load("FrameBuffer", c));
            dLightShader = new ShaderProgram(c, VertexShader.Load("DirectionalLight", c), FragmentShader.Load("DirectionalLight", c));
            pLightShader = new ShaderProgram(c, VertexShader.Load("PointLight", c), FragmentShader.Load("PointLight", c));
            giShader = new ShaderProgram(c, VertexShader.Load("RSM", c), FragmentShader.Load("RSM", c));
            ssrShader = new ShaderProgram(c, VertexShader.Load("SSR", c), FragmentShader.Load("SSR", c));

            ssrFSQ = new FullScreenQuad(c);
            ssrFSQ.RenderInfo.PushShader(ssrShader);

            outFSQ = new FullScreenQuad(c);
            outFSQ.RenderInfo.PushShader(outShader);

            pLightPrim = new Sphere(1, 20, c);
            pLightPrim.RenderInfo.PushShader(pLightShader);

            dLightPrim = new FullScreenQuad(c);
            dLightPrim.RenderInfo.PushShader(dLightShader);

            giLightPrim = new Sphere(1, 20, c);
            giLightPrim.RenderInfo.PushShader(giShader);

            dlights = new List<DirectionalLight>();
            plights = new List<PointLight>();
            idMap = new Dictionary<int, Tuple<int, int>>();

            lightBuffer = new FrameBuffer(width, height, c);
            lightBuffer.Add("Lit", FramebufferTextureSource.Create(width, height, 0, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);
            lightBuffer.Add("Bloom", FramebufferTextureSource.Create(width, height, 0, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment1, c);

            ssrBuffer = new FrameBuffer(width, height, c);
            ssrBuffer.Add("SSR", FramebufferTextureSource.Create(width, height, 0, PixelComponentType.RGBA8, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);


            bloomPass = new TextureBlurFilter(width, height, PixelComponentType.RGBA16f, c);
            bloomPass.BlurRadius = 0.0015f * 960 / width;
            shadowPass = new TextureBlurFilter(width, height, PixelComponentType.RGBA8, c);
            shadowPass.BlurRadius = 0.0025f * 960 / width;

            avgSceneFSQ = new FullScreenQuad(c);
            avgSceneFSQ.RenderInfo.PushShader(avgSceneShader);
            avgSceneColor = new FrameBuffer(1, 1, c);
            avgSceneColor.Add("AvgColor", FramebufferTextureSource.Create(1, 1, 0, PixelComponentType.RGBA8, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);
            avgSceneColor.Add("DepthBuffer", DepthTextureSource.Create(1, 1, PixelComponentType.D32, c), FrameBufferAttachments.DepthAttachment, c);

            //Precalculate PBR data
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
            if (preCalcQuad == null) Precalculate(c);
            ssrShader["worldData"] = g["WorldPos"];
            ssrShader["normData"] = g["Normal"];
            ssrShader["depthMap"] = g["DepthBuffer"];
            ssrShader["colorMap"] = g["Color"];
            g["Color"].WrapX = false;
            g["Color"].WrapY = false;
            g["Normal"].WrapX = false;
            g["Normal"].WrapY = false;
            g["Color"].FilterMode = TextureFilter.Linear;
            g["Normal"].FilterMode = TextureFilter.Linear;

            ssrBuffer.Bind(c);
            c.Draw(ssrFSQ);
            ssrBuffer.UnBind(c);


            lightBuffer.Add("DepthBuffer", g["DepthBuffer"], FrameBufferAttachments.DepthAttachment, c);
            lightBuffer.Bind(c);

            c.DepthWrite = false;
            c.Blend = true;
            c.Blending = new BlendFunc()
            {
                Src = BlendingFactor.One,
                Dst = BlendingFactor.One
            };
            c.DepthFunction = DepthFunc.Always;

            c.ClearColor(0, 0, 0, 0);

            dLightShader["colorMap"] = g["Color"];
            dLightShader["normData"] = g["Normal"];
            dLightShader["worldData"] = g["WorldPos"];
            dLightShader["envMap"] = EnvironmentMap;
            dLightShader["ssrMap"] = ssrBuffer["SSR"];
            dLightShader["preCalc"] = preCalcBuffer["PreCalc"];

            //First apply all directional lights
            for (int i = 0; i < dlights.Count; i++)
            {
                dLightShader["lColor"] = dlights[i].LightColor;
                dLightShader["lDir"] = dlights[i].Direction;
                c.Draw(dLightPrim);
            }

            var p = c.FaceCulling;
            c.FaceCulling = CullMode.Back;
            c.DepthFunction = DepthFunc.LEqual;

            pLightShader["colorMap"] = g["Color"];
            pLightShader["normData"] = g["Normal"];
            pLightShader["worldData"] = g["WorldPos"];
            pLightShader["envMap"] = EnvironmentMap;

            List<int> redoVolums = new List<int>(); //Volumes to redo because we're inside them

            //Now apply all point lights
            for (int i = 0; i < plights.Count; i++)
            {
                if ((c.Camera.Position - plights[i].Position).LengthSquared <= plights[i].MaxDistance * plights[i].MaxDistance)
                {
                    redoVolums.Add(i);
                    continue;
                }
                pLightShader["lColor"] = new Vector4(plights[i].LightColor, plights[i].Attenuation);
                pLightShader["lPos"] = plights[i].Position;

                pLightPrim.RenderInfo.World = Matrix4.Scale(plights[i].MaxDistance) * Matrix4.CreateTranslation(plights[i].Position);
                c.Draw(pLightPrim);
            }

            c.DepthFunction = DepthFunc.GEqual;
            c.FaceCulling = CullMode.Front;

            for (int i0 = 0; i0 < redoVolums.Count; i0++)
            {
                int i = redoVolums[i0];
                pLightShader["lColor"] = new Vector4(plights[i].LightColor, plights[i].Attenuation);
                pLightShader["lPos"] = plights[i].Position;

                pLightPrim.RenderInfo.World = Matrix4.Scale(plights[i].MaxDistance) * Matrix4.CreateTranslation(plights[i].Position);
                c.Draw(pLightPrim);
            }

            c.DepthFunction = DepthFunc.LEqual;
            c.Blend = false;
            c.FaceCulling = p;
            c.DepthWrite = true;
            lightBuffer.UnBind(c);

            avgSceneColor.Bind(c);
            avgSceneFSQ.Material.AlbedoMap = g["Color"];
            c.Draw(avgSceneFSQ);
            avgSceneColor.UnBind(c);

            var b0 = bloomPass.ApplyBlur(lightBuffer["Bloom"], c);
            for (int i = 0; i < 3; i++)
            {
                b0 = bloomPass.ApplyBlur(b0, c);
            }

            //Now, blur and blend the shadow map on top of the lighting, then blend in the bloom
            outShader["DiffuseMap"] = g["Color"];
            outShader["LitMap"] = lightBuffer["Lit"];
            outShader["BloomMap"] = lightBuffer["Bloom"];
            outShader["SSRMap"] = ssrBuffer["SSR"];
            outShader["ShadowMap"] = shadowPass.ApplyBlur(g["WorldPos"], c);
            outShader["AvgColor"] = avgSceneColor["AvgColor"];
            c.Draw(outFSQ);

        }

        #region PBR Precalculation
        private static FullScreenQuad preCalcQuad;
        private static ShaderProgram preCalcShader;
        private static FrameBuffer preCalcBuffer;

        public static void Precalculate(GraphicsContext c)
        {
            preCalcQuad = new FullScreenQuad(c);
            preCalcShader = new ShaderProgram(c, VertexShader.Load("PBRPreProcess", c), FragmentShader.Load("PBRPreProcess", c));
            preCalcBuffer = new FrameBuffer(512, 512, c);
            preCalcBuffer.Add("PreCalc", FramebufferTextureSource.Create(preCalcBuffer.Width, preCalcBuffer.Height, 0, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);

            preCalcQuad.RenderInfo.PushShader(preCalcShader);
            preCalcBuffer.Bind(c);
            c.Draw(preCalcQuad);
            preCalcBuffer.UnBind(c);

            preCalcBuffer["PreCalc"].FilterMode = TextureFilter.Linear;
            preCalcBuffer["PreCalc"].WrapX = false;
            preCalcBuffer["PreCalc"].WrapY = false;
        }

        #endregion

    }
}
