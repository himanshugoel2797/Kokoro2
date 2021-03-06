﻿using Kokoro2.Engine.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine;
using Kokoro2.Engine.Prefabs;
using Kokoro2.Engine.Shaders;
using Kokoro2.Engine.HighLevel.Cameras;
using Kokoro2.Math;
using Kokoro2.Engine.HighLevel.Rendering;

namespace Sandbox
{
    public class SphereRenderTest : IScene
    {
        public IScene Parent
        {
            get; set;
        }

        public GBuffer gBuffer;

        public Model TestSphereA;
        public FullScreenQuad FSQ;
        public bool ResourcesLoaded = false;

        public void LoadResources(GraphicsContext context)
        {
            if (!ResourcesLoaded)
            {
                context.Wireframe = true;
                //context.FaceCulling = CullMode.Back;
                context.PatchSize = 3;

                TestSphereA = new Sphere(1, 10, context);
                //TestSphereA.Materials[0].Shader = new ShaderProgram(VertexShader.Load("GBuffer"), FragmentShader.Load("GBuffer"));
                TestSphereA.RenderInfo.PushShader(new ShaderProgram(context, VertexShader.Load("LoD", context), TessellationShader.Load("LoD", "LoD", context), FragmentShader.Load("LoD", context)));
                TestSphereA.DrawMode = DrawMode.Patches;

                gBuffer = new GBuffer(1920, 1080, context);

                FSQ = new FullScreenQuad(context);
                FSQ.RenderInfo.PushShader(new ShaderProgram(context, VertexShader.Load("FrameBuffer", context), FragmentShader.Load("FrameBuffer", context)));
                FSQ.Material.AlbedoMap = gBuffer["RGBA0"];

                context.ZFar = 1000;
                context.ZNear = 0.01f;
                context.Camera = new FirstPersonCamera(context, Vector3.Zero, Vector3.UnitX);

                ResourcesLoaded = true;
            }
        }

        public void Render(double interval, GraphicsContext context)
        {
            if (ResourcesLoaded)
            {
                context.ClearColor(0, 0, 0, 0);

                context.DepthWrite = true;

                gBuffer.Bind(context);

                context.Wireframe = true;

                context.ClearColor(1, 1, 1, 1);
                context.Draw(TestSphereA);

                context.Wireframe = false;

                gBuffer.UnBind(context);

                context.DepthWrite = false;

                context.Draw(FSQ);

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
