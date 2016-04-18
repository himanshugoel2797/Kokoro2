using Kokoro.IDE.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kokoro.IDE.ProjectWindows
{
    public partial class NewLightWindow : Form
    {
        private List<LightSource> srcs;

        public LightSource Result { get; set; }

        public NewLightWindow(List<LightSource> s)
        {
            srcs = s;
            InitializeComponent();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("You must enter a name for this object.");
                return;
            }

            SourceEqualityComparer srcComp = new SourceEqualityComparer();

            LightSource s = new LightSource()
            {
                Name = textBox1.Text
            };

            if (srcs.Contains(s, srcComp))
            {
                MessageBox.Show("Object name must be unique.");
                return;
            }

            Result = s;
            DialogResult = DialogResult.OK;
        }
    }
}
