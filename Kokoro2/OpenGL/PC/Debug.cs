#if OPENGL

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.OpenGL.PC
{
    /// <summary>
    /// Exposes Debugging related functions to the engine runtime
    /// </summary>
    public static class Debug
    {
        public static void EnableDebug()
        {
#if DEBUG
            GL.Enable(EnableCap.DebugOutput);
#endif
        }

        static Bitmap bmp;
        static bool texRetrieved = false;
        internal static void DLbmp(int id)
        {
#if DEBUG
            GL.BindTexture(TextureTarget.Texture2D, id);
            int width, height;
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out width);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out height);
            bmp = new Bitmap(width, height);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.GetTexImage(TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            texRetrieved = true;
#endif
        }

#if DEBUG
        public static Bitmap TexToBMP(int id)
        {
            GraphicsContextLL.RequestTexture(id);
            while (!texRetrieved) { }
            texRetrieved = false;
            return bmp;
        }
#endif

        public static void InsertDebugMessage(int id, string message, Kokoro2.Debug.DebugType dt, Kokoro2.Debug.Severity severity)
        {
#if DEBUG
            //DebugType converter
            DebugType type = DebugType.DebugTypeOther;
            if (dt == Kokoro2.Debug.DebugType.Compatibility) type = DebugType.DebugTypePortability;
            else if (dt == Kokoro2.Debug.DebugType.Error) type = DebugType.DebugTypeError;
            else if (dt == Kokoro2.Debug.DebugType.Marker) type = DebugType.DebugTypeMarker;
            else if (dt == Kokoro2.Debug.DebugType.Other) type = DebugType.DebugTypeOther;
            else if (dt == Kokoro2.Debug.DebugType.Performance) type = DebugType.DebugTypePerformance;

            //DebugSeverity converter
            DebugSeverity severeness = DebugSeverity.DebugSeverityNotification;
            if (severity == Kokoro2.Debug.Severity.High) severeness = DebugSeverity.DebugSeverityHigh;
            else if (severity == Kokoro2.Debug.Severity.Low) severeness = DebugSeverity.DebugSeverityLow;
            else if (severity == Kokoro2.Debug.Severity.Medium) severeness = DebugSeverity.DebugSeverityMedium;
            else if (severity == Kokoro2.Debug.Severity.Notification) severeness = DebugSeverity.DebugSeverityNotification;

            DebugCallback(DebugSource.DebugSourceApplication, type, id, severeness, message.Length, Marshal.StringToHGlobalAnsi(message), IntPtr.Zero);
            //GL.DebugMessageInsert(DebugSourceExternal.DebugSourceApplication, type, id, severeness, message.Length, message);
#endif
        }

        internal static DebugProc proc;
        static Action<string, Kokoro2.Debug.DebugType, Kokoro2.Debug.Severity> actionCallback;
        public static void DebugCallback(DebugSource src, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr usrData)
        {
#if DEBUG
            string msg = "";
            if (src == DebugSource.DebugSourceApi) msg += "[API]";
            else if (src == DebugSource.DebugSourceApplication) msg += "[Application]";
            else if (src == DebugSource.DebugSourceOther) msg += "[Other]";
            else if (src == DebugSource.DebugSourceShaderCompiler) msg += "[Shader Compiler]";
            else if (src == DebugSource.DebugSourceThirdParty) msg += "[Third Party]";
            else if (src == DebugSource.DebugSourceWindowSystem) msg += "[Window System]";

            if (type == DebugType.DebugTypeDeprecatedBehavior) msg += "[Deprecated]";
            else if (type == DebugType.DebugTypeError) msg += "[Error]";
            else if (type == DebugType.DebugTypeMarker) msg += "[Marker]";
            else if (type == DebugType.DebugTypeOther) msg += "[Other]";
            else if (type == DebugType.DebugTypePerformance) msg += "[Performance]";
            else if (type == DebugType.DebugTypePortability) msg += "[Portability]";
            else if (type == DebugType.DebugTypeUndefinedBehavior) msg += "[Undefined]";

            if (severity == DebugSeverity.DebugSeverityHigh) msg += "[High]";
            else if (severity == DebugSeverity.DebugSeverityLow) msg += "[Low]";
            else if (severity == DebugSeverity.DebugSeverityMedium) msg += "[Medium]";
            else if (severity == DebugSeverity.DebugSeverityNotification) msg += "[Notification]";

            msg += "[ID = " + id + "]";

            if (message != IntPtr.Zero) msg += System.Runtime.InteropServices.Marshal.PtrToStringAnsi(message, length);

            actionCallback(msg, EnumConverters.ODebugType(type), (Kokoro2.Debug.Severity)Enum.Parse(typeof(Kokoro2.Debug.Severity), severity.ToString().Replace("DebugSeverity", "")));
#endif
        }
        public static void RegisterCallback(Action<string, Kokoro2.Debug.DebugType, Kokoro2.Debug.Severity> callback)
        {
#if DEBUG
            actionCallback = callback;
            proc = DebugCallback;
            GL.DebugMessageCallback(proc, IntPtr.Zero);
#endif
        }



    }
}

#endif