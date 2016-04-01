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
        public Vector3 RealFront = -Vector3.UnitZ;
        public Vector3 PhysicalFront = -Vector3.UnitZ;
        public Vector3 PhysicalRight;
        public Vector3 MovementDirection;
        public Vector3 Velocity
        {
            get
            {
                return collisionMesh.LinearVelocity;
            }
        }
        public float Mass = 100f;

        public float Scale;
        private Vector3 Rotations = Vector3.Zero;
        private Quaternion Orientation;
        private IShipController controller;
        private BEPUphysics.Entities.Entity collisionMesh;

        public ShaderProgram Shader
        {
            get { return Mesh.Shader; }
            set { Mesh.Shader = value; }
        }

#if DEBUG
        private Model colVis;
#endif

        public Ship(string infoString, IShipController controller)
        {
            string[] parts = infoString.Split(',');
            if (parts.Length < 3) throw new ArgumentException();

            Name = parts[1];
            this.controller = controller;
            Mesh = new VertexMesh("Resources/Proc/Car_Vis/" + parts[0] + ".ko", false);

            Mesh.AlbedoMap = new Texture("Resources/Proc/Tex/" + parts[2] + "tex.png");
            Mesh.Materials[0].GlossinessMap = new Texture("Resources/Proc/Tex/" + parts[2] + "roughness.png");
            Mesh.PushShader(new ShaderProgram(VertexShader.Load("Shadowed"), FragmentShader.Load("Shadowed")));

            if (parts.Length > 3)
            {
                for (int i = 3; i < parts.Length; i++)
                {
                    ParseConfigData(parts[i]);
                }
            }

            Vector3 min, max;
            GetBounds(out min, out max);

            Vector3 r = Vector3.Cross(RealFront, Vector3.UnitY);
            r.Normalize();
            r.Round();

            Vector3 t = (max - min);

            Vector3 w0 = new Vector3(t.X * r.X, t.Y * r.Y, t.Z * r.Z);
            Vector3 h0 = new Vector3(t.X * 0, t.Y * 1, t.Z * 0);
            Vector3 l0 = new Vector3(t.X * RealFront.X, t.Y * RealFront.Y, t.Z * RealFront.Z);

            float w = 0, h = 0, l = 0;
            for (int i = 0; i < 3; i++)
            {
                if (w0[i] != 0) w = w0[i];
                else if (h0[i] != 0) h = h0[i];
                else if (l0[i] != 0) l = l0[i];
            }
            float sc = 1 * Scale;

            float[] verts = VertexMesh.GetVertices("Resources/Proc/Car_Vis/" + parts[0] + ".ko", false);

            List<BEPUutilities.Vector3> v = new List<BEPUutilities.Vector3>();
            for (int i = 0; i < verts.Length; i += 3)
            {
                v.Add(new BEPUutilities.Vector3(verts[i] * sc, verts[i + 1] * sc, verts[i + 2] * sc));
            }

            collisionMesh = new BEPUphysics.Entities.Prefabs.Sphere(Position, 0.5f, Mass);
            //collisionMesh = new BEPUphysics.Entities.Prefabs.MobileMesh(v.ToArray(), VertexMesh.GetIndices("Resources/Proc/Car_Vis/" + parts[0] + ".ko", false), BEPUutilities.AffineTransform.Identity, BEPUphysics.CollisionShapes.MobileMeshSolidity.DoubleSided, Mass);

            collisionMesh.PositionUpdateMode = BEPUphysics.PositionUpdating.PositionUpdateMode.Continuous;
            collisionMesh.PositionUpdated += CollisionMesh_PositionUpdated;

            collisionMesh.Orientation = Quaternion.FromAxisAngle(Vector3.UnitX, Rotations.X) * Quaternion.FromAxisAngle(Vector3.UnitY, Rotations.Y) * Quaternion.FromAxisAngle(Vector3.UnitZ, Rotations.Z);
            PhysicalFront = Vector3.TransformVector(RealFront, Matrix4.CreateFromQuaternion(collisionMesh.Orientation));

#if DEBUG
            colVis = new VertexMesh("Resources/Proc/Car_Vis/" + parts[0] + ".ko", false);
            colVis.PushShader(new ShaderProgram(VertexShader.Load("Shadowed"), FragmentShader.Load("Shadowed")));
            colVis.AlbedoMap = Mesh.Materials[0].AlbedoMap;
#endif
        }

        public void PushShader(ShaderProgram s)
        {
            Mesh.PushShader(s);
#if DEBUG
            colVis.PushShader(s);
#endif
        }

        public ShaderProgram PopShader()
        {
#if DEBUG
            colVis.PopShader();
#endif
            return Mesh.PopShader();
        }

        private void CollisionMesh_PositionUpdated(BEPUphysics.Entities.Entity obj)
        {
            Position = collisionMesh.CollisionInformation.LocalPosition + collisionMesh.Position;
            Orientation = collisionMesh.Orientation;
            var rot = Matrix4.CreateFromQuaternion(Orientation);

            PhysicalFront = -Vector3.TransformVector(RealFront, rot);
            MovementDirection = collisionMesh.LinearVelocity;
            MovementDirection.Normalize();

            PhysicalRight = Vector3.Cross(PhysicalFront, Vector3.UnitY);
            PhysicalRight.Normalize();
#if DEBUG
            Vector3 axis;
            float angle;
            ((Quaternion)collisionMesh.Orientation).ToAxisAngle(out axis, out angle);
            colVis.World = Matrix4.Scale(Scale) * Matrix4.CreateFromAxisAngle(axis, angle) * Matrix4.CreateTranslation(collisionMesh.Position);
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
                Rotations += Vector3.UnitX * (float)(Math.PI * float.Parse(str.Split('=')[1]) / 180f);
            }
            else if (str.StartsWith("RotY"))
            {
                Rotations += Vector3.UnitY * (float)(Math.PI * float.Parse(str.Split('=')[1]) / 180f);
            }
            else if (str.StartsWith("RotZ"))
            {
                Rotations += Vector3.UnitZ * (float)(Math.PI * float.Parse(str.Split('=')[1]) / 180f);
            }
            else if (str.StartsWith("FrontDir"))
            {
                var t = str.Split('=')[1].Split(':');
                RealFront = new Vector3(float.Parse(t[0]), float.Parse(t[1]), float.Parse(t[2]));
            }
            else if (str.StartsWith("Mass"))
            {
                Mass = float.Parse(str.Split('=')[1]);
            }

        }

        public BEPUphysics.Entities.Entity GetPhysicsEntity()
        {
            return collisionMesh;
        }

        public void Draw(GraphicsContext context)
        {
            Mesh.Draw(context);

#if DEBUG
            context.Wireframe = true;
            //colVis.Draw(context);
            context.Wireframe = false;
#endif
        }

        public Vector3 GetRayLocation(ShipRayLocations loc)
        {
            Vector3 r = Vector3.Cross(RealFront, Vector3.UnitY);
            r.Normalize();
            r.Round();

            Vector3 t = (Mesh.Bound.Max - Mesh.Bound.Min) / 2;
            t *= Scale;

            Vector3 w = new Vector3(t.X * r.X, t.Y * r.Y, t.Z * r.Z);
            Vector3 h = new Vector3(t.X * 0, t.Y * 1, t.Z * 0);
            Vector3 l = new Vector3(t.X * RealFront.X, t.Y * RealFront.Y, t.Z * RealFront.Z);

            Vector3 pos = Vector3.Zero;

            switch (loc)
            {
                case ShipRayLocations.BackRightBottom:
                    pos = -l - w - h;
                    break;
                case ShipRayLocations.BackLeftBottom:
                    pos = -l + w - h;
                    break;
                case ShipRayLocations.FrontRightBottom:
                    pos = l - w - h;
                    break;
                case ShipRayLocations.FrontLeftBottom:
                    pos = l + w - h;
                    break;
                case ShipRayLocations.BackRightSide:
                    pos = -l - w;
                    break;
                case ShipRayLocations.BackLeftSide:
                    pos = -l + w;
                    break;
                case ShipRayLocations.FrontRightSide:
                    pos = l - w;
                    break;
                case ShipRayLocations.FrontLeftSide:
                    pos = l + w;
                    break;
                default:
                    throw new ArgumentException();
            }

            pos = Vector3.TransformPosition(pos, Matrix4.CreateFromQuaternion(Orientation));
            pos += Position;
            return pos;
        }

        public Vector3 GetRayDirection(ShipRayLocations loc)
        {
            Vector3 r = Vector3.Cross(RealFront, Vector3.UnitY);
            r.Normalize();
            r.Round();

            Vector3 toRet = Vector3.Zero;

            switch (loc)
            {
                case ShipRayLocations.BackLeftBottom:
                    toRet = new Vector3(0, -1, 0);
                    break;

                case ShipRayLocations.BackLeftSide:
                    toRet = -r;
                    break;

                case ShipRayLocations.BackRightBottom:
                    toRet = new Vector3(0, -1, 0);
                    break;

                case ShipRayLocations.BackRightSide:
                    toRet = r;
                    break;

                case ShipRayLocations.FrontLeftBottom:
                    toRet = new Vector3(0, -1, 0);
                    break;

                case ShipRayLocations.FrontLeftSide:
                    toRet = -r;
                    break;

                case ShipRayLocations.FrontRightBottom:
                    toRet = new Vector3(0, -1, 0);
                    break;

                case ShipRayLocations.FrontRightSide:
                    toRet = r;
                    break;

                default:
                    throw new ArgumentException();
            }

            toRet = Vector3.TransformVector(toRet, Matrix4.CreateFromQuaternion(Orientation));

            return -Vector3.UnitY;
            return toRet;
        }

        public void ApplyImpulse(Vector3 loc, Vector3 amnt)
        {
            BEPUutilities.Vector3 l = loc;
            BEPUutilities.Vector3 a = amnt;
            //collisionMesh.ApplyImpulse(ref l, ref a);
            collisionMesh.ApplyLinearImpulse(ref a);
        }

        public void ChangeVelocity(Vector3 nPos)
        {

            BEPUutilities.Vector3 impulse = nPos;
            collisionMesh.ApplyLinearImpulse(ref impulse);
        }

        public void Rotate(Vector3 axis, float angle)
        {
            BEPUutilities.Vector3 rot = axis * angle;
            collisionMesh.Orientation = (Quaternion)collisionMesh.Orientation * Quaternion.FromAxisAngle(axis, angle);
        }

        public bool isGrounded = false;
        const float height = 3.0f, force = 30;
        private void CalcAG(ShipRayLocations l, Track t)
        {
            float dist;
            Vector3 norm;

            bool res = t.RayCast(GetRayLocation(l), GetRayDirection(l), out dist, out norm);

            if (res && dist < height)
            {
                ApplyImpulse(GetRayLocation(l), Vector3.UnitY * force * (height - dist) / height - new Vector3(0, collisionMesh.LinearVelocity.Y, 0));
                isGrounded = true;
            }
        }


        public int findNearestTrackPoint(Track t)
        {
            float closestLen = 10000;
            int index = 0;

            for (int i = 0; i < t.GetPointCount(); i += 2)
            {
                if ((t.GetPosition(i) - Position).LengthSquared < closestLen)
                {
                    closestLen = (t.GetPosition(i) - Position).LengthSquared;
                    index = i;
                }
            }

            return index;
        }

        float c = 0;
        public Vector3 prevUp = Vector3.UnitY;
        public void Update(double interval, GraphicsContext context, Track t)
        {
            Vector3 r = Vector3.Cross(RealFront, Vector3.UnitY);
            r.Normalize();
            int index = findNearestTrackPoint(t);
            Vector3 tPos = t.GetPosition(index);
            Vector3 dir = t.GetDirection(index);
            Vector3 N = Vector3.Cross(dir, PhysicalRight);
            N.Normalize();

            float angle = Vector3.Dot(N, Vector3.UnitY);
            angle = (float)Math.Acos(angle);

            if (angle > Math.PI / 2.0f)
            {
                dir = -dir;
                N = Vector3.Cross(dir, PhysicalRight);
                N.Normalize();
                angle = Vector3.Dot(N, Vector3.UnitY);
                angle = (float)Math.Acos(angle);
            }

            isGrounded = false;
            controller.Update(this, t);

            collisionMesh.LocalInertiaTensorInverse = new BEPUutilities.Matrix3x3(0, 0, 0,
                                                                             0, 0, 0,
                                                                            0, 0, 0);

            float tilt = Vector3.Dot(MovementDirection, PhysicalFront);
            tilt = (float)Math.Acos(tilt);
            if (Vector3.Dot(MovementDirection, PhysicalRight) > 0) tilt = -tilt;

            tilt *= 0.75f;

            CalcAG(ShipRayLocations.BackLeftBottom, t);
            CalcAG(ShipRayLocations.BackRightBottom, t);
            CalcAG(ShipRayLocations.FrontLeftBottom, t);
            CalcAG(ShipRayLocations.FrontRightBottom, t);
            Mesh.World = Matrix4.Scale(Scale) * Matrix4.CreateFromQuaternion(Orientation) * Matrix4.CreateFromAxisAngle(PhysicalRight, -0.2f + 0.05f * (float)Math.Sin(c) + angle) * Matrix4.CreateFromAxisAngle(PhysicalFront, tilt) * Matrix4.CreateTranslation(Position);
            c += 0.05f;

            /*
            float distblB = 0, distbrB = 0, distfrB = 0, distflB = 0, distflS = 0, distfrS = 0, distblS = 0, distbrS = 0, dist = 0;
            Vector3 normblB = Vector3.Zero, normbrB = Vector3.Zero, normflB = Vector3.Zero, normfrB = Vector3.Zero, normflS = Vector3.Zero, normfrS = Vector3.Zero, normblS = Vector3.Zero, normbrS = Vector3.Zero, norm = Vector3.Zero;

            bool blBottom = t.RayCast(GetRayLocation(ShipRayLocations.BackLeftBottom), GetRayDirection(ShipRayLocations.BackLeftBottom), out distblB, out normblB);
            bool brBottom = t.RayCast(GetRayLocation(ShipRayLocations.BackRightBottom), GetRayDirection(ShipRayLocations.BackRightBottom), out distbrB, out normbrB);
            bool flBottom = t.RayCast(GetRayLocation(ShipRayLocations.FrontLeftBottom), GetRayDirection(ShipRayLocations.FrontLeftBottom), out distflB, out normflB);
            bool frBottom = t.RayCast(GetRayLocation(ShipRayLocations.FrontRightBottom), GetRayDirection(ShipRayLocations.FrontRightBottom), out distfrB, out normfrB);
            bool frSide = t.RayCast(GetRayLocation(ShipRayLocations.FrontRightSide), GetRayDirection(ShipRayLocations.FrontRightSide), out distfrS, out normfrS);
            bool flSide = t.RayCast(GetRayLocation(ShipRayLocations.FrontLeftSide), GetRayDirection(ShipRayLocations.FrontLeftSide), out distflS, out normflS);
            bool brSide = t.RayCast(GetRayLocation(ShipRayLocations.BackRightSide), GetRayDirection(ShipRayLocations.BackRightSide), out distbrS, out normbrS);
            bool blSide = t.RayCast(GetRayLocation(ShipRayLocations.BackLeftSide), GetRayDirection(ShipRayLocations.BackLeftSide), out distblS, out normblS);

            float force = 60;
            float height = 2f;

            normblB.Normalize();
            normbrB.Normalize();
            normflB.Normalize();
            normfrB.Normalize();
            normflS.Normalize();
            normfrS.Normalize();
            normblS.Normalize();
            normbrS.Normalize();*/



            float distB, distL, distR, distF;
            Vector3 nB, nL, nR, nF;




            /*t.RayCast(tPos, -Vector3.UnitY, out distB, out nB);
            t.RayCast(tPos + PhysicalFront, -Vector3.UnitY, out distF, out nF);
            t.RayCast(tPos + worldRight, -Vector3.UnitY, out distR, out nR);
            t.RayCast(tPos - worldRight, -Vector3.UnitY, out distL, out nL);


            Vector3 N = new Vector3(
            (distL - distR)
            , 2.0f,
            (distB - distF)
            );*/




            //float angle = 1.0f - Vector3.Dot(prevUp, N);
            //angle = (float)Math.Round(angle, 4);
            //collisionMesh.Orientation *= (BEPUutilities.Quaternion)Quaternion.FromAxisAngle(r, angle);




            Vector3 flat_v = collisionMesh.LinearVelocity;
            flat_v = new Vector3(flat_v.X, 0, flat_v.Z);

            float slip = Vector3.Dot(PhysicalRight, flat_v);

            Vector3 anti_slip = -slip * PhysicalRight * 2;
            BEPUutilities.Vector3 b_anti_slip = anti_slip;
            collisionMesh.ApplyLinearImpulse(ref b_anti_slip);

            //netAngle += 0.0001f;
            //Console.WriteLine(N+ " Angle:" + angle);

            //angle = netAngle;

            //prevUp = N;

            /*
            float rot = Vector3.Dot(Vector3.UnitY, N);
            rot = (float)Math.Round(rot, 2);
            //collisionMesh.Orientation *= (BEPUutilities.Quaternion)Quaternion.FromAxisAngle(Vector3.UnitY, rot);
            */


            /*
            if (frSide && distfrS < 0.2)
            {
                BEPUutilities.Vector3 impulse = normfrS * 100;
                collisionMesh.ApplyLinearImpulse(ref impulse);
            }

            if (flSide && distflS < 0.2)
            {
                BEPUutilities.Vector3 impulse = normflS * 100;
                collisionMesh.ApplyLinearImpulse(ref impulse);
            }*/

            /*if (t.RayCast(Position, -Vector3.UnitY, out dist, out norm))
            {
                BEPUutilities.Vector3 impulse = norm * 8;
                if (dist < 1)
                {
                    collisionMesh.ApplyLinearImpulse(ref impulse);
                }
                else if (dist > 1.001f)
                {
                    impulse *= -1;
                    collisionMesh.ApplyLinearImpulse(ref impulse);
                }
            }
            else {
                collisionMesh.LinearMomentum *= (BEPUutilities.Vector3)(Vector3.One - Vector3.UnitY);
                collisionMesh.LinearVelocity *= (BEPUutilities.Vector3)(Vector3.One - Vector3.UnitY);
            }*/
        }
    }
}
