using Kokoro.ContentPipeline;
using Kokoro.EssencePainter.Editor;
using Kokoro2.Engine;
using Kokoro2.Engine.SceneGraph;
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
using System.Xml.Serialization;

namespace Kokoro.EssencePainter
{
    public partial class Form1 : Form
    {
        private Stack<PaintAction> operations;
        private Stack<PaintAction> undone;
        private Dictionary<string, TextureSet> materials;
        private TextureSet currentMaterial;

        private GraphicsContext context;
        private SceneManager iSceneManager;     //Internal scene manager
        private Editor.EditorScene editorManager;

        private string activeModelFilePath;

        public Form1(GraphicsContext c)
        {
            InitializeComponent();

            operations = new Stack<PaintAction>();
            undone = new Stack<PaintAction>();
            materials = new Dictionary<string, TextureSet>();

            #region Scene Initialization
            context = c;
            context.FocusPollHandler += (context) => { context.IsFocused = context.ViewportControl.ContainsFocus; };
            iSceneManager = new SceneManager(c);

            editorManager = new Editor.EditorScene();
            iSceneManager.Add(nameof(editorManager), editorManager);

            iSceneManager.Activate(nameof(editorManager));
            #endregion
        }

        private void ApplyAction(PaintAction p)
        {
            operations.Push(p);
            undone.Clear();
        }

        private void RedoAction()
        {
            if (undone.Count > 0)
            {
                var op = undone.Pop();
                operations.Push(op);


            }
        }

        private void UndoTopAction()
        {
            if (operations.Count > 0)
            {
                var op = operations.Pop();
                undone.Push(op);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            iSceneManager.Register(context);
            context.Start(16, 16);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = openModelDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                activeModelFilePath = openModelDialog.FileName;
                if (Path.GetExtension(activeModelFilePath) != ".ko")
                {
                    //Run the file through the model converter
                    var conv = ModelConvert.Process(activeModelFilePath);

                    //Show a save file dialog to save the converted .ko file
                    result = saveModelDialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        File.WriteAllBytes(saveModelDialog.FileName, conv);
                        activeModelFilePath = saveModelDialog.FileName;
                    }
                    else return;
                }

                editorManager.LoadMesh(activeModelFilePath);
            }
        }

        private void newTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTextureDialog newTex = new NewTextureDialog();
            var re = newTex.ShowDialog();
            if (re == DialogResult.OK)
            {
                AddNewTextureSet(newTex.TextureSet);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Exit?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                e.Cancel = false;
                context.Stop();
                context.Dispose();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void openTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var re = openTextureSetDialog.ShowDialog();
            if (re == DialogResult.OK)
            {
                AddExistingTextureSet(openTextureSetDialog.FileName);
            }
        }

