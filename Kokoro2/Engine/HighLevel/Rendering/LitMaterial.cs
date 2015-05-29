using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Rendering
{
    public class LitMaterial : Material
    {
        public Texture SmoothnessMap { get; set; }
        public Texture MetalnessMap { get; set; }

        public override void Apply(GraphicsContext context, Model m)
        {
            if (SmoothnessMap != null) Shader["SmoothnessMap"] = SmoothnessMap;
            if (MetalnessMap != null) Shader["MetalnessMap"] = MetalnessMap;
            base.Apply(context, m);
        }
    }
}
