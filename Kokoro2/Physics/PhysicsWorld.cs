using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Math;

namespace Kokoro2.Physics
{
    public class PhysicsWorld
    {
        private Dictionary<long, PhysicsBody> bodies;
        internal Queue<long> activeBodies;
        internal Queue<long> prevActiveBodies;

        public bool Enabled = true;

        public Vector3 Gravity
        {
            get;
            set;
        }

        public PhysicsWorld()
        {
            bodies = new Dictionary<long, PhysicsBody>();
            activeBodies = new Queue<long>();
            prevActiveBodies = new Queue<long>();
        }

        /// <summary>
        /// Update the physics simulation, interval is in milliseconds
        /// </summary>
        /// <param name="interval"></param>
        public void Update(double interval)
        {
            interval /= 1000.0d;    //Convert to seconds

            //Check all the previously active bodies for collisions and update them as necessary

            prevActiveBodies.Clear();
            while (activeBodies.Count > 0)
            {
                var b = bodies[activeBodies.Dequeue()];
                
                var g = Gravity;
                if (b.State.Gravity.HasValue)
                    g = b.State.Gravity.Value;

                //Update the positions of everything here
                Vector3 nF = g;
                Vector3 tq = Vector3.Zero;

                //Calculate the net force on the object by calculating all the collisions

                //Find all the nearest objects

                var a_dt = new Vector3((float)(nF.X * interval), (float)(nF.Y * interval), (float)(nF.Z * interval));
                var v_dt = new Vector3((float)(b.State.Velocity.X * interval), (float)(b.State.Velocity.Y * interval), (float)(b.State.Velocity.Z * interval));

                b.State.Position += new Vector3((float)(a_dt.X * interval), (float)(a_dt.Y * interval), (float)(a_dt.Z * interval)) * 0.5f;
                b.State.Position += v_dt;

                b.State.Velocity += a_dt;

                b.State.Momentum = b.State.Mass * b.State.Velocity;

                prevActiveBodies.Enqueue(b.ID);
            }
        }

        public void AddEntity(PhysicsBody m)
        {
            m.Parent = null;
            bodies.Add(m.ID, m);
        }

    }
}
