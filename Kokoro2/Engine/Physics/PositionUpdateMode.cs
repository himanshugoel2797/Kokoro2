using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.Physics
{
    public enum PositionUpdateMode
    {
        Continuous = BEPUphysics.PositionUpdating.PositionUpdateMode.Continuous,
        Discrete = BEPUphysics.PositionUpdating.PositionUpdateMode.Discrete,
        Passive = BEPUphysics.PositionUpdating.PositionUpdateMode.Passive
    }
}
