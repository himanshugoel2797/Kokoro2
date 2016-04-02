using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro.IDE.Extras
{
    public class FixedLengthQueue<T> : Queue<T>
    {
        public int SizeLimit { get; private set; }

        public FixedLengthQueue(int sizeLimit) : base()
        {
            SizeLimit = sizeLimit;
        }

        public new void Enqueue(T a)
        {
            if (this.Count >= SizeLimit) this.Dequeue();
            base.Enqueue(a);
        }
    }
}
