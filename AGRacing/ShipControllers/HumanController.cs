using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGRacing.ShipControllers
{
    class HumanController : IShipController
    {
        public void Update(Ship target, Track t)
        {

            //Collect information on intersection distances on all 4 sides and perform collision response
            float fwdDist;
            Vector3 fwdNorm;

            bool frontLeftFWD = t.RayCast(target.GetRayLocation(ShipRayLocations.FrontLeftSide), target.PhysicalFront, out fwdDist, out fwdNorm);

            if (Kokoro2.Engine.Input.Keyboard.IsKeyPressed(Kokoro2.Engine.Input.Key.E))
            {
                target.ChangeVelocity(target.PhysicalRight * 25f);
            }

            if (Kokoro2.Engine.Input.Keyboard.IsKeyPressed(Kokoro2.Engine.Input.Key.Q))
            {
                target.ChangeVelocity(target.PhysicalRight * -25f);
            }

            if (Kokoro2.Engine.Input.Keyboard.IsKeyPressed(Kokoro2.Engine.Input.Key.W))
            {
                target.ChangeVelocity(target.PhysicalFront * 50f);
            }

            if (Kokoro2.Engine.Input.Keyboard.IsKeyPressed(Kokoro2.Engine.Input.Key.A))
            {
                target.Rotate(Vector3.UnitY, 0.05f);
            }

            if (Kokoro2.Engine.Input.Keyboard.IsKeyPressed(Kokoro2.Engine.Input.Key.D))
            {
                target.Rotate(Vector3.UnitY, -0.05f);
            }

            //Read controls and move ship accordingly
            //Airbrakes work by push the back side of the ship
            //Turning works by actually rotating the ship?
        }
    }
}
