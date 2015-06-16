#if OPENAL
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenAL
{
    public class AudioSource : IDisposable
    {
        internal int ID;
        public AudioSource()
        {
            ID = AL.GenSource();
        }



        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
#endif