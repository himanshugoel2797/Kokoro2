using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.Common
{
    public abstract class StateMachineBase
    {
        public bool Initialized = false;
        public GameDevice Device;

        public virtual void Initialize(GameDevice device)
        {
            this.Device = device;
            Initialized = true;
        }

        public abstract void Update(double interval);

        public abstract void Render(double interval)

    }
}
