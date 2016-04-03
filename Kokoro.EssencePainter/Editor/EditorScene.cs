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
                    lights.EnvironmentMap = new Texture("Resources/envMap.jpg", true, context);
                    g = new GBuffer((int)context.WindowSize.X, (int)context.WindowSize.Y, context);
                };


                context.Camera.SetProjection(Camera.RecommendedFieldOfView, context.WindowSize.X / context.WindowSize.Y, context.ZNear, context.ZFar);
                lights = new LightPass((int)context.WindowSize.X, (int)context.WindowSize.Y, context);
                lights.EnvironmentMap = new Texture("Resources/envMap.jpg", true, context);
                g = new GBuffer((int)context.WindowSize.X, (int)context.WindowSize.Y, context);

                sun = new DirectionalLight(context, -Vector3.UnitY * 0.25f + -Vector3.UnitX * 1f);
                sun.CastShadows = true;
                sun.ShadowResolution = 2048;
                sun.InitializeShadowBuffer(context);

                q = new Sphere(100, 10, context);
                q.RenderInfo.PushShader(new ShaderProgram(context, VertexShader.Load("Shadowed", context), FragmentShader.Load("Shadowed", context)));
                q.Material.AlbedoMap = q.Material.GlossinessMap = q.Material.SpecularMap = new Texture("Resources/uv.png", true, context);

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
                    mesh.Material.AlbedoMap = mesh.Material.GlossinessMap = mesh.Material.SpecularMap = new Texture("Resources/uv.png", true, context);
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

                g.Bind(context);
                context.ClearColor(0, 0, 0, 0);
                context.ClearDepth();

                context.Draw(q);

                if (mesh != null)
                {
                    mesh.Shader["ShadowMap"] = sun.GetShadowMap();
                    mesh.Shader["sWVP"] = sun.ShadowSpace;
                    mesh.Shader["ReflectiveNormMap"] = sun.GetNormals();
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
                context.Camera.Update(interval, context);
            }
        }
    }
}
