
#if OPENGL


using System;

namespace Kokoro3.OpenGL{
public partial class MemoryBlock {
            public static void BufferData (Int16[] data, uint len) {
            unsafe
			{
				fixed (Int16* SystemMemory = &data[0])
                    {
                        Marshal.Copy(data, 0, Location, len);
                    }
			}
        }
            public static void BufferData (Int32[] data, uint len) {
            unsafe
			{
				fixed (Int32* SystemMemory = &data[0])
                    {
                        Marshal.Copy(data, 0, Location, len);
                    }
			}
        }
            public static void BufferData (Int64[] data, uint len) {
            unsafe
			{
				fixed (Int64* SystemMemory = &data[0])
                    {
                        Marshal.Copy(data, 0, Location, len);
                    }
			}
        }
            public static void BufferData (UInt16[] data, uint len) {
            unsafe
			{
				fixed (UInt16* SystemMemory = &data[0])
                    {
                        Marshal.Copy(data, 0, Location, len);
                    }
			}
        }
            public static void BufferData (UInt32[] data, uint len) {
            unsafe
			{
				fixed (UInt32* SystemMemory = &data[0])
                    {
                        Marshal.Copy(data, 0, Location, len);
                    }
			}
        }
            public static void BufferData (UInt64[] data, uint len) {
            unsafe
			{
				fixed (UInt64* SystemMemory = &data[0])
                    {
                        Marshal.Copy(data, 0, Location, len);
                    }
			}
        }
            public static void BufferData (Byte[] data, uint len) {
            unsafe
			{
				fixed (Byte* SystemMemory = &data[0])
                    {
                        Marshal.Copy(data, 0, Location, len);
                    }
			}
        }
            public static void BufferData (Single[] data, uint len) {
            unsafe
			{
				fixed (Single* SystemMemory = &data[0])
                    {
                        Marshal.Copy(data, 0, Location, len);
                    }
			}
        }
    }
}
#endif