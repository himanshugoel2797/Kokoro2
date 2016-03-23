using Kokoro2.Engine.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine;
using Kokoro2.Engine.HighLevel.Cameras;
using Kokoro2.Math;
using Kokoro2.Engine.Shaders;

namespace AGRacing.Test.TrackLoadTest
{
    class Test : IScene
    {
        public IScene Parent
        {
            get;
            set;
        }

        Kokoro2.Engine.Prefabs.VertexMesh track;

        private bool ResourcesLoaded = false;
        public void LoadResources(GraphicsContext context)
        {
            if(!ResourcesLoaded)
            {
                track = new Kokoro2.Engine.Prefabs.VertexMesh("Resources/Proc/Track_Vis/car1_0.ko", false, false);

                for(int i = 0; i < track.Materials.Length; i++)
                    track.Materials[i].Shader = new Kokoro2.Engine.Shaders.ShaderProgram(VertexShader.Load("Default"), FragmentShader.Load("Default"));

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
                context.Clear(0, 0.5f, 1.0f, 0);
                context.DepthWrite = true;
                context.Wireframe = true;

                track.Draw(context);

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
