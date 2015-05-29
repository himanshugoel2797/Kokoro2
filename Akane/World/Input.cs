using Kokoro2.Engine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akane.World
{
    [Flags]
    public enum InputKeys
    {
        Empty = 0,
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        Enter = 1 << 4,
        Back = 1 << 5,
        Start = 1 << 6,
        Select = 1 << 7,
        Square = 1 << 8,
        Triangle = 1 << 9
    }

    public static class Input
    {
        public static InputKeys KeysPressed { get; internal set; }

        internal static void Update()
        {
            //Update the Input data
            //TODO Load strings from config
            InputKeys keys = InputKeys.Empty;

            if (Keyboard.IsKeyPressed(Key.Up)) keys |= InputKeys.Up;
            if (Keyboard.IsKeyPressed(Key.Down)) keys |= InputKeys.Down;
            if (Keyboard.IsKeyPressed(Key.Left)) keys |= InputKeys.Left;
            if (Keyboard.IsKeyPressed(Key.Right)) keys |= InputKeys.Right;
            if (Keyboard.IsKeyPressed(Key.Enter)) keys |= InputKeys.Enter;
            if (Keyboard.IsKeyPressed(Key.BackSpace)) keys |= InputKeys.Back;
            if (Keyboard.IsKeyPressed(Key.Escape)) keys |= InputKeys.Start;
            if (Keyboard.IsKeyPressed(Key.Space)) keys |= InputKeys.Select;
            if (Keyboard.IsKeyPressed(Key.Q)) keys |= InputKeys.Square;
            if (Keyboard.IsKeyPressed(Key.W)) keys |= InputKeys.Triangle;

            if (keys.ToString() != InputKeys.Empty.ToString()) keys &= ~InputKeys.Empty;
            KeysPressed = keys;
        }
    }
}
