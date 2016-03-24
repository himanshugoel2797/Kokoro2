using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGRacing
{
    interface IShipController
    {
        void Update(Ship target, Track t);
    }
}
