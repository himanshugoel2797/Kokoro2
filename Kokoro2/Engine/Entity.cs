using Kokoro2.Engine.SceneGraph;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public abstract class Entity
    {
        public Entity Parent;
        public Vector3 Position;
        public Quaternion Rotation;
        public Matrix4 WorldTransform;
        public float Radius;
        public uint ID;

        public virtual void Activate(GraphicsContext context, double interval)
        {
            WorldTransform = Matrix4.CreateTranslation(Position) * Matrix4.CreateRotation(Rotation);
            if (this.Parent != null) WorldTransform *= this.Parent.WorldTransform;  //Build the transformations properly, this entity should be translated by its parents
        }
        public abstract void Update(GraphicsContext context, double interval);
    }
}
