using Kokoro2.Engine;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kokoro2.IDE
{
    static class Program
    {
        static GraphicsContext context;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            context = new GraphicsContext(new Vector2(10, 10), false);
            context.ViewportControl.Dock = System.Windows.Forms.DockStyle.Fill;

            Form1 form = new Form1(context);

            //Find the entry for the graphicsPanel
            var container = form.Controls.Find("graphicsPanel", true);
            if (container.Length != 1) throw new Exception("Failed to initialize editor. Contact the developer.");
            container[0].Controls.Add(context.ViewportControl);
            
            form.ShowDialog();
            return;
        }
    }
}
