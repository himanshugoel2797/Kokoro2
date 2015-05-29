using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akane.Physics
{
    public abstract class BaseCollisionSolver<T>
    {
        public virtual bool SolveOptimized(T a, T b)
        {
            return Solve(a, b);
        }

        public abstract bool Solve(T a, T b);
    }
}
