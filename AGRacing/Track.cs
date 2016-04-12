using Kokoro2.Engine;
using Kokoro2.Engine.HighLevel.Lights;
using Kokoro2.Engine.HighLevel.Rendering;
using Kokoro2.Engine.Physics;
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

        public Track(TrackData data, GraphicsContext context)
        {
            Name = data.Name;
            trackModel = new VertexMesh(data.modelFile, false, context);
            trackModel.Material = Material.Load(data.textureFile, context);
            trackModel.Material.PackedMap = Material.PackTextures(trackModel.Material, context);
            trackModel.RenderInfo.PushShader(new Kokoro2.Engine.Shaders.ShaderProgram(context, VertexShader.Load("ShadowedPacked", context), FragmentShader.Load("ShadowedPacked", context)));
            Reverse = data.Reverse;


            trackPath = VertexMesh.GetVertices(data.path, false);

            float[] verts = VertexMesh.GetVertices(data.collisionMesh, false);
            int[] indices = VertexMesh.GetIndices(data.collisionMesh, false);

            ships = new Ship[8];

            phys = new PhysicsWorld();
            phys.Gravity = new Vector3(0, -30f, 0);

            collisionMesh = new MobileMesh(verts, indices, 1, MobileMesh.Solidity.Clockwise);
            phys.AddEntity(collisionMesh);

            sun = new DirectionalLight(context, -Vector3.UnitY);
            sun.ShadowResolution = 1024;
            sun.CastShadows = true;
            sun.InitializeShadowBuffer(context);
            sun.ShadowBoxSize = new BoundingBox(trackModel.Bound.Min, trackModel.Bound.Max);

            context.WindowResized += (con) =>
            {
                //TODO implement light dispose stuff to close up the memory leak we have here
                lights = new LightPass((int)con.WindowSize.X, (int)con.WindowSize.Y, con);
                lights.AddLight(sun);
                lights.GILight = sun;
                lights.EnvironmentMap = ImageTextureSource.Create("Resources/Proc/Tex/envMap.jpg", 0, true, con);
                gbuf = new GBuffer((int)con.WindowSize.X, (int)con.WindowSize.Y, con);
            };

            lights = new LightPass((int)context.WindowSize.X, (int)context.WindowSize.Y, context);
            lights.AddLight(sun);
            lights.GILight = sun;
            lights.EnvironmentMap = ImageTextureSource.Create("Resources/Proc/Tex/envMap.jpg", 0, true, context);

            gbuf = new GBuffer((int)context.WindowSize.X, (int)context.WindowSize.Y, context);
            fsq = new FullScreenQuad(context);
            fsq.RenderInfo.PushShader(new ShaderProgram(context, VertexShader.Load("FrameBuffer", context), FragmentShader.Load("FrameBuffer", context)));

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

            Vector3 min, max;
            s.GetBounds(out min, out max);
            sun.ShadowBoxSize = new BoundingBox(min, max);

        }

        public void Dispose()
        {
            ((IDisposable)trackModel).Dispose();
        }

        public void Draw(GraphicsContext context)
        {
            context.FaceCulling = CullMode.Off;
            sun.SetupShadowPass(context);
            trackModel.RenderInfo.PushShader(sun.ShadowShader);
            trackModel.DrawMode = DrawMode.Triangles;
            context.Draw(trackModel);
            trackModel.RenderInfo.PopShader();

            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i] != null)
                {
                    ships[i].PushShader(sun.ShadowShader);
                    ships[i].Draw(context);
                    ships[i].PopShader();
                }
            }
            sun.EndShadowPass(context);
            context.FaceCulling = CullMode.Back;

            gbuf.Bind(context);
            context.ClearDepth();

            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i] != null)
                {
                    ships[i].Shader["ShadowMap"] = sun.GetShadowMap();
                    ships[i].Shader["sWVP"] = sun.ShadowSpace;
                    ships[i].Shader["ReflectiveColMap"] = sun.GetColors();
                    ships[i].Shader["ReflectivePosMap"] = sun.GetPositions();
                    ships[i].DrawEffects(context);
                    ships[i].Draw(context);
                }
            }

            trackModel.Shader["ShadowMap"] = sun.GetShadowMap();
            trackModel.Shader["sWVP"] = sun.ShadowSpace;
            trackModel.Shader["ReflectiveColMap"] = sun.GetColors();
            trackModel.Shader["ReflectivePosMap"] = sun.GetPositions();
            context.Draw(trackModel);

            gbuf.UnBind(context);

            lights.ApplyLights(gbuf, context);
        }

        public bool RayCast(Vector3 origin, Vector3 direction, out float distance, out Vector3 normal)
        {
            return collisionMesh.RayCast(origin, direction, out distance, out normal);
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
