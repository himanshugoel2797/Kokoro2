using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Physics
{
    /// <summary>
    /// Represents a single Verlet object
    /// </summary>
    public class VObject
    {
        public VEdge[] Edges;

        public VObject(VEdge[] edges)
        {
            this.Edges = edges;
        }

        public bool PhysicsEnabled { get; set; }
    }
}
