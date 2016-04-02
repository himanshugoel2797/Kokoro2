using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public interface IEngineObject : IDisposable
    {
        ulong ID { get; set; }
        GraphicsContext ParentContext { get; set; }
    }
}
