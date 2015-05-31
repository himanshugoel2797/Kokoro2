using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Rendering
{
    public struct LightSource
    {
        public Vector3 Position;
        public Vector3 Color;
        public Entity Parent;       //The light only affects the objects under the same parent
        public float Radius;        //The radius of the light for the GI system to process, this is not the radius of influence of the light
        public double Intensity;    //Store the intensity in lumens
    }
}
