#if OPENGL
using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public class Fence : IDisposable
    {
        private IntPtr SyncObject;

        public Fence() { }

        public void Place()
        {
            SyncObject = GL.FenceSync(SyncCondition.SyncGpuCommandsComplete, WaitSyncFlags.None);
        }

        public bool IsSet()
        {
            var status = GL.ClientWaitSync(SyncObject, ClientWaitSyncFlags.SyncFlushCommandsBit, 1);
            return (status == WaitSyncStatus.AlreadySignaled) | (status == WaitSyncStatus.ConditionSatisfied);
        }

        public void Dispose()
        {
            GL.DeleteSync(SyncObject);
            SyncObject = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }

        ~Fence()
        {
            Debug.WriteLine($"[WARN] The Fence object {SyncObject} was automatically disposed");
            Dispose();
        }

    }
}
#endif