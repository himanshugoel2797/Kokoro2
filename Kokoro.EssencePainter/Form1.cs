using Kokoro.ContentPipeline;
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

namespace Kokoro.EssencePainter
{
    public partial class Form1 : Form
    {
        private GraphicsContext context;
        private SceneManager iSceneManager;     //Internal scene manager
        private Editor.EditorScene editorManager;

        private string activeModelFilePath;

        public Form1(GraphicsContext c)
        {
            InitializeComponent();

            #region Scene Initialization
            context = c;
            context.FocusPollHandler += (context) => { context.IsFocused = context.ViewportControl.ContainsFocus; };
            iSceneManager = new SceneManager(c);

            editorManager = new Editor.EditorScene();
            iSceneManager.Add(nameof(editorManager), editorManager);

            iSceneManager.Activate(nameof(editorManager));
            #endregion
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

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            context.Stop();
            context.Dispose();
        }
    }
}
