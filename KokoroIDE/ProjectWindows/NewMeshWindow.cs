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
    public partial class NewMeshWindow : Form
    {
        private List<MeshSource> srcs;

        public MeshSource Result { get; set; }


        public NewMeshWindow(List<MeshSource> s)
        {
            srcs = s;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
