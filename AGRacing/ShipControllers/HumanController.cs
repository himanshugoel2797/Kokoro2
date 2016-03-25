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

            bool frontLeftFWD = t.RayCast(target.GetRayLocation(ShipRayLocations.FrontLeftSide), target.MeshFront, out fwdDist, out fwdNorm);

            if(Kokoro2.Engine.Input.Keyboard.IsKeyPressed(Kokoro2.Engine.Input.Key.Space))
            {
                target.ChangePosition(target.Position + Vector3.UnitX);
            }


            //Read controls and move ship accordingly
            //Airbrakes work by push the back side of the ship
            //Turning works by actually rotating the ship?
        }
    }
}
