using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Math;

namespace Kokoro2.Engine.HighLevel.Lights
{
    public class PhysicalDirectionalLight : DirectionalLight
    {


        private float Temperature;
        public float LightTemperature
        {
            get
            {
                return Temperature;
            }
            set
            {
                Temperature = value;
                LightColor = ColorTools.temperatureToColor(Temperature);
            }
        }

        public PhysicalDirectionalLight(GraphicsContext context, Vector3 direction) : base(context, direction)
        {

        }
    }
}
