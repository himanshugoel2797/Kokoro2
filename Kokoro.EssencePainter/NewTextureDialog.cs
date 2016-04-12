using Kokoro.EssencePainter.Editor;
using Kokoro2.Engine;
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

namespace Kokoro.EssencePainter
{
    public partial class NewTextureDialog : Form
    {
        public TextureSet TextureSet { get; set; }

        public NewTextureDialog()
        {
            InitializeComponent();
        }

        private void saveLocBtn_Click(object sender, EventArgs e)
        {
            var res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                textureFilePath.Text = saveFileDialog1.FileName;
            }
        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(texName.Text))
            {
                MessageBox.Show("Enter a Texture Set Name");
                return;
            }

            if (string.IsNullOrWhiteSpace(textureFilePath.Text))
            {
                MessageBox.Show("Choose a save location and file name");
            }

            bool aMap = albedoMapEnabledBox.Checked;
            bool roMap = roughnessMapEnabledBox.Checked;
            bool reMap = reflectivityMapEnabledBox.Checked;
            bool nMap = normalMapEnabledBox.Checked;
            bool eMap = emissionMapEnabledBox.Checked;

            if (!aMap && !roMap && !reMap && !nMap && !eMap)
            {
                MessageBox.Show("Select atleast one Texture type");
                return;
            }

            TextureSet = new TextureSet()
            {
                Name = texName.Text,
                File = textureFilePath.Text,
                Width = (int)widthSelector.Value,
                Height = (int)heightSelector.Value,
                AlbedoMap = aMap,
                RoughnessMap = roMap,
                ReflectivityMap = reMap,
                NormalMap = nMap,
                EmissiveMap = eMap,
                AlbedoMapFile = aMap ? Path.Combine(Path.GetDirectoryName(textureFilePath.Text), Path.GetFileNameWithoutExtension(textureFilePath.Text) + "_albedo.png") : "",
                RoughnessMapFile = roMap ? Path.Combine(Path.GetDirectoryName(textureFilePath.Text), Path.GetFileNameWithoutExtension(textureFilePath.Text) + "_roughness.png") : "",
                ReflectivityMapFile = reMap ? Path.Combine(Path.GetDirectoryName(textureFilePath.Text), Path.GetFileNameWithoutExtension(textureFilePath.Text) + "_reflectivity.png") : "",
                NormalMapFile = nMap ? Path.Combine(Path.GetDirectoryName(textureFilePath.Text), Path.GetFileNameWithoutExtension(textureFilePath.Text) + "_normal.png") : "",
                EmissiveMapFile = eMap ? Path.Combine(Path.GetDirectoryName(textureFilePath.Text), Path.GetFileNameWithoutExtension(textureFilePath.Text) + "_emissive.png") : ""
            };

            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
