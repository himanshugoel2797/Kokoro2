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
        StaticMesh collisionMesh;
        VertexMesh trackModel;
        float[] trackPath;
        PhysicsWorld phys;
        Ship[] ships;


        public string Name { get; private set; }

        public Track(string infoLine)
        {
            string[] parts = infoLine.Split(',');
            if (parts.Length != 3) throw new ArgumentException();
            Name = parts[1];
            trackModel = new VertexMesh("Resources/Proc/Track_Vis/" + parts[0] + ".ko", false);

            for (int i = 0; i < trackModel.Materials.Length; i++)
            {
                trackModel.Materials[i].AlbedoMap = new Texture("Resources/Proc/Tex/" + parts[2]);
                trackModel.Materials[i].Shader = new Kokoro2.Engine.Shaders.ShaderProgram(VertexShader.Load("Default"), FragmentShader.Load("Default"));
            }

            trackPath = VertexMesh.GetVertices("Resources/Proc/Track_Path/" + parts[0] + "_path.ko", false);

            float[] verts = VertexMesh.GetVertices("Resources/Proc/Track_Path/" + parts[0] + "_col.ko", false);
            int[] indices = VertexMesh.GetIndices("Resources/Proc/Track_Path/" + parts[0] + "_col.ko", false);

            List<BEPUutilities.Vector3> v = new List<BEPUutilities.Vector3>();
            for (int i = 0; i < verts.Length; i += 3)
            {
                v.Add(new BEPUutilities.Vector3(verts[i], verts[i + 1], verts[i + 2]));
            }

            ships = new Ship[8];

            phys = new PhysicsWorld();
            collisionMesh = new StaticMesh(v.ToArray(), indices);
            phys.AddEntity(collisionMesh);
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
            s.Position = GetPosition(position);
            s.Direction = GetDirection(position);
            ships[position] = s;
        }

        public void Dispose()
        {
            ((IDisposable)trackModel).Dispose();
        }

        public void Draw(GraphicsContext context)
        {
            trackModel.Draw(context);

            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i] != null) ships[i].Draw(context);
            }
        }

        public bool RayCast(Vector3 origin, Vector3 direction, out float distance, out Vector3 normal)
        {
            BroadPhaseEntry e;
            bool res = phys.RayCast(origin, direction, out distance, out normal, out e);

            return res && (e == collisionMesh);
        }

        public void Update(double interval, GraphicsContext context)
        {
            phys.Update(interval);

            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i] != null) ships[i].Update(interval, context, this);
            }
        }
    }
}
