using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.SceneGraph.Entities
{
    public class RenderableEntity : Entity
    {
        public Model Renderable;
        public bool Visible { get; set; }
        public bool UpdateController { get; set; }

        public Controller Control;

        public RenderableEntity(Model m, Controller control)
        {
            this.Renderable = m;
            this.Visible = true;
            this.UpdateController = true;
            this.Control = control;
        }

        public override void Activate(GraphicsContext context, double interval)
        {
            if (Control != null && UpdateController) Control.Update(this, context, interval);

            base.Activate(context, interval);

            Renderable.World = this.WorldTransform;
            if (Visible) Renderable.Draw(context);
        }

        public override void Update(GraphicsContext context, double interval)
        {
        }
    }
}
