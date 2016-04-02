using Kokoro.IDE.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kokoro.IDE.ProjectWindows
{
    public partial class NewProjectWindow : Form
    {
        public ProjectInfo NewProjectInfo { get; set; }

        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = projectDirSelector.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                projDirTextBox.Text = projectDirSelector.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(projNameTextBox.Text))
            {
                MessageBox.Show("Enter a Project Name first.");
                return;
            }

            if (string.IsNullOrWhiteSpace(projDirTextBox.Text))
            {
                MessageBox.Show("Choose a directory to save the project in.");
                return;
            }

            if (!Directory.Exists(projDirTextBox.Text))
            {
                MessageBox.Show("Non-existent path, select a valid location to save the project.");
                return;
            }

            DialogResult = DialogResult.OK;

            //Create the new project description
            NewProjectInfo = new ProjectInfo();
            NewProjectInfo.Name = projDirTextBox.Text;
            NewProjectInfo.Description = projDescBox.Text;
            NewProjectInfo.SaveDir = projDirTextBox.Text;
            NewProjectInfo.SaveProject();   //Save the project
        }
    }
}
