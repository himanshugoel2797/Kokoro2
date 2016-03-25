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

        private bool ResourcesLoaded = false;
        public void LoadResources(GraphicsContext context)
        {
            if (!ResourcesLoaded)
            {
                track = ResourceLoader.LoadTrack("Test Track");
                s1 = ResourceLoader.LoadShip("Fiel F35", new HumanController());

                track.AddShip(0, s1);

                context.DepthWrite = true;
                context.FaceCulling = CullMode.Back;
                context.DepthFunction = DepthFunc.LEqual;
                context.ZFar = 1000;
                context.ZNear = 0.1f;
                context.Camera = new FirstPersonCamera(context, Vector3.Zero, Vector3.UnitX);
                //context.Camera = new FollowPointCamera(context, Vector3.Zero, Vector3.UnitX);
                //context.Wireframe = true;

                ResourcesLoaded = true;
            }
        }

        public void Render(double interval, GraphicsContext context)
        {
            if (ResourcesLoaded)
            {
                context.Clear(0, 0.5f, 1.0f, 0);


                //Vector3 dir = track.GetDirection(cnt);
                //Vector3 pos = (1.0f - cnt3) * track.GetPosition(cnt) + (cnt3) * track.GetPosition(cnt + 1);
                //cnt3 += 0.1f;

                //Matrix4 rot = new Matrix4(new Vector4(dir, 0), new Vector4(Vector3.UnitY, 0), new Vector4(Vector3.Normalize(Vector3.Cross(dir, Vector3.UnitY)), 0), Vector4.UnitW);
                //car.World = Matrix4.CreateRotationY(-1.57f) * rot * Matrix4.CreateTranslation(pos);

                //if (cnt3 >= 1.0f)
                //{
                //    cnt += 2;
                //    cnt3 = 0;
                //}

                var tmpCam = context.Camera as FollowPointCamera;

                if (tmpCam != null)
                {
                    tmpCam.Position = s1.Position - s1.Direction * 4 + Vector3.UnitY * 1.5f;

                    tmpCam.Direction = -(tmpCam.Position - s1.Position);
                    tmpCam.Up = Vector3.UnitY;
                    context.Camera = tmpCam;
                }

                track.Draw(context);

                context.SwapBuffers();
            }
        }

        int cnt = 0;
        float cnt3 = 0;
        public void Update(double interval, GraphicsContext context)
        {
            if (ResourcesLoaded)
            {
                //Console.WriteLine(interval);
                track.Update(interval, context);
                context.Camera.Update(interval, context);
            }
        }
    }
}
