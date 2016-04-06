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
        VertexMesh mesh;
        LightPass lights;
        GBuffer g;
        DirectionalLight sun;
        PointLight pLight;
        Model q;

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
                    Random rng = new Random();
                    for (int i = 0; i < 500; i++)
                    {
                        float x = rng.Next(0, 15) / 15f;
                        float y = rng.Next(0, 25) / 25f;
                        float z = rng.Next(0, 5) / 5f;

                        var pLight = new PointLight(context);
                        pLight.LightColor = new Vector3(x, y, z);
                        pLight.Attenuation = 0.1f;
                        pLight.Position = new Vector3(rng.Next(-200, 200), rng.Next(-100, 100), rng.Next(-200, 200));
                        lights.AddLight(pLight);
                    }
                    g = new GBuffer((int)context.WindowSize.X, (int)context.WindowSize.Y, context);
                };


                context.Camera.SetProjection(Camera.RecommendedFieldOfView, context.WindowSize.X / context.WindowSize.Y, context.ZNear, context.ZFar);
                lights = new LightPass((int)context.WindowSize.X, (int)context.WindowSize.Y, context);
                lights.EnvironmentMap = ImageTextureSource.Create("Resources/envMap.jpg", 0, true, context);
                g = new GBuffer((int)context.WindowSize.X, (int)context.WindowSize.Y, context);

                sun = new DirectionalLight(context, -Vector3.UnitY * 0.75f + Vector3.UnitX * 0.25f);
                sun.CastShadows = true;
                sun.ShadowResolution = 2048;
                sun.InitializeShadowBuffer(context);
                lights.AddLight(sun);
                lights.GILight = sun;

                {
                    Random rng = new Random();
                    for (int i = 0; i < 500; i++)
                    {
                        float x = rng.Next(0, 15) / 15f;
                        float y = rng.Next(0, 25) / 25f;
                        float z = rng.Next(0, 5) / 5f;

                        var pLight = new PointLight(context);
                        pLight.LightColor = new Vector3(x, y, z);
                        pLight.Attenuation = 0.1f;
                        pLight.Position = new Vector3(rng.Next(-200, 200), rng.Next(-100, 100), rng.Next(-200, 200));
                        lights.AddLight(pLight);
                    }
                }

                q = new Sphere(10, 20, context);
                q.RenderInfo.PushShader(new ShaderProgram(context, VertexShader.Load("Shadowed", context), FragmentShader.Load("Shadowed", context)));
                q.Material.AlbedoMap = q.Material.GlossinessMap = q.Material.SpecularMap = ImageTextureSource.Create("Resources/envMap.jpg", 0, true, context);

                ResourcesLoaded = true;
            }
        }

        public void LoadMesh(string file)
        {
            meshLoaded = false;
            meshPath = file;
        }

        public void Render(double interval, GraphicsContext context)
        {
            if (ResourcesLoaded)
            {
                context.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
                context.ClearDepth();


                if (!meshLoaded && !string.IsNullOrWhiteSpace(meshPath))
                {
                    mesh = new VertexMesh(meshPath, false, context);
                    mesh.RenderInfo.PushShader(new ShaderProgram(context, VertexShader.Load("Shadowed", context), FragmentShader.Load("Shadowed", context)));
                    mesh.Material.AlbedoMap = mesh.Material.GlossinessMap = mesh.Material.SpecularMap = ImageTextureSource.Create("Resources/uv.png", 0, true, context);
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

                q.RenderInfo.PushShader(sun.ShadowShader);
                context.Draw(q);
                q.RenderInfo.PopShader();

                sun.EndShadowPass(context);

                g.Bind(context);
                context.ClearColor(0, 0, 0, 0);
                context.ClearDepth();


                if (mesh != null)
                {
                    mesh.Shader["ShadowMap"] = sun.GetShadowMap();
                    mesh.Shader["sWVP"] = sun.ShadowSpace;
                    mesh.Shader["ReflectiveColMap"] = sun.GetColors();
                    mesh.Shader["ReflectivePosMap"] = sun.GetPositions();
                    context.Draw(mesh);
                }
                q.Shader["ShadowMap"] = sun.GetShadowMap();
                q.Shader["sWVP"] = sun.ShadowSpace;
                q.Shader["ReflectiveColMap"] = sun.GetColors();
                q.Shader["ReflectivePosMap"] = sun.GetPositions();
                context.Draw(q);

                g.UnBind(context);

                lights.ApplyLights(g, context);

                context.SwapBuffers();
            }
        }

        public void Update(double interval, GraphicsContext context)
        {
            if (ResourcesLoaded)
            {
                context.Camera.Update(interval, context);
            }
        }
    }
}
