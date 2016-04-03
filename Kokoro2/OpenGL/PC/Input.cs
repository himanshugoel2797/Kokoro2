#if OPENGL && PC

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Input;
using Kokoro2.Math;

namespace Kokoro2.OpenGL.PC
{
    public static class InputLL
    {
        static InputLL()
        {
            locker = new object();
            msLocker = new object();
        }

        static bool foc;
        public static Vector2 xy;
        public static Vector2 dim;
        internal static void IsFocused(bool focused)
        {
            foc = focused;
            if (focused)
            {
                scrollOffset = msState.WheelPrecise - prevScrollVal;
            }
        }
        internal static void SetWinXY(int x, int y, int width, int height)
        {
            xy = new Vector2(x, y);
            dim = new Vector2(width, height);
        }

        #region Keyboard
        static KeyboardState kbdState;
        static object locker;
        public static void UpdateKeyboard()
        {
            lock (locker)
            {
                kbdState = Keyboard.GetState();
            }
        }

        public static bool KeyDown(Engine.Input.Key k)
        {
            if (!foc) return false;
            lock (locker)
            {
                return kbdState[EnumConverters.EKey(k)];
            }
        }
        #endregion

        #region Mouse
        static float prevScrollVal = 0;
        static float scrollOffset = 0;
        static MouseState msState;
        static object msLocker;
        public static Vector2 UpdateMouse()
        {
            lock (msLocker)
            {
                msState = Mouse.GetCursorState();
                return new Vector2(msState.X - xy.X, msState.Y - xy.Y);
            }
        }
        public static bool LeftMouseButtonDown()
        {
            if (!foc) return false; lock (msLocker) { return msState.IsButtonDown(MouseButton.Left); }
        }
        public static bool RightMouseButtonDown()
        {
            if (!foc) return false; lock (msLocker) { return msState.IsButtonDown(MouseButton.Right); }
        }
        public static bool MiddleMouseButtonDown()
        {
            if (!foc) return false; lock (msLocker) { return msState.IsButtonDown(MouseButton.Middle); }
        }

        public static bool LeftMouseButtonUp()
        {
            if (!foc) return true; lock (msLocker) { return msState.IsButtonUp(MouseButton.Left); }
        }
        public static bool RightMouseButtonUp()
        {
            if (!foc) return true; lock (msLocker) { return msState.IsButtonUp(MouseButton.Right); }
        }
        public static bool MiddleMouseButtonUp()
        {
            if (!foc) return true; lock (msLocker) { return msState.IsButtonUp(MouseButton.Middle); }
        }

        public static float GetScroll()
        {
            if (!foc) return prevScrollVal; lock (msLocker) { prevScrollVal = msState.WheelPrecise - scrollOffset; return msState.WheelPrecise - scrollOffset; }
        }
        public static void SetMousePos(Vector2 pos) { Mouse.SetPosition(pos.X, pos.Y); }

        public static Vector2 GetNDMousePos(Vector2 mousePos)
        {
            return new Vector2((mousePos.X - xy.X) / dim.X, (mousePos.Y - xy.Y) / dim.Y);
        }
        #endregion
    }
}

#endif