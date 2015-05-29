using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Physics
{
    /// <summary>
    /// Represents a single Verlet object edge constraint
    /// </summary>
    public class VEdge
    {
        public VPoint A;
        public VPoint B;

        float len;
        public float Length
        {
            get { return len; }
            set
            {
                len = value;
                LengthSquared = len * len;
            }
        }
        public float LengthSquared { get; private set; }

        float elasticity;
        public float Elasticity
        {
            get { return elasticity; }
            set
            {
                elasticity = value;
                ElasticitySquared = elasticity * elasticity;
            }
        }
        public float ElasticitySquared { get; private set; }
    }
}
