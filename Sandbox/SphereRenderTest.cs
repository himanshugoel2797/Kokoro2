using Kokoro2.Engine.SceneGraph;
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

        public Model TestSphereA, Atmosphere;
        public FullScreenQuad FSQ;
        public bool ResourcesLoaded = false;

        public void LoadResources(GraphicsContext context)
        {
            if (!ResourcesLoaded)
            {
                //context.FaceCulling = CullMode.Back;
                context.DepthFunction = (x, y) => (x <= y);
                context.PatchSize = 3;

                TestSphereA = new Box(100, 100, 100);
                //TestSphereA.Materials[0].Shader = new ShaderProgram(VertexShader.Load("Default"), FragmentShader.Load("Default"));
                //TestSphereA.Materials[0].Shader = new ShaderProgram(VertexShader.Load("GBuffer"), FragmentShader.Load("GBuffer"));
                TestSphereA.Materials[0].Shader = new ShaderProgram(VertexShader.Load("LoD"), TessellationShader.Load("Sphere", "Sphere"), FragmentShader.Load("LoD"));
                TestSphereA.DrawMode = DrawMode.Patches;

                Atmosphere = new HighResQuad(0, 0, 100, 100);
                Atmosphere.Materials[0].Shader = new ShaderProgram(VertexShader.Load("GBuffer"), FragmentShader.Load("AtmosphereFromSpace"));


                gBuffer = new GBuffer(1920, 1080, context);
                gBuffer.SetBlendFunc(new BlendFunc() { Src = BlendingFactor.One, Dst = BlendingFactor.Zero });


                FSQ = new FullScreenQuad();
                FSQ.Materials[0].Shader = new ShaderProgram(VertexShader.Load("FrameBuffer"), FragmentShader.Load("FrameBuffer"));
                FSQ.Materials[0].AlbedoMap = gBuffer["RGBA0"];

                context.ZFar = 1e27f;
                context.ZNear = 1e-6f;
                context.Camera = new FirstPersonCamera(context, Vector3.Zero, Vector3.UnitX);

                ResourcesLoaded = true;
            }
        }

        public void Render(double interval, GraphicsContext context)
        {
            if (ResourcesLoaded)
            {

                context.Clear(0, 0, 0, 0);

                gBuffer.Bind(context);

                //context.Wireframe = true;

                context.Clear(0, 0, 0, 0);
                TestSphereA.Draw(context);
                //context.DepthFunction = (x, y) => true;
                //Atmosphere.Draw(context);
                //context.DepthFunction = (x, y) => x <= y;
                gBuffer.UnBind(context);

                context.Wireframe = false;

                FSQ.Draw(context);

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
