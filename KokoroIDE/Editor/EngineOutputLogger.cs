using Kokoro.IDE.Extras;
using Kokoro2.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro.IDE.Editor
{
    public class EngineOutputLogger
    {
        public FixedLengthQueue<ErrorLogger.DebugMessageData> messages;

        const int MaxMessageCount = 256;

        public EngineOutputLogger()
        {
            messages = new FixedLengthQueue<ErrorLogger.DebugMessageData>(MaxMessageCount);
            ErrorLogger.NewMessage += ErrorLogger_NewMessage;
        }

        private void ErrorLogger_NewMessage(ErrorLogger.DebugMessageData obj)
        {
            messages.Enqueue(obj);
        }
    }
}
