using Kokoro2.Engine;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro.Launcher
{
    class Program
    {
        static GraphicsContext context;
        static void Main(string[] args)
        {
            Form1 form = new Form1();
            context = new GraphicsContext(new Vector2(10, 10));
            context.ViewportControl.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(context.ViewportControl);
            form.ClientSize = new System.Drawing.Size(960, 540);

            Kokoro2.Game.Game game = new Game.Game(context);
            form.ShowDialog();
            return;
        }
    }
}
