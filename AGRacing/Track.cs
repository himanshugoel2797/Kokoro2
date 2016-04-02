using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.Entities.Prefabs;
using Kokoro2.Engine;
using Kokoro2.Engine.HighLevel.Lights;
using Kokoro2.Engine.HighLevel.Rendering;
using Kokoro2.Engine.Prefabs;
using Kokoro2.Engine.Shaders;
using Kokoro2.Math;
using Kokoro2.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGRacing
{
    class Track : IDisposable
    {

        public bool Reverse { get; set; }

        MobileMesh collisionMesh;
        Model trackModel;
        float[] trackPath;
        PhysicsWorld phys;
        Ship[] ships;

        GBuffer gbuf;
        FullScreenQuad fsq;
        DirectionalLight sun;
        LightPass lights;

#if DEBUG
        Model collisionVis;
#endif

        public string Name { get; private set; }

        public Track(string infoLine, GraphicsContext context)
        {
            string[] parts = infoLine.Split(',');
            if (parts.Length != 4) throw new ArgumentException();
            Name = parts[1];
            trackModel = new VertexMesh("Resources/Proc/Track_Vis/" + parts[0] + ".ko", false);
            trackModel.AlbedoMap = new Texture("Resources/Proc/Tex/" + parts[2], true, context);
            trackModel.Materials[0].GlossinessMap = new Texture("Resources/Proc/Tex/" + parts[3], false, context);
            trackModel.PushShader(new Kokoro2.Engine.Shaders.ShaderProgram(context, VertexShader.Load("Shadowed", context), FragmentShader.Load("Shadowed", context)));

#if DEBUG
            //collisionVis = new Kokoro2.Engine.Prefabs.Box(100, 100, 100);
            //collisionVis.World = Matrix4.CreateTranslation(225, -55, -50);
            collisionVis = new VertexMesh("Resources/Proc/Track_Vis/" + parts[0] + ".ko", false);

            collisionVis.AlbedoMap = new Texture("Resources/Proc/Tex/" + parts[2], true, context);
            collisionVis.PushShader(new Kokoro2.Engine.Shaders.ShaderProgram(context, VertexShader.Load("Default", context), FragmentShader.Load("Default", context)));

#endif


            trackPath = VertexMesh.GetVertices("Resources/Proc/Track_Path/" + parts[0] + "_path.ko", false);

            float[] verts = VertexMesh.GetVertices("Resources/Proc/Track_Path/" + parts[0] + "_col.ko", false);
            int[] indices = VertexMesh.GetIndices("Resources/Proc/Track_Path/" + parts[0] + "_col.ko", false);

            float sc = 1;

            List<BEPUutilities.Vector3> v = new List<BEPUutilities.Vector3>();
            for (int i = 0; i < verts.Length; i += 3)
            {
                v.Add(new BEPUutilities.Vector3(verts[i] * sc, verts[i + 1] * sc, verts[i + 2] * sc));
            }

            ships = new Ship[8];

            phys = new PhysicsWorld();
            collisionMesh = new MobileMesh(v.ToArray(), indices, BEPUutilities.AffineTransform.Identity, BEPUphysics.CollisionShapes.MobileMeshSolidity.Clockwise);
            //collisionMesh.ImproveBoundaryBehavior = true;
            phys.AddEntity(collisionMesh);
            collisionMesh.CollisionInformation.Tag = collisionMesh.Tag;
            phys.Gravity = new Vector3(0, -30f, 0);

            //BEPUphysics.Entities.Prefabs.Box b = new BEPUphysics.Entities.Prefabs.Box(-Vector3.UnitY * 55 + -Vector3.UnitZ * 50 + Vector3.UnitX * 225, 100, 100, 100);
            //phys.AddEntity(b);
            sun = new DirectionalLight(context, -Vector3.UnitY * 1.0f + -Vector3.UnitX * 0f);
            sun.ShadowResolution = 2048;
            sun.CastShadows = true;
            sun.InitializeShadowBuffer(context);
            sun.ShadowBoxSize = new BoundingBox(trackModel.Bound.Min, trackModel.Bound.Max);
            //sun.ShadowBoxLocation = new Vector3(0, 0, 0);
            //sun.ShadowBoxSize = new BoundingBox(new Vector3(-50, -50, -5), new Vector3(50, 50, 200));

            //blur = new TextureBlurFilter((int)context.WindowSize.X, (int)context.WindowSize.Y, PixelComponentType.RGBA8, context);
            context.WindowResized += (con) =>
            {
                lights = new LightPass((int)con.WindowSize.X, (int)con.WindowSize.Y, con);
                lights.AddLight(sun);
                gbuf = new GBuffer((int)con.WindowSize.X, (int)con.WindowSize.Y, con);
            };

            lights = new LightPass((int)context.WindowSize.X, (int)context.WindowSize.Y, context);
            lights.AddLight(sun);

            gbuf = new GBuffer((int)context.WindowSize.X, (int)context.WindowSize.Y, context);
            fsq = new FullScreenQuad();
            fsq.Shader = new ShaderProgram(context, VertexShader.Load("FrameBuffer", context), FragmentShader.Load("FrameBuffer", context));
        }

        #region Ideal Race Line Controls 
        public Vector3 GetPosition(int b)
        {
            if (b < 0) b = trackPath.Length / 3 - b;
            if (b % 2 == 1) b++;
            b = b % (trackPath.Length / 3);

            if (Reverse) b = (trackPath.Length / 3 - 1) - b;

            b *= 3;
            return new Vector3(trackPath[b + 0], trackPath[b + 1], trackPath[b + 2]);
        }

        public int GetPointCount()
        {
            return trackPath.Length / 3;
        }

        public Vector3 GetStartPosition()
        {
            return GetPosition(0);
        }

        public Vector3 GetStartDirection()
        {
            return GetDirection(0);
        }

        public Vector3 GetDirection(int b)
        {
            if (b % 2 == 1) b++;
            return Vector3.Normalize(GetPosition(b + 1) - GetPosition(b));
        }
        #endregion

        public void AddShip(int position, Ship s)
        {
            if (position >= ships.Length) throw new ArgumentException();
            s.Position = GetPosition((position) / 2) + Vector3.UnitY * 25;
            s.PhysicalFront = GetDirection((position) / 2);

            Vector3 right = Vector3.Cross(s.PhysicalFront, Vector3.UnitY);
            right.Normalize();

            if (position % 2 == 0)
            {
                s.Position += right * 2;
            }
            else if (position % 2 == 1)
            {
                s.Position -= right * 2;
            }
            ships[position] = s;

            var t = s.GetPhysicsEntity();
            t.Position = s.Position;
            phys.AddEntity(t);
            t.CollisionInformation.Tag = t.Tag;

            Vector3 min, max;
            s.GetBounds(out min, out max);
            //sun.ShadowBoxSize = new BoundingBox(min, max);

        }

        public void Dispose()
        {
            ((IDisposable)trackModel).Dispose();
        }

        bool sceneShaddowMapPass = false;
        public void Draw(GraphicsContext context)
        {

            sceneShaddowMapPass = true;
            context.FaceCulling = CullMode.Off;
            sun.SetupShadowPass(context);
            //context.Wireframe = true;
            trackModel.PushShader(sun.ShadowShader);
            trackModel.Draw(context);
            trackModel.PopShader();
            sun.EndShadowPass(context);
            context.FaceCulling = CullMode.Back;


            //context.Wireframe = false;
            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i] != null)
                {
                    ships[i].PushShader(sun.ShadowShader);
                    ships[i].Draw(context);
                    ships[i].PopShader();
                }
            }

            gbuf.Bind(context);
            context.Clear(0, 0, 0, 0);
            trackModel.Shader["ShadowMap"] = sun.GetShadowMap();
            trackModel.Shader["sWVP"] = sun.ShadowSpace;
            trackModel.Shader["ReflectiveNormMap"] = sun.GetNormals();
            trackModel.Shader["ReflectivePosMap"] = sun.GetPositions();
            trackModel.Draw(context);

            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i] != null)
                {
                    ships[i].Shader["ShadowMap"] = sun.GetShadowMap();
                    ships[i].Shader["sWVP"] = sun.ShadowSpace;
                    ships[i].Shader["ReflectiveNormMap"] = sun.GetNormals();
                    ships[i].Shader["ReflectivePosMap"] = sun.GetPositions();
                    ships[i].Draw(context);
                }
            }
            gbuf.UnBind(context);

            lights.ApplyLights(gbuf, context);

            //fsq.AlbedoMap = blur.ApplyBlur(gbuf["Shadow"], context);
            //fsq.Draw(context);
#if DEBUG
            context.Wireframe = true;
            //collisionVis.Draw(context);
            context.Wireframe = false;
#endif
        }

        public bool RayCast(Vector3 origin, Vector3 direction, out float distance, out Vector3 normal)
        {
            BroadPhaseEntry[] e;
            float[] dists;
            Vector3[] norms;

            bool res = phys.RayCast(origin, direction, out dists, out norms, out e);

            distance = 0;
            normal = -direction;

            //Console.WriteLine(sun.ShadowBoxLocation);

            for (int i = 0; i < e.Length; i++)
            {
                if ((int)e[i].Tag == (int)collisionMesh.Tag)
                {
                    distance = dists[i];
                    normal = norms[i];
                    return (int)e[i].Tag == (int)collisionMesh.Tag;
                }
            }
            return false;
        }

        public void Update(double interval, GraphicsContext context)
        {
            sun.ShadowBoxLocation = context.Camera.Position;
            phys.Update(interval / 1000d);

            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i] != null) ships[i].Update(interval, context, this);
            }
        }
    }
}
