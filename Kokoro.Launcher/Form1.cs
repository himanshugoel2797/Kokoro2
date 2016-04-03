using Kokoro2.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kokoro.Launcher
{
    public partial class Form1 : Form
    {
        private GraphicsContext context;

        public Form1(GraphicsContext c)
        {
            c.FocusPollHandler += (context) => { context.IsFocused = this.ContainsFocus; };
            context = c;
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Kokoro2.Game.Game game = new Kokoro2.Game.Game(context);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            context.Stop();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
