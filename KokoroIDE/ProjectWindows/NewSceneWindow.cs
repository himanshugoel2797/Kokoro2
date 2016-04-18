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
    public partial class NewSceneWindow : Form
    {
        private List<Scene> srcs;

        public Scene Result { get; set; }

        public NewSceneWindow(List<Scene> s)
        {
            srcs = s;
            InitializeComponent();

            for (int i = 0; i < srcs.Count; i++)
            {
                comboBox1.Items.Add(srcs[i]);
            }
        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("You must enter a name for this object.");
                return;
            }

            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Select a Parent for this Scene.");
                return;
            }

            SourceEqualityComparer srcComp = new SourceEqualityComparer();

            Scene s = new Scene()
            {
                Name = textBox1.Text
            };

            s.Parent = srcs.IndexOf((Scene)comboBox1.SelectedItem);
            ((Scene)comboBox1.SelectedItem).ChildScenes.Add(srcs.Count);

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
