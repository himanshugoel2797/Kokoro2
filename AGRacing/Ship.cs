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
        public float Mass = 5f;

        private float Scale;
        private Vector3 Rotations = Vector3.Zero;
        private Quaternion Orientation;
        private IShipController controller;
        private BEPUphysics.Entities.Prefabs.Box collisionMesh;

#if DEBUG
        private Box colVis;
#endif

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

            Vector3 min, max;
            GetBounds(out min, out max);

            Vector3 r = Vector3.Cross(MeshFront, Vector3.UnitY);
            r.Normalize();
            r.Round();

            Vector3 t = (max - min);

            Vector3 w0 = new Vector3(t.X * r.X, t.Y * r.Y, t.Z * r.Z);
            Vector3 h0 = new Vector3(t.X * 0, t.Y * 1, t.Z * 0);
            Vector3 l0 = new Vector3(t.X * MeshFront.X, t.Y * MeshFront.Y, t.Z * MeshFront.Z);

            float w = 0, h = 0, l = 0;
            for (int i = 0; i < 3; i++)
            {
                if (w0[i] != 0) w = w0[i];
                else if (h0[i] != 0) h = h0[i];
                else if (l0[i] != 0) l = l0[i];
            }

            collisionMesh = new BEPUphysics.Entities.Prefabs.Box(Position, w * Scale, h * Scale, l * Scale, Mass);
            collisionMesh.PositionUpdateMode = BEPUphysics.PositionUpdating.PositionUpdateMode.Continuous;
            collisionMesh.PositionUpdated += CollisionMesh_PositionUpdated;
            collisionMesh.Orientation = Quaternion.FromAxisAngle(Vector3.UnitX, Rotations.X) * Quaternion.FromAxisAngle(Vector3.UnitY, Rotations.Y) * Quaternion.FromAxisAngle(Vector3.UnitZ, Rotations.Z);
            collisionMesh.Gravity = new BEPUutilities.Vector3(0, 0, 0);

#if DEBUG
            colVis = new Box(collisionMesh.Width, collisionMesh.Height, collisionMesh.Length);
            colVis.Materials[0].Shader = new Kokoro2.Engine.Shaders.ShaderProgram(VertexShader.Load("Default"), FragmentShader.Load("Default"));
            colVis.Materials[0].AlbedoMap = Mesh.Materials[0].AlbedoMap;
#endif
        }

        private void CollisionMesh_PositionUpdated(BEPUphysics.Entities.Entity obj)
        {
            Position = collisionMesh.Position;
            Orientation = collisionMesh.Orientation;
#if DEBUG
            Vector3 axis;
            float angle;
            ((Quaternion)collisionMesh.Orientation).ToAxisAngle(out axis, out angle);
            colVis.World = Matrix4.CreateFromAxisAngle(axis, angle) * Matrix4.CreateTranslation(Position);
#endif
        }

        public void GetBounds(out Vector3 min, out Vector3 max)
        {
            min = Mesh.Bound.Min;
            max = Mesh.Bound.Max;
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
            else if (str.StartsWith("FrontDir"))
            {
                var t = str.Split('=')[1].Split(':');
                MeshFront = new Vector3(float.Parse(t[0]), float.Parse(t[1]), float.Parse(t[2]));
            }
            else if (str.StartsWith("Mass"))
            {
                Mass = float.Parse(str.Split('=')[1]);
            }

        }

        public BEPUphysics.Entities.Prefabs.Box GetPhysicsEntity()
        {
            return collisionMesh;
        }

        public void Draw(GraphicsContext context)
        {
            Mesh.Draw(context);

#if DEBUG
            context.Wireframe = true;
            colVis.Draw(context);
            context.Wireframe = false;
#endif
        }

        public Vector3 GetRayLocation(ShipRayLocations loc)
        {
            Vector3 r = Vector3.Cross(MeshFront, Vector3.UnitY);
            r.Normalize();
            r.Round();

            Vector3 t = (Mesh.Bound.Max - Mesh.Bound.Min) / 2;

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

        public Vector3 GetRayDirection(ShipRayLocations loc)
        {
            Vector3 r = Vector3.Cross(MeshFront, Vector3.UnitY);
            r.Normalize();
            r.Round();

            switch (loc)
            {
                case ShipRayLocations.BackLeftBottom:
                    return new Vector3(0, -1, 0);
                case ShipRayLocations.BackLeftSide:
                    return -r;
                case ShipRayLocations.BackRightBottom:
                    return new Vector3(0, -1, 0);

                case ShipRayLocations.BackRightSide:
                    return r;

                case ShipRayLocations.FrontLeftBottom:
                    return new Vector3(0, -1, 0);

                case ShipRayLocations.FrontLeftSide:
                    return -r;

                case ShipRayLocations.FrontRightBottom:
                    return new Vector3(0, -1, 0);

                case ShipRayLocations.FrontRightSide:
                    return r;

                default:
                    throw new ArgumentException();
            }
        }

        public void ApplyImpulse(Vector3 loc, Vector3 amnt)
        {
            collisionMesh.ApplyImpulse(loc, amnt);
        }

        public void ChangePosition(Vector3 nPos)
        {
            collisionMesh.Position = nPos;
        }

        float impulse = 3410;
        public void Update(double interval, GraphicsContext context, Track t)
        {

            Vector3 axis;
            float angle;
            ((Quaternion)collisionMesh.Orientation).ToAxisAngle(out axis, out angle);

            controller.Update(this, t);
            Mesh.World = Matrix4.Scale(Scale) * Matrix4.CreateFromAxisAngle(axis, angle) * Matrix4.CreateTranslation(Position);

            float dist = 0;
            Vector3 norm = Vector3.Zero;

            /*if (t.RayCast(Position, -Vector3.UnitY, out dist, out norm))
            {
                if (dist > 1)
                {
                    //collisionMesh.IsAffectedByGravity = true;
                }
                else collisionMesh.IsAffectedByGravity = false;
            }
            else collisionMesh.IsAffectedByGravity = false;*/
        }
    }
}
