using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Kokoro2.Math;
using Kokoro2.Engine.Shaders;

namespace Kokoro2.Engine
{
    public class Material
    {
        public string Name { get; set; }
        public Texture AlbedoMap { get; set; }
        public Texture DerivativeAOCavityMicrosurfaceMap { get; set; }  //Derivative map in R, AO in G, cavity in B, microsurface in A
        public Texture ReflectivityMap { get; set; }
        public ShaderProgram Shader { get; set; }

        public virtual void Apply(GraphicsContext context, Model m)
        {
            Shader["World"] = m.World;
            Shader["View"] = context.View;
            Shader["Projection"] = context.Projection;
            Shader["ZNear"] = context.ZNear;
            Shader["ZFar"] = context.ZFar;
            Shader["EyePos"] = context.Camera.Position;
            Shader["EyeDir"] = context.Camera.Direction;

            if (AlbedoMap != null) Shader["AlbedoMap"] = AlbedoMap;
            if (DerivativeAOCavityMicrosurfaceMap != null) Shader["DerivativeAOCavityMicrosurfaceMap"] = DerivativeAOCavityMicrosurfaceMap;
            if (ReflectivityMap != null) Shader["ReflectivityMap"] = ReflectivityMap;

            Shader.Apply(context);
        }

        public virtual void Cleanup(GraphicsContext context, Model m)
        {
            Shader.Cleanup(context);
        }

    }
}
