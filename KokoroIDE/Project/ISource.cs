using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro.IDE.Project
{
    public interface ISource
    {
        string Name { get; set; }
    }

    public class SourceEqualityComparer : IEqualityComparer<ISource>
    {
        public bool Equals(ISource x, ISource y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(ISource obj)
        {
            return obj.GetHashCode();
        }
    }
}
