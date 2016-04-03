using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.Physics
{
    public abstract class BaseEntity
    {
        private static ulong id_counter = 0;

        internal BEPUphysics.Entities.Entity physEntity;

        private ulong id;
        public ulong ID
        {
            get { return id; }
            set
            {
                id = value;
                physEntity.Tag = physEntity.CollisionInformation.Tag = id;
            }
        }

        public PhysicsWorld ParentSpace { get; internal set; }

        public float Mass { get { return physEntity.Mass; } set { physEntity.Mass = value; } }
        public Vector3 LinearVelocity { get { return physEntity.LinearVelocity; } set { physEntity.LinearVelocity = value; } }
        public Vector3 LinearMomentum { get { return physEntity.LinearMomentum; } set { physEntity.LinearMomentum = value; } }
        public float LinearDamping { get { return physEntity.LinearDamping; } set { physEntity.LinearDamping = value; } }

        public Vector3 AngularVelocity { get { return physEntity.AngularVelocity; } set { physEntity.AngularVelocity = value; } }
        public Vector3 AngularMomentum { get { return physEntity.AngularMomentum; } set { physEntity.AngularMomentum = value; } }
        public float AngularDamping { get { return physEntity.AngularDamping; } set { physEntity.AngularDamping = value; } }

        public Vector3? Gravity { get { return physEntity.Gravity; } set { physEntity.Gravity = value; } }

        public Vector3 Position { get { return physEntity.Position; } set { physEntity.Position = value; } }
        public Quaternion Orientation { get { return physEntity.Orientation; } set { physEntity.Orientation = value; } }

        public PositionUpdateMode PositionUpdateMode { get { return (PositionUpdateMode)physEntity.PositionUpdateMode; } set { physEntity.PositionUpdateMode = (BEPUphysics.PositionUpdating.PositionUpdateMode)value; } }

        public event Action<BaseEntity> PositionUpdated;

        private Vector3 rotLock;
        public Vector3 RotationLock
        {
            get
            {
                return rotLock;
            }
            set
            {
                rotLock = new Vector3(value.X != 1 ? 0 : 1, value.Y != 1 ? 0 : 1, value.Z != 1 ? 0 : 1);
                physEntity.LocalInertiaTensorInverse = new BEPUutilities.Matrix3x3(1 * rotLock.X, 0, 0,
                                                                                    0, 1 * rotLock.Y, 0,
                                                                                    0, 0, 1 * rotLock.Z);
            }
        }

        public BaseEntity(BEPUphysics.Entities.Entity e)
        {
            physEntity = e;
            ID = ++id_counter;

            e.PositionUpdated += (a) =>
            {
                PositionUpdated?.Invoke(this);
            };
        }

        public void ApplyLinearImpulse(Vector3 val)
        {
            BEPUutilities.Vector3 imp = val;
            physEntity.ApplyLinearImpulse(ref imp);
        }

        public void ApplyAngularImpulse(Vector3 val)
        {
            BEPUutilities.Vector3 imp = val;
            physEntity.ApplyAngularImpulse(ref imp);
        }

        public void ApplyImpulse(Vector3 loc, Vector3 val)
        {
            BEPUutilities.Vector3 loc_ = loc;
            BEPUutilities.Vector3 imp = val;
            physEntity.ApplyImpulse(loc_, imp);
        }

        public bool RayCast(Vector3 origin, Vector3 direction, out float distance, out Vector3 normal)
        {
            ulong[] e;
            float[] dists;
            Vector3[] norms;

            bool res = ParentSpace.RayCast(origin, direction, out dists, out norms, out e);

            distance = 0;
            normal = -direction;

            for (int i = 0; i < e.Length; i++)
            {
                if (e[i] == ID)
                {
                    distance = dists[i];
                    normal = norms[i];
                    return true;
                }
            }
            return false;
        }
    }
}
