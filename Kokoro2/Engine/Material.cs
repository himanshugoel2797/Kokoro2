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
        public Texture SpecularMap { get; set; }
        public Texture GlossinessMap { get; set; }
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
            Shader["Fcoef"] = 2.0f / (float)System.Math.Log(context.ZFar + 1.0, 2);

            if (AlbedoMap != null) Shader["AlbedoMap"] = AlbedoMap;
            if (GlossinessMap != null) Shader["GlossinessMap"] = GlossinessMap;
            if (SpecularMap != null) Shader["SpecularMap"] = SpecularMap;
            else if (AlbedoMap != null) Shader["SpecularMap"] = AlbedoMap;

            Shader.Apply(context);
        }

        public virtual void Cleanup(GraphicsContext context, Model m)
        {
            Shader.Cleanup(context);
        }

    }
}
