using Kokoro2.Math;
using Kokoro2.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel
{
    /// <summary>
    /// Represents an entity to the engine
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Contains the identifier of this object
        /// </summary>
        public int ID;

        /// <summary>
        /// The position of the body
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return State.Position;
            }
            set
            {
                State.Position = value;
            }
        }

        /// <summary>
        /// The renderable representation of the entity
        /// </summary>
        public Model Renderable;

        /// <summary>
        /// Enable/Disable rendering of the entity
        /// </summary>
        public bool Visible;

        /// <summary>
        /// The PhysicsState of the body
        /// </summary>
        public PhysicsState State;

        public void Update(double interval, GraphicsContext context)
        {

        }

        public void Render(double interval, GraphicsContext context)
        {
            //Check if this object ought to be visible, if so, draw it
            if (Renderable != null && Visible)
            {
                Renderable.World = Matrix4.CreateTranslation(Position);
                Renderable.Draw(context);
            }
        }
    }
}
