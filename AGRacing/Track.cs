using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.Entities.Prefabs;
using Kokoro2.Engine;
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
        MobileMesh collisionMesh;
        Model trackModel;
        float[] trackPath;
        PhysicsWorld phys;
        Ship[] ships;

#if DEBUG
        Model collisionVis;
#endif

        public string Name { get; private set; }

        public Track(string infoLine)
        {
            string[] parts = infoLine.Split(',');
            if (parts.Length != 3) throw new ArgumentException();
            Name = parts[1];
            trackModel = new VertexMesh("Resources/Proc/Track_Vis/" + parts[0] + ".ko", false);

#if DEBUG
            //collisionVis = new Kokoro2.Engine.Prefabs.Box(100, 100, 100);
            //collisionVis.World = Matrix4.CreateTranslation(225, -55, -50);
            collisionVis = new VertexMesh("Resources/Proc/Track_Vis/" + parts[0] + ".ko", false);
            for (int i = 0; i < collisionVis.Materials.Length; i++)
            {
                collisionVis.Materials[i].AlbedoMap = new Texture("Resources/Proc/Tex/" + parts[2]);
                collisionVis.Materials[i].Shader = new Kokoro2.Engine.Shaders.ShaderProgram(VertexShader.Load("Default"), FragmentShader.Load("Default"));
            }
#endif

            for (int i = 0; i < trackModel.Materials.Length; i++)
            {
                trackModel.Materials[i].AlbedoMap = new Texture("Resources/Proc/Tex/" + parts[2]);
                trackModel.Materials[i].Shader = new Kokoro2.Engine.Shaders.ShaderProgram(VertexShader.Load("Default"), FragmentShader.Load("Default"));
            }

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

        }

        #region Ideal Race Line Controls 
        public Vector3 GetPosition(int b)
        {
            if (b < 0) b = trackPath.Length / 3 - b;
            if (b % 2 == 1) b++;
            b = b % (trackPath.Length / 3);

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
        }

        public void Dispose()
        {
            ((IDisposable)trackModel).Dispose();
        }

        public void Draw(GraphicsContext context)
        {
            trackModel.Draw(context);

#if DEBUG
            context.Wireframe = true;
            collisionVis.Draw(context);
            context.Wireframe = false;
#endif
            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i] != null) ships[i].Draw(context);
            }
        }

        public bool RayCast(Vector3 origin, Vector3 direction, out float distance, out Vector3 normal)
        {
            BroadPhaseEntry[] e;
            float[] dists;
            Vector3[] norms;

            bool res = phys.RayCast(origin, direction, out dists, out norms, out e);

            distance = 0;
            normal = -direction;

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
            phys.Update(interval / 1000d);

            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i] != null) ships[i].Update(interval, context, this);
            }
        }
    }
}
