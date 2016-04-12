using Kokoro2.Engine.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine;
using Kokoro2.Engine.Prefabs;
using Kokoro2.Math;
using Kokoro2.Engine.HighLevel.Cameras;
using Kokoro2.Engine.Shaders;
using Kokoro2.Engine.HighLevel.Rendering;
using Kokoro2.Engine.HighLevel.Lights;
using System.Drawing;

namespace Kokoro.EssencePainter.Editor
{
    class EditorScene : IScene
    {
        public IScene Parent
        {
            get; set;
        }

        bool meshLoaded = true;
        string meshPath = "";

        bool textureSetLoaded = true;
        TextureSet s;

        VertexMesh mesh;
        LightPass lights;
        GBuffer g;
        DirectionalLight sun;
        PointLight pLight;
        Sphere sphere;
        Bitmap[] bmps = new Bitmap[10];
        Texture[] bmpTexs = new Texture[10];

        bool ResourcesLoaded = false;
        public void LoadResources(GraphicsContext context)
        {
            if (!ResourcesLoaded)
            {
                context.DepthWrite = true;
                context.FaceCulling = CullMode.Off;
                context.DepthFunction = DepthFunc.LEqual;
                context.Camera = new ArcBallCamera(context, Vector3.Zero, Vector3.UnitX)
                {
                    Pannable = true,
                    MoveSpeed = 80f,
                    RotationSpeed = 0.4f,
                    ZoomSpeed = 1f
                };

                //context.Camera = new FirstPersonCamera(context, Vector3.Zero, Vector3.UnitX);
                context.WindowResized += (c) =>
                {
                    context.Camera.SetProjection(Camera.RecommendedFieldOfView, context.WindowSize.X / context.WindowSize.Y, context.ZNear, context.ZFar);
                    lights = new LightPass((int)context.WindowSize.X, (int)context.WindowSize.Y, context);
                    lights.EnvironmentMap = ImageTextureSource.Create("Resources/envMap.jpg", 0, true, context);
                    lights.AddLight(sun);
                    lights.GILight = sun;
                    lights.AddLight(new DirectionalLight(context, Vector3.UnitY));
                    g = new GBuffer((int)context.WindowSize.X, (int)context.WindowSize.Y, context);
                };

                int w = (int)context.WindowSize.X;
                int h = (int)context.WindowSize.Y;

                context.Camera.SetProjection(Camera.RecommendedFieldOfView, (float)w / (float)h, context.ZNear, context.ZFar);
                g = new GBuffer(w, h, context);
                lights = new LightPass(w, h, context);
                lights.EnvironmentMap = ImageTextureSource.Create("Resources/envMap.jpg", 0, true, context);

                sun = new DirectionalLight(context, Vector3.UnitY * -1 + Vector3.UnitX * 0);
                sun.CastShadows = true;
                sun.ShadowResolution = 2048;
                sun.InitializeShadowBuffer(context);
                lights.AddLight(sun);
                lights.GILight = sun;
                lights.AddLight(new DirectionalLight(context, Vector3.UnitY));

                sphere = new Sphere(100, 20, context);
                sphere.Material.AlbedoMap = ImageTextureSource.Create("Resources/envMap.jpg", 0, true, context);
                sphere.RenderInfo.PushShader(new ShaderProgram(context, VertexShader.Load("Shadowed", context), FragmentShader.Load("Shadowed", context)));

                ResourcesLoaded = true;
            }
        }

        public void LoadMesh(string file)
        {
            meshLoaded = false;
            meshPath = file;
        }

        public void LoadTextureSet(TextureSet s)
        {
            this.s = s;
            textureSetLoaded = false;
        }

        int j = 0;
        public void Render(double interval, GraphicsContext context)
        {
            if (ResourcesLoaded)
            {
                context.Camera.Update(interval, context);

                context.ClearDepth();


                if (!meshLoaded && !string.IsNullOrWhiteSpace(meshPath))
                {
                    mesh = new VertexMesh(meshPath, false, context);
                    mesh.RenderInfo.PushShader(new ShaderProgram(context, VertexShader.Load("Shadowed", context), FragmentShader.Load("Shadowed", context)));
                    mesh.Material.AlbedoMap = mesh.Material.RoughnessMap = mesh.Material.SpecularMap = ImageTextureSource.Create("Resources/uv.png", 0, true, context);
                    sun.ShadowBoxSize = mesh.Bound;
                    meshLoaded = true;
                }

                sun.SetupShadowPass(context);
                if (mesh != null)
                {
                    mesh.RenderInfo.PushShader(sun.ShadowShader);
                    context.Draw(mesh);
                    mesh.RenderInfo.PopShader();
                }
                sun.EndShadowPass(context);

                if (mesh != null && !textureSetLoaded)
                {
                    if (mesh.Material.AlbedoMap != null)
                    {
                        mesh.Material.AlbedoMap.Dispose();
                        mesh.Material.AlbedoMap = null;
                    }
                    if (mesh.Material.EmissionMap != null)
                    {
                        mesh.Material.EmissionMap.Dispose();
                        mesh.Material.EmissionMap = null;
                    }
                    if (mesh.Material.SpecularMap != null)
                    {
                        mesh.Material.SpecularMap.Dispose();
                        mesh.Material.SpecularMap = null;
                    }
                    if (mesh.Material.RoughnessMap != null)
                    {
                        mesh.Material.RoughnessMap.Dispose();
                        mesh.Material.RoughnessMap = null;
                    }
                    if (mesh.Material.NormalMap != null)
                    {
                        mesh.Material.NormalMap.Dispose();
                        mesh.Material.NormalMap = null;
                    }

                    if (s.AlbedoMap) mesh.Material.AlbedoMap = ImageTextureSource.Create(s.AlbedoMapFile, 0, true, context);
                    if (s.EmissiveMap) mesh.Material.EmissionMap = ImageTextureSource.Create(s.EmissiveMapFile, 0, true, context);
                    if (s.ReflectivityMap) mesh.Material.SpecularMap = ImageTextureSource.Create(s.ReflectivityMapFile, 0, true, context);
                    if (s.RoughnessMap) mesh.Material.RoughnessMap = ImageTextureSource.Create(s.RoughnessMapFile, 0, true, context);
                    if (s.NormalMap) mesh.Material.NormalMap = ImageTextureSource.Create(s.NormalMapFile, 0, true, context);
                    textureSetLoaded = true;
                }


                g.Bind(context);
                context.ClearColor(0, 0, 0, 0);
                context.ClearDepth();

                context.Draw(sphere);
                if (mesh != null)
                {
                    mesh.Shader["ShadowMap"] = sun.GetShadowMap();
                    mesh.Shader["sWVP"] = sun.ShadowSpace;
                    mesh.Shader["ReflectiveColMap"] = sun.GetColors();
                    mesh.Shader["ReflectivePosMap"] = sun.GetPositions();
                    context.Draw(mesh);
                }

                g.UnBind(context);

                lights.ApplyLights(g, context);
                context.SwapBuffers();
            }
        }

        public void Update(double interval, GraphicsContext context)
        {
            if (ResourcesLoaded)
            {
            }
        }
    }
}
