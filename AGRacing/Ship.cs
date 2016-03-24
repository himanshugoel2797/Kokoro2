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
    class Ship
    {
        VertexMesh Mesh;
        public string Name { get; private set; }
        public Vector3 Position;
        public Vector3 Direction;


        private float Scale;
        private Vector3 Rotations = Vector3.Zero;

        public Ship(string infoString)
        {
            string[] parts = infoString.Split(',');
            if (parts.Length < 3) throw new ArgumentException();

            Name = parts[1];

            Mesh = new VertexMesh("Resources/Proc/Car_Vis/" + parts[0] + ".ko", false);

            for (int i = 0; i < Mesh.Materials.Length; i++)
            {
                Mesh.Materials[i].AlbedoMap = new Texture("Resources/Proc/Tex/" + parts[2] + "tex.png");
                Mesh.Materials[i].Shader = new Kokoro2.Engine.Shaders.ShaderProgram(VertexShader.Load("Default"), FragmentShader.Load("Default"));
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
            }

        }

        public void Draw(GraphicsContext context)
        {
            Mesh.Draw(context);
        }

        public void Update(double interval, GraphicsContext context, Track t)
        {
            Mesh.World = Matrix4.Scale(Scale) * Matrix4.CreateRotationX(Rotations.X) * Matrix4.CreateRotationY(Rotations.Y) * Matrix4.CreateRotationZ(Rotations.Z) * Matrix4.CreateTranslation(Position);
        }
    }
}
