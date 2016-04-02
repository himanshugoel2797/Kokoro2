using Kokoro.IDE.Project;
using Kokoro.IDE.ProjectWindows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kokoro2.IDE
{
    public partial class Form1 : Form
    {
        private ProjectInfo CurrentProject;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProjectWindow newProjWin = new NewProjectWindow();
            var result = newProjWin.ShowDialog();
            if (result == DialogResult.OK)
            {
                CurrentProject = newProjWin.NewProjectInfo;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = openPrj.ShowDialog();
            if(result == DialogResult.OK)
            {
                CurrentProject = ProjectInfo.LoadProject(openPrj.FileName);
            }
        }
    }
}
