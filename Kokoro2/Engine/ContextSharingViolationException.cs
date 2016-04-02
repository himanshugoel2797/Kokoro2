using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public class ContextSharingViolationException : Exception
    {
        public ContextSharingViolationException() : base("This object can not be shared between contexts") { }
    }
}
