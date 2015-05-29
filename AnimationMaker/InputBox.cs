using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimationMaker
{
    public partial class InputBox : Form
    {
        public string AnimationName { get; set; }

        public InputBox()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text)) MessageBox.Show("The animation name can not be empty!");
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                AnimationName = textBox1.Text;
                this.Close();
            }
        }
    }
}
