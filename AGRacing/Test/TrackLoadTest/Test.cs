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
using Kokoro2.Physics;
using AGRacing.ShipControllers;
using System.Diagnostics;
using Kokoro2.Engine.Prefabs;

namespace AGRacing.Test.TrackLoadTest
{
    class Test : IScene
    {
        public IScene Parent
        {
            get;
            set;
        }

        Ship s1;
        Track track;

        Sphere sp;

        private bool ResourcesLoaded = false;
        public void LoadResources(GraphicsContext context)
        {
            if (!ResourcesLoaded)
            {
                track = ResourceLoader.LoadTrack("Test Track2", context);
                s1 = ResourceLoader.LoadShip("Fiel F35", new HumanController());

                track.Reverse = true;
                track.AddShip(0, s1);

                sp = new Sphere(10, 10);
                sp.Shader = new ShaderProgram(VertexShader.Load("Default"), FragmentShader.Load("Default"));
                sp.AlbedoMap = new Texture("Resources/Proc/Tex/track_tex.png", false);
                sp.World = Matrix4.CreateTranslation(Vector3.UnitY * -0.75f + Vector3.UnitX * -0.25f);

                context.DepthWrite = true;
                context.FaceCulling = CullMode.Back;
                context.DepthFunction = DepthFunc.LEqual;
                //context.DepthClamp = true;
                context.ZFar = 1000;
                context.ZNear = 0.1f;
                context.Camera = new FirstPersonCamera(context, Vector3.Zero, Vector3.UnitX);
                context.Camera = new FollowPointCamera(context, Vector3.Zero, Vector3.UnitX);
                //context.Wireframe = true;

                if(context.Camera as FirstPersonCamera != null)
                {
                    context.Camera.Position = track.GetPosition(0);
                }

                ResourcesLoaded = true;
            }
        }

        public void Render(double interval, GraphicsContext context)
        {
            if (ResourcesLoaded)
            {
                context.Clear(0, 0.5f, 1.0f, 0);
                
                track.Draw(context);
                /*context.DepthFunction = DepthFunc.Always;
                sp.World = Matrix4.CreateTranslation(Vector3.UnitY * -75f + Vector3.UnitX * -25f);
                sp.Draw(context);
                sp.World = Matrix4.Identity;
                sp.Draw(context);
                context.DepthFunction = DepthFunc.LEqual;
                */
                context.SwapBuffers();
            }
        }

        int cnt = 0;
        float cnt3 = 0;
        public void Update(double interval, GraphicsContext context)
        {
            if (ResourcesLoaded)
            {
                track.Update(interval, context);
                var tmpCam = context.Camera as FollowPointCamera;

                if (tmpCam != null)
                {
                    int index = s1.findNearestTrackPoint(track);

                    float camDist = 50 - (float)Math.Min(50, Math.Pow(Vector3.Dot(s1.PhysicalFront, s1.Velocity) / (Vector3.Dot(s1.PhysicalFront, s1.MovementDirection) * 100), 4));
                    float turningFactor = (1 - Vector3.Dot(s1.MovementDirection, s1.PhysicalFront));

                    camDist /= 10;
                    camDist -= turningFactor * 2;
                    if (camDist > 5) camDist = 5;
                    if (camDist < 2) camDist = 2;
                    camDist *= 1.5f;

                    tmpCam.Position = s1.Position - (s1.MovementDirection + s1.PhysicalFront) / 2 * camDist;
                    tmpCam.Position += Vector3.UnitY * 1.5f;
                    tmpCam.Direction = -(tmpCam.Position - s1.Position) + s1.PhysicalFront * 5;
                    tmpCam.Up = s1.prevUp;
                    tmpCam.Position += Vector3.UnitY * 1.0f;

                    context.Camera = tmpCam;
                }
                context.Camera.Update(interval, context);
            }
        }
    }
}
