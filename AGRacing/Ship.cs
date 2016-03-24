using Kokoro2.Engine;
using Kokoro2.Engine.Prefabs;
using Kokoro2.Engine.Shaders;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGRacing
{
    enum ShipRayLocations
    {
        FrontLeftSide,
        FrontRightSide,
        BackLeftSide,
        BackRightSide,
        FrontLeftBottom,
        FrontRightBottom,
        BackLeftBottom,
        BackRightBottom
    }

    class Ship
    {
        VertexMesh Mesh;
        public string Name { get; private set; }
        public Vector3 Position;
        public Vector3 Direction;
        public Vector3 MeshFront = -Vector3.UnitZ;


        private float Scale;
        private Vector3 Rotations = Vector3.Zero;
        private IShipController controller;

        public Ship(string infoString, IShipController controller)
        {
            string[] parts = infoString.Split(',');
            if (parts.Length < 3) throw new ArgumentException();

            Name = parts[1];
            this.controller = controller;
            Mesh = new VertexMesh("Resources/Proc/Car_Vis/" + parts[0] + ".ko", false);

            for (int i = 0; i < Mesh.Materials.Length; i++)
            {
                Mesh.Materials[i].AlbedoMap = new Texture("Resources/Proc/Tex/" + parts[2] + "tex.png");
                Mesh.Materials[i].Shader = new ShaderProgram(VertexShader.Load("Default"), FragmentShader.Load("Default"));
            }

            if (parts.Length > 3)
            {
                for (int i = 3; i < parts.Length; i++)
                {
                    ParseConfigData(parts[i]);
                }
            }
        }

        private void ParseConfigData(string str)
        {
            if (str.StartsWith("Scale"))
            {
                Scale = float.Parse(str.Split('=')[1]);
            }
            else if (str.StartsWith("RotX"))
            {
                Rotations += Vector3.UnitX * float.Parse(str.Split('=')[1]);
            }
            else if (str.StartsWith("RotY"))
            {
                Rotations += Vector3.UnitY * float.Parse(str.Split('=')[1]);
            }
            else if (str.StartsWith("RotZ"))
            {
                Rotations += Vector3.UnitZ * float.Parse(str.Split('=')[1]);
            }else if(str.StartsWith("FrontDir"))
            {
                var t = str.Split('=')[1].Split(':');
                MeshFront = new Vector3(float.Parse(t[0]), float.Parse(t[1]), float.Parse(t[2]));
            }

        }

        public void Draw(GraphicsContext context)
        {
            Mesh.Draw(context);
        }

        public Vector3 GetRayLocation(ShipRayLocations loc)
        {
            Vector3 r = Vector3.Cross(MeshFront, Vector3.UnitY);
            r.Normalize();
            r.Round();

            Vector3 t = (Mesh.Bound.Max - Mesh.Bound.Min)/2;

            Vector3 w = new Vector3(t.X * r.X, t.Y * r.Y, t.Z * r.Z);
            Vector3 h = new Vector3(t.X * 0, t.Y * 1, t.Z * 0);
            Vector3 l = new Vector3(t.X * MeshFront.X, t.Y * MeshFront.Y, t.Z * MeshFront.Z);

            switch (loc)
            {
                case ShipRayLocations.BackLeftBottom:
                    return Position - l - w - h;
                case ShipRayLocations.BackRightBottom:
                    return Position - l + w - h;
                case ShipRayLocations.FrontLeftBottom:
                    return Position + l - w - h;
                case ShipRayLocations.FrontRightBottom:
                    return Position + l + w - h;
                case ShipRayLocations.BackLeftSide:
                    return Position - l - w;
                case ShipRayLocations.BackRightSide:
                    return Position - l + w;
                case ShipRayLocations.FrontLeftSide:
                    return Position + l - w;
                case ShipRayLocations.FrontRightSide:
                    return Position + l + w;
                default:
                    throw new ArgumentException();
            }
        }

        public void Update(double interval, GraphicsContext context, Track t)
        {
            controller.Update(this, t);
            Mesh.World = Matrix4.Scale(Scale) * Matrix4.CreateRotationX(Rotations.X) * Matrix4.CreateRotationY(Rotations.Y) * Matrix4.CreateRotationZ(Rotations.Z) * Matrix4.CreateTranslation(Position);
        }
    }
}
