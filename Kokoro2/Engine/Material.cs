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
        public Texture ColorMap { get; set; }
        public Texture LightingMap { get; set; }
        public Texture NormalMap { get; set; }
        public Texture EmissiveMap { get; set; }
        public ShaderProgram Shader { get; set; }

        public virtual void Apply(GraphicsContext context, Model m)
        {
            Shader["World"] = m.World;
            Shader["View"] = context.View;
            Shader["Projection"] = context.Projection;
            Shader["ZNear"] = context.ZNear;
            Shader["ZFar"] = context.ZFar;

            if (ColorMap != null) Shader["ColorMap"] = ColorMap;
            if (LightingMap != null) Shader["LightingMap"] = LightingMap;
            if (NormalMap != null) Shader["NormalMap"] = NormalMap;
            if (EmissiveMap != null) Shader["EmissiveMap"] = EmissiveMap;

            Shader.Apply(context);
        }

        public virtual void Cleanup(GraphicsContext context, Model m)
        {
            Shader.Cleanup(context);
        }

    }
}
