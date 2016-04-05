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

            lightBuffer = new FrameBuffer(width, height, PixelComponentType.D32, c, false);
            lightBuffer.Add("Lit", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);
            lightBuffer.Add("Bloom", new FrameBufferTexture(width, height, PixelFormat.BGRA, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment1, c);

            bloomPass = new TextureBlurFilter(width, height, PixelComponentType.RGBA16f, c);
            bloomPass.BlurRadius = 0.0015f * 960 / width;
            shadowPass = new TextureBlurFilter(width, height, PixelComponentType.RGBA8, c);
            shadowPass.BlurRadius = 0.0025f * 960 / width;

            avgSceneFSQ = new FullScreenQuad(c);
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
            lightBuffer.Add("DepthBuffer", (FrameBufferTexture)g["DepthBuffer"], FrameBufferAttachments.DepthAttachment, c);
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
            dLightShader["specularData"] = g["Specular"];
            dLightShader["worldData"] = g["WorldPos"];
            dLightShader["envMap"] = EnvironmentMap;

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

            //Perform the RSM pass
            giShader["ShadowMap"] = GILight.GetShadowMap();
            giShader["sWVP"] = GILight.ShadowSpace;
            giShader["ReflectiveColMap"] = GILight.GetColors();
            giShader["ReflectivePosMap"] = GILight.GetPositions();
            //c.DrawInstanced(giLightPrim, giLightInstanceData, giLightInstanceData.Length / 4);


            pLightShader["colorMap"] = g["Color"];
            pLightShader["normData"] = g["Normal"];
            pLightShader["specularData"] = g["Specular"];
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

            for(int i0 = 0; i0 < redoVolums.Count; i0++)
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
            //outShader["BloomMap"] = bloomPass.ApplyBlur(b0, c);
            outShader["ShadowMap"] = shadowPass.ApplyBlur(g["Shadow"], c);
            outShader["AvgColor"] = avgSceneColor["AvgColor"];
            c.Draw(outFSQ);

        }

    }
}