        private void texturesTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (texturesTree.SelectedNode == texturesTree.TopNode)
            {
                var tSet = ((TextureSet)texturesTree.SelectedNode.Tag);

                activeSet = tSet.Name;
                editorManager.LoadTextureSet((TextureSet)texturesTree.SelectedNode.Tag);

                activeTexturePieceList.Items.Clear();
                if (tSet.NormalMap) activeTexturePieceList.Items.Add("Normal Map", CheckState.Checked);
                if (tSet.ReflectivityMap) activeTexturePieceList.Items.Add("Reflectivity Map", CheckState.Checked);
                if (tSet.RoughnessMap) activeTexturePieceList.Items.Add("Roughness Map", CheckState.Checked);
                if (tSet.AlbedoMap) activeTexturePieceList.Items.Add("Albedo Map", CheckState.Checked);
                if (tSet.EmissiveMap) activeTexturePieceList.Items.Add("Emissive Map", CheckState.Checked);
            }
        }

        private string LoadTex(OpenFileDialog d, Button b)
        {
            var result = d.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!textures.ContainsKey(d.FileName)) textures[d.FileName] = new Bitmap(d.FileName);
                b.BackgroundImage = textures[d.FileName];
                return d.FileName;
            }
            return null;
        }

        private void normMapBtn_Click(object sender, EventArgs e)
        {
            string p = LoadTex(normalMapLoadDialog, normMapBtn); if (p != null)
            {
                currentMaterial.NormalMapFile = p;
                currentMaterial.NormalMap = true;
            }
        }

        private void albedoMapBtn_Click(object sender, EventArgs e)
        {
            string p = LoadTex(albedoMapLoadDialog, albedoMapBtn);
            if (p != null)
            {
                currentMaterial.AlbedoMapFile = p;
                currentMaterial.AlbedoMap = true;
            }
        }

        private void roughnessMapBtn_Click(object sender, EventArgs e)
        {
            string p = LoadTex(roughnessMapLoadDialog, roughnessMapBtn); if (p != null)
            {
                currentMaterial.RoughnessMapFile = p;
                currentMaterial.RoughnessMap = true;
            }
        }

        private void reflectivityMapBtn_Click(object sender, EventArgs e)
        {
            string p = LoadTex(reflectivityMapLoadDialog, reflectivityMapBtn);
            if (p != null)
            {
                currentMaterial.ReflectivityMapFile = p;
                currentMaterial.ReflectivityMap = true;
            }
        }

        private void emissiveMapBtn_Click(object sender, EventArgs e)
        {
            string p = LoadTex(emissiveMapLoadDialog, emissiveMapBtn);
            if (p != null)
            {
                currentMaterial.EmissiveMapFile = p;
                currentMaterial.EmissiveMap = true;
            }
        }

        private void bgColorButton_Click(object sender, EventArgs e)
        {
            var result = backColorSelect.ShowDialog();
            if (result == DialogResult.OK)
            {
                bgColorButton.ForeColor = bgColorButton.BackColor = backColorSelect.Color;
            }
        }

        private void fgColorButton_Click(object sender, EventArgs e)
        {
            var result = foreColorSelect.ShowDialog();
            if (result == DialogResult.OK)
            {
                fgColorButton.ForeColor = fgColorButton.BackColor = foreColorSelect.Color;
            }
        }

        private void redoBtn_Click(object sender, EventArgs e)
        {
            RedoAction();
        }

        private void undoBtn_Click(object sender, EventArgs e)
        {
            UndoTopAction();
        }

        private void openMaterialBtn_Click(object sender, EventArgs e)
        {
            var res = openMaterialDialog.ShowDialog();

            if (res == DialogResult.OK)
            {
                XmlSerializer s = new XmlSerializer(typeof(TextureSet));
                TextureSet t;

                using (Stream strm = File.OpenRead(openMaterialDialog.FileName)) t = (TextureSet)s.Deserialize(strm);

                materials[t.Name] = t;
                materialList.Items.Add(t.Name);

                LoadTextureSetToDatabase(t);
            }

        }

        private void materialList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (materialList.SelectedItems.Count > 0)
            {
                currentMaterial = materials[materialList.SelectedItems[0].Text];
                materialNameBox.Text = currentMaterial.Name;
                normMapBtn.BackgroundImage = textures[currentMaterial.NormalMapFile];
                albedoMapBtn.BackgroundImage = textures[currentMaterial.AlbedoMapFile];
                reflectivityMapBtn.BackgroundImage = textures[currentMaterial.ReflectivityMapFile];
                roughnessMapBtn.BackgroundImage = textures[currentMaterial.RoughnessMapFile];
                emissiveMapBtn.BackgroundImage = textures[currentMaterial.EmissiveMapFile];
            }
        }

        private void removeMaterial_Click(object sender, EventArgs e)
        {
            if (materialList.SelectedItems.Count > 0)
            {
                for (int i = 0; i < materialList.SelectedItems.Count; i++)
                {
                    materials.Remove(materialList.SelectedItems[i].Text);
                    materialList.Items.RemoveAt(materialList.SelectedIndices[i]);
                }
            }
        }

        private void saveMatBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(currentMaterial.File))
            {
                SaveTextureSet(currentMaterial);
            }
            else saveAsMatBtn.PerformClick();
        }

        private void saveAsMatBtn_Click(object sender, EventArgs e)
        {
            var res = saveMaterialDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(materialNameBox.Text))
                {
                    MessageBox.Show("Enter a name for the material.");
                    return;
                }

                currentMaterial.Name = materialNameBox.Text;
                currentMaterial.File = saveMaterialDialog.FileName;

                SaveTextureSet(currentMaterial);
            }
        }

        private void activeTexturePieceList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
