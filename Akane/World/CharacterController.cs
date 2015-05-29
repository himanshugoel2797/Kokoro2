using Akane.Graphics;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akane.World
{
    public class CharacterController
    {
        public Map map;
        public Sprite Sprite;
        public int Speed = 5;
        public int AnimationInterval = 1;

        public CharacterController(Sprite s, Map m)
        {
            Sprite = s;
            map = m;
        }

        InputKeys prev = new InputKeys();
        public void Update(AkaneManager manager)
        {
            Vector2 pos = Sprite.Position;
            if (Input.KeysPressed.HasFlag(InputKeys.Left) && !prev.HasFlag(InputKeys.Left))
            {
                Sprite.SetAnimation("left_walk", AnimationInterval, 1);
            }
            if (Input.KeysPressed.HasFlag(InputKeys.Right) && !prev.HasFlag(InputKeys.Right))
            {
                Sprite.SetAnimation("right_walk", AnimationInterval, 1);
            }
            if (Input.KeysPressed.HasFlag(InputKeys.Up) && !prev.HasFlag(InputKeys.Up))
            {
                Sprite.SetAnimation("up_walk", AnimationInterval, 1);
            }
            if (Input.KeysPressed.HasFlag(InputKeys.Down) && !prev.HasFlag(InputKeys.Down))
            {
                Sprite.SetAnimation("down_walk", AnimationInterval, 1);
            }

            //TODO remove collision detection code from here into the physics engine

            if (Input.KeysPressed.HasFlag(InputKeys.Left))// && !map.Collidable((int)pos.X - Speed, (int)(pos.Y + Sprite.Size.Y / 2)))
            {
                pos.X -= Speed;
            }

            if (Input.KeysPressed.HasFlag(InputKeys.Right))// && !map.Collidable((int)(pos.X + Speed + Sprite.Size.X - 1), (int)(pos.Y + Sprite.Size.Y / 2)))
                pos.X += Speed;

            if (Input.KeysPressed.HasFlag(InputKeys.Up))// && !map.Collidable((int)(pos.X), (int)(pos.Y - Speed)))
                pos.Y -= Speed;

            if (Input.KeysPressed.HasFlag(InputKeys.Down))// && !map.Collidable((int)(pos.X), (int)(pos.Y + Speed + Sprite.Size.Y - 1)))
                pos.Y += Speed;


            if (!Input.KeysPressed.HasFlag(InputKeys.Down) && !Input.KeysPressed.HasFlag(InputKeys.Up)
                && !Input.KeysPressed.HasFlag(InputKeys.Left) && !Input.KeysPressed.HasFlag(InputKeys.Right)) Sprite.SetAnimation("standing", 4, 0);

            Sprite.Position = pos;
            Sprite.Draw(manager);

            prev = Input.KeysPressed;
        }

    }
}