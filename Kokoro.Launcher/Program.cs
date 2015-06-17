using Kokoro3.Common;
using Kokoro3.Engine;
using Kokoro3.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro.Launcher
{
    class Program
    {
        static GameDevice context;
        static void Main(string[] args)
        {
            Form1 form = new Form1();
            context = new GameDevice();
            context.GraphicsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(context.GraphicsControl);
            form.ClientSize = new System.Drawing.Size(960, 540);

            Kokoro.Game.Game game = new Kokoro.Game.Game(context);
            form.ShowDialog();
            return;
        }
    }
}
