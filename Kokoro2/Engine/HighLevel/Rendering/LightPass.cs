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

        private TextureBlurFilter bloomPass, shadowPass, ssrPass;
        private int id = 0;

        private FrameBuffer avgSceneColor;

        private ShaderProgram avgSceneShader;
        private FullScreenQuad avgSceneFSQ;

        private FullScreenQuad giLightPrim;
        private ShaderProgram giShader;
        private FrameBuffer giBuffer;

        FullScreenQuad fsq2, skyAddFSQ;
        ShaderProgram atmosphereShader, skyFSQ, skyFSQGbuffer;
        FrameBuffer sky;



        public Texture EnvironmentMap { get; set; }
        public DirectionalLight GILight { get; set; }   //The light that sources the global illumination data

        public LightPass(int width, int height, GraphicsContext c)
        {
            outShader = new ShaderProgram(c, VertexShader.Load("LightShadowBloom", c), FragmentShader.Load("LightShadowBloom", c));
            avgSceneShader = new ShaderProgram(c, VertexShader.Load("FrameBuffer", c), FragmentShader.Load("FrameBuffer", c));
            dLightShader = new ShaderProgram(c, VertexShader.Load("DirectionalLight", c), FragmentShader.Load("DirectionalLight", c));
            pLightShader = new ShaderProgram(c, VertexShader.Load("PointLight", c), FragmentShader.Load("PointLight", c));
            giShader = new ShaderProgram(c, VertexShader.Load("SSGI", c), FragmentShader.Load("SSGI", c));
            ssrShader = new ShaderProgram(c, VertexShader.Load("SSR", c), FragmentShader.Load("SSR", c));

            ssrFSQ = new FullScreenQuad(c);
            ssrFSQ.RenderInfo.PushShader(ssrShader);

            outFSQ = new FullScreenQuad(c);
            outFSQ.RenderInfo.PushShader(outShader);

            pLightPrim = new Sphere(1, 20, c);
            pLightPrim.RenderInfo.PushShader(pLightShader);

            dLightPrim = new FullScreenQuad(c);
            dLightPrim.RenderInfo.PushShader(dLightShader);

            giLightPrim = new FullScreenQuad(c);
            giLightPrim.RenderInfo.PushShader(giShader);

            dlights = new List<DirectionalLight>();
            plights = new List<PointLight>();
            idMap = new Dictionary<int, Tuple<int, int>>();

            giBuffer = new FrameBuffer(width / 2, height / 2, c);
            giBuffer.Add("GI", FramebufferTextureSource.Create(giBuffer.Width, giBuffer.Height, 0, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);

            lightBuffer = new FrameBuffer(width, height, c);
            lightBuffer.Add("Lit", FramebufferTextureSource.Create(width, height, 0, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);

            ssrBuffer = new FrameBuffer(width, height, c);
            var ssrTex = FramebufferTextureSource.Create(ssrBuffer.Width, ssrBuffer.Height, -1, PixelComponentType.RGBA16f, PixelType.Float, c);
            ssrTex.FilterMode = TextureFilter.Linear;
            ssrBuffer.Add("SSR", ssrTex, FrameBufferAttachments.ColorAttachment0, c);
            ssrPass = new TextureBlurFilter(ssrBuffer.Width * 2, ssrBuffer.Height * 2, PixelComponentType.RGBA16f, c);

            bloomPass = new TextureBlurFilter(width, height, PixelComponentType.RGBA16f, c);
            bloomPass.BlurRadius = 0.0015f * 960 / width;
            shadowPass = new TextureBlurFilter(width / 2, height / 2, PixelComponentType.RGBA8, c);
            shadowPass.BlurRadius = 0.0025f * 960 / width;

            //Precalculate PBR data
            sky = new FrameBuffer(960, 540, c);
            sky.Add("Color", FramebufferTextureSource.Create(sky.Width, sky.Height, 0, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);
            fsq2 = new FullScreenQuad(c);
            atmosphereShader = new ShaderProgram(c, VertexShader.Load("Atmosphere", c), FragmentShader.Load("Atmosphere", c));
            fsq2.RenderInfo.PushShader(atmosphereShader);

            skyAddFSQ = new FullScreenQuad(c);
            skyFSQ = new ShaderProgram(c, VertexShader.Load("FrameBuffer", c), FragmentShader.Load("FrameBuffer", c));
            skyFSQGbuffer = new ShaderProgram(c, VertexShader.Load("FrameBufferToG", c), FragmentShader.Load("FrameBufferToG", c));
            skyAddFSQ.RenderInfo.PushShader(skyFSQ);
            skyAddFSQ.Material.AlbedoMap = sky["Color"];
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
            sky.Bind(c);
            atmosphereShader["uSunPos"] = -GILight.Direction;
            c.Draw(fsq2);
            sky.UnBind(c);

            g.Bind(c);
            skyAddFSQ.RenderInfo.PushShader(skyFSQGbuffer);
            c.Draw(skyAddFSQ);
            skyAddFSQ.RenderInfo.PopShader();
            g.UnBind(c);


            var b1 = bloomPass.ApplyBlur(g["Bloom"], c);
            for (int i = 0; i < 2; i++)
            {
                b1 = bloomPass.ApplyBlur(b1, c);
            }

            if (preCalcQuad == null) Precalculate(c);
            ssrShader["worldData"] = g["WorldPos"];
            ssrShader["normData"] = g["Normal"];
            ssrShader["depthMap"] = g["DepthBuffer"];
            ssrShader["colorMap"] = g["Color"];
            ssrShader["bloomMap"] = b1;
            g["Color"].WrapX = false;
            g["Color"].WrapY = false;
            g["Normal"].WrapX = false;
            g["Normal"].WrapY = false;
            g["Color"].FilterMode = TextureFilter.Nearest;
            g["Normal"].FilterMode = TextureFilter.Nearest;
            g["WorldPos"].FilterMode = TextureFilter.Nearest;
            b1.WrapX = false;
            b1.WrapY = false;

            ssrBuffer.Bind(c);
            c.Draw(ssrFSQ);
            ssrBuffer.UnBind(c);

            var ssrBlurred = ssrPass.ApplyBlur(ssrBuffer["SSR"], c);

            ssrBuffer["SSR"].UpdateMipMaps();
            ssrBuffer["SSR"].FilterMode = TextureFilter.Linear;

            g["WorldPos"].WrapX = false;
            g["WorldPos"].WrapY = false;

            g["DepthBuffer"].WrapX = false;
            g["DepthBuffer"].WrapY = false;

            giShader["worldData"] = g["WorldPos"];
            giShader["normData"] = g["Normal"];
            giShader["depthMap"] = g["DepthBuffer"];
            giShader["colorMap"] = g["Color"];
            giShader["giLightDirection"] = GILight.Direction;
            giShader["sVP"] = GILight.ShadowSpace;
            giShader["shadowDepth"] = GILight.GetShadowMap();
            giShader["bloomMap"] = b1;

            giBuffer.Bind(c);
            c.Draw(giLightPrim);
            giBuffer.UnBind(c);

            lightBuffer.Add("DepthBuffer", g["DepthBuffer"], FrameBufferAttachments.DepthAttachment, c);
            lightBuffer.Bind(c);

            c.DepthWrite = false;

            c.ClearColor(0, 0, 0, 0);

            c.Blending = new BlendFunc()
            {
                Src = BlendingFactor.One,
                Dst = BlendingFactor.One
            };
            c.DepthFunction = DepthFunc.Always;

            c.Blend = true;


            dLightShader["colorMap"] = g["Color"];
            dLightShader["normData"] = g["Normal"];
            dLightShader["worldData"] = g["WorldPos"];
            dLightShader["envMap"] = EnvironmentMap;
            dLightShader["ssrMap"] = ssrBuffer["SSR"];
            dLightShader["preCalc"] = preCalcBuffer["PreCalc"];
            dLightShader["depthBuffer"] = g["DepthBuffer"];

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

            var b0 = shadowPass.ApplyBlur(g["WorldPos"], c);
            for (int i = 0; i < 0; i++)
            {
                b0 = shadowPass.ApplyBlur(b0, c);
            }

            lightBuffer["Lit"].FilterMode = TextureFilter.Linear;


            //Now, blur and blend the shadow map on top of the lighting, then blend in the bloom
            outShader["DiffuseMap"] = g["Normal"];
            outShader["LitMap"] = lightBuffer["Lit"];
            outShader["BloomMap"] = b1;
            outShader["ShadowMap"] = b0;
            outShader["depthBuffer"] = g["DepthBuffer"];
            outShader["giBuffer"] = giBuffer["GI"];
            c.Draw(outFSQ);
            c.Draw(skyAddFSQ);
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
