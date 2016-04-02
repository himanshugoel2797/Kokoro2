using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kokoro2.Debug
{
    internal class DebuggerManager
    {
#if DEBUG
        public static LogViewer logger = new LogViewer();
        public static MemoryMonitor monitor = new MemoryMonitor();
#endif
        public static void ShowDebugger()
        {
#if DEBUG
            var thrdA = new Thread(() =>
                {
                    logger.Active = true;
                    Application.Run(logger);
                });

            var thrdB = new Thread(() =>
            {
                monitor.Active = true;
                Application.Run(monitor);
            });

            thrdA.Start();
            thrdB.Start();
#endif
        }
    }
}
