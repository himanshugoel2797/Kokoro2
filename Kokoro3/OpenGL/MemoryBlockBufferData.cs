
#if OPENGL


using System;
using System.Runtime.InteropServices;
using System.Linq;

namespace Kokoro3.OpenGL
{
    public partial class MemoryBlock
    {
        public void BufferData(Int16[] data, uint len)
        {

            Marshal.Copy(data, 0, Location, (int)len);
        }
        public void BufferData(Int32[] data, uint len)
        {

            Marshal.Copy(data, 0, Location, (int)len);
        }
        public void BufferData(Int64[] data, uint len)
        {

            Marshal.Copy(data, 0, Location, (int)len);
        }
        public void BufferData(UInt16[] data, uint len)
        {
            var tmp = data.SelectMany(BitConverter.GetBytes).ToArray();
            Marshal.Copy(tmp, 0, Location, (int)len);
        }
        public void BufferData(UInt32[] data, uint len)
        {
            var tmp = data.SelectMany(BitConverter.GetBytes).ToArray();
            Marshal.Copy(tmp, 0, Location, (int)len);
        }
        public void BufferData(UInt64[] data, uint len)
        {
            var tmp = data.SelectMany(BitConverter.GetBytes).ToArray();
            Marshal.Copy(tmp, 0, Location, (int)len);
        }
        public void BufferData(Byte[] data, uint len)
        {

            Marshal.Copy(data, 0, Location, (int)len);
        }
        public void BufferData(Single[] data, uint len)
        {

            Marshal.Copy(data, 0, Location, (int)len);
        }
    }
}
#endif