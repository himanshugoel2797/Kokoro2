using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if OPENGL
#if PC
using deb = Kokoro2.OpenGL.PC.Debug;
using System.Threading;
#endif
#endif

namespace Kokoro2.Engine
{
    /// <summary>
    /// Message Type
    /// </summary>
    public enum DebugType
    {
        Error, Performance, Marker, Other, Compatibility, Undefined
    }

    /// <summary>
    /// Message Severity
    /// </summary>
    public enum Severity
    {
        High, Medium, Low, Notification
    }

    /// <summary>
    /// Logs all messages
    /// </summary>
    public class ErrorLogger
    {

        public struct DebugMessageData
        {
            public string message;
            public DebugType debType;
            public Severity severity;
        }

        public static Action<DebugMessageData> NewMessage { get; set; }
        public static Action<DebugMessageData> ErrorMessage { get; set; }


        /// <summary>
        /// Start logging
        /// </summary>
        /// <param name="showLoggerWindow">Show Logger Window?</param>
        public static void StartLogger()
        {
#if DEBUG
            deb.EnableDebug();
            deb.RegisterCallback(Callback);
#endif
        }

        private static void Callback(string message, DebugType debType, Severity severity)
        {
            DebugMessageData d = new DebugMessageData()
            {
                message = message,
                debType = debType,
                severity = severity
            };

            NewMessage?.Invoke(d);
            ErrorMessage?.Invoke(d);
        }

        /// <summary>
        /// Add a message to the log
        /// </summary>
        /// <param name="id">The object ID</param>
        /// <param name="message">Any related message</param>
        /// <param name="type">The message type</param>
        /// <param name="severity">The message severity</param>
        public static void AddMessage(ulong id, string message, DebugType type, Severity severity)
        {
#if DEBUG
            deb.InsertDebugMessage(0, id + "," + message, type, severity);
#endif
        }
    }
}