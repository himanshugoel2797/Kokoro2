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

namespace Kokoro2.Debug
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

        /// <summary>
        /// Start logging
        /// </summary>
        /// <param name="showLoggerWindow">Show Logger Window?</param>
        public static void StartLogger(bool showLoggerWindow)
        {
#if DEBUG
            deb.EnableDebug();
            deb.RegisterCallback(Callback);
#endif
        }

        private static void Callback(string message, DebugType debType, Severity severity)
        {
#if DEBUG
            if(!message.Contains("End Render Frame"))DebuggerManager.logger.NewMessage(message, debType, severity);
#endif
        }

        /// <summary>
        /// Add a message to the log
        /// </summary>
        /// <param name="id">The object ID</param>
        /// <param name="message">Any related message</param>
        /// <param name="type">The message type</param>
        /// <param name="severity">The message severity</param>
        public static void AddMessage(int id, string message, DebugType type, Severity severity)
        {
#if DEBUG
            while (DebuggerManager.logger.Pause) { }    //Block until resume
            deb.InsertDebugMessage(id, message, type, severity);
#endif
        }

    }
}
