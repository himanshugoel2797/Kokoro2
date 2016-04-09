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

        FullScreenQuad fsq2;
        ShaderProgram atmosphereShader;
        FrameBuffer sky;

#if DEBUG
        Model collisionVis;
#endif

        public string Name { get; private set; }

        public Track(string infoLine, GraphicsContext context)
        {
            string[] parts = infoLine.Split(',');
            if (parts.Length != 6) throw new ArgumentException();
            Name = parts[1];
            trackModel = new VertexMesh("Resources/Proc/Track_Vis/" + parts[0] + ".ko", false, context);
            trackModel.Material.AlbedoMap = ImageTextureSource.Create("Resources/Proc/Tex/" + parts[2], 0, true, context);
            trackModel.Material.RoughnessMap = ImageTextureSource.Create("Resources/Proc/Tex/" + parts[3], 0, false, context);
            trackModel.Material.SpecularMap = ImageTextureSource.Create("Resources/Proc/Tex/" + parts[4], 0, false, context);
            trackModel.Material.NormalMap = ImageTextureSource.Create("Resources/Proc/Tex/" + parts[5], 0, false, context);
            trackModel.Material.PackedMap = Material.PackTextures(trackModel.Material, context);
            trackModel.RenderInfo.PushShader(new Kokoro2.Engine.Shaders.ShaderProgram(context, VertexShader.Load("ShadowedPacked", context), FragmentShader.Load("ShadowedPacked", context)));

#if DEBUG
            //collisionVis = new Kokoro2.Engine.Prefabs.Box(100, 100, 100);
            //collisionVis.World = Matrix4.CreateTranslation(225, -55, -50);
            collisionVis = new VertexMesh("Resources/Proc/Track_Path/" + parts[0] + "_col.ko", false, context);

            collisionVis.Material.AlbedoMap = ImageTextureSource.Create("Resources/Proc/Tex/" + parts[2], 0, true, context);
            collisionVis.RenderInfo.PushShader(new Kokoro2.Engine.Shaders.ShaderProgram(context, VertexShader.Load("Default", context), FragmentShader.Load("Default", context)));

#endif


            trackPath = VertexMesh.GetVertices("Resources/Proc/Track_Path/" + parts[0] + "_path.ko", false);

            float[] verts = VertexMesh.GetVertices("Resources/Proc/Track_Path/" + parts[0] + "_col.ko", false);
            int[] indices = VertexMesh.GetIndices("Resources/Proc/Track_Path/" + parts[0] + "_col.ko", false);

            float sc = 1;

            ships = new Ship[8];

            phys = new PhysicsWorld();
            phys.Gravity = new Vector3(0, -30f, 0);

            collisionMesh = new MobileMesh(verts, indices, sc, MobileMesh.Solidity.Clockwise);
            phys.AddEntity(collisionMesh);

            sun = new DirectionalLight(context, -Vector3.UnitY * 0.25f + -Vector3.UnitX * 0.75f);
            sun.ShadowResolution = 1024;
            sun.CastShadows = true;
            sun.InitializeShadowBuffer(context);
            sun.ShadowBoxSize = new BoundingBox(trackModel.Bound.Min, trackModel.Bound.Max);

            context.WindowResized += (con) =>
            {
                //TODO implement light dispose stuff to close up the memory leak we have here
                lights = new LightPass((int)con.WindowSize.X, (int)con.WindowSize.Y, con);
                lights.AddLight(sun);

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

            sky = new FrameBuffer(960, 540, context);
            sky.Add("Color", FramebufferTextureSource.Create(sky.Width, sky.Height, 0, PixelComponentType.RGBA8, PixelType.Float, context), FrameBufferAttachments.ColorAttachment0, context);
            fsq2 = new FullScreenQuad(context);
            atmosphereShader = new ShaderProgram(context, VertexShader.Load("Atmosphere", context), FragmentShader.Load("Atmosphere", context));
            fsq2.RenderInfo.PushShader(atmosphereShader);
            atmosphereShader["uSunPos"] = -sun.Direction;
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
            trackModel.RenderInfo.PushShader(sun.ShadowShader);
            context.Draw(trackModel);
            trackModel.RenderInfo.PopShader();


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
            sun.EndShadowPass(context);
            context.FaceCulling = CullMode.Back;

            //sky.Bind(context);
            //context.Draw(fsq2);
            //sky.UnBind(context);

            gbuf.Bind(context);
            context.ClearColor(0, 0, 0, 0);
            context.ClearDepth();
            trackModel.Shader["ShadowMap"] = sun.GetShadowMap();
            trackModel.Shader["sWVP"] = sun.ShadowSpace;
            trackModel.Shader["ReflectiveColMap"] = sun.GetColors();
            trackModel.Shader["ReflectivePosMap"] = sun.GetPositions();
            context.Draw(trackModel);

            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i] != null)
                {
                    ships[i].Shader["ShadowMap"] = sun.GetShadowMap();
                    ships[i].Shader["sWVP"] = sun.ShadowSpace;
                    ships[i].Shader["ReflectiveColMap"] = sun.GetColors();
                    ships[i].Shader["ReflectivePosMap"] = sun.GetPositions();
                    ships[i].Draw(context);
                }
            }
            context.Draw(fsq2);

            gbuf.UnBind(context);

            lights.ApplyLights(gbuf, context);

#if DEBUG
            context.Wireframe = true;
            //context.Draw(collisionVis);
            context.Wireframe = false;
#endif
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
