using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Engine.Input;
using Kokoro2.Math;

namespace Kokoro2.Engine.HighLevel.CharacterControllers
{
    public class ThirdPersonController2D : CharacterControllerBase
    {
        public float MoveSpeed = 0.15f;
        public Vector3 Direction;
        public Vector3 Up;
        public Vector3 Distance;

        public ThirdPersonController2D(Vector3 InitialPosition)
        {
            Position = InitialPosition;
        }

        public void Update(double interval, GraphicsContext context)
        {
            interval /= 100;
            Vector3 Right = Vector3.Cross(Up, Direction);

            if (Keyboard.IsKeyPressed(Key.Up))
            {
                Position += Direction * (float)(MoveSpeed * interval / 10000f);
            }
            else if (Keyboard.IsKeyPressed(Key.Down))
            {
                Position -= Direction * (float)(MoveSpeed * interval / 10000f);
            }

            if (Keyboard.IsKeyPressed(Key.Left))
            {
                Position += Right * (float)(MoveSpeed * interval / 10000f);
            }
            else if (Keyboard.IsKeyPressed(Key.Right))
            {
                Position -= Right * (float)(MoveSpeed * interval / 10000f);
            }
#if DEBUG
            if (Keyboard.IsKeyPressed(Key.PageUp))
            {
                Position += Up * (float)(MoveSpeed * interval / 10000f);
            }
            else if (Keyboard.IsKeyPressed(Key.PageDown))
            {
                Position -= Up * (float)(MoveSpeed * interval / 10000f);
            }
#endif
        }
    }
}
