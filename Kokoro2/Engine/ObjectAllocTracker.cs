using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public static class ObjectAllocTracker
    {
        public static void NewCreated(IEngineObject a, string msg = "")
        {
            ErrorLogger.AddMessage(a.ID, "Object Created:" + a.GetType().Name + "\n\t" + msg, DebugType.Other, Severity.Notification);
        }

        public static void ObjectDestroyed(IEngineObject a)
        {
            ErrorLogger.AddMessage(a.ID, "Object Destroyed:" + a.GetType().Name, DebugType.Other, Severity.Notification);
        }
    }
}
