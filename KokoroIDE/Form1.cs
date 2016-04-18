using Kokoro.IDE;
using Kokoro.IDE.Editor;
using Kokoro.IDE.Project;
using Kokoro.IDE.ProjectWindows;
using Kokoro2.Engine;
using Kokoro2.Engine.SceneGraph;
using Kokoro2.IDE.Editor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kokoro.IDE
{
    public partial class Form1 : Form
    {
        private ProjectInfo CurrentProject;
        private EditorUIManager uiManager;
        private GraphicsContext context;
        private SceneManager iSceneManager;     //Internal scene manager
        private EngineOutputLogger logger;

        public Form1(GraphicsContext c)
        {
            InitializeComponent();

            #region Scene Initialization
            context = c;
            iSceneManager = new SceneManager(c);

            uiManager = new EditorUIManager();
            iSceneManager.Add(nameof(uiManager), uiManager);

            iSceneManager.Activate(nameof(uiManager));
            #endregion

        }

        private void AddObjectEntry(ISource src)
        {
            TreeNode n = new TreeNode(src.Name);
            n.Name = n.Text = src.Name;

            if (src.GetType() == typeof(MeshSource)) objectTreeView.Nodes["Models"].Nodes.Add(n);
            if (src.GetType() == typeof(LightSource)) objectTreeView.Nodes["Lights"].Nodes.Add(n);
            if (src.GetType() == typeof(SoundSource)) objectTreeView.Nodes["Sounds"].Nodes.Add(n);
        }

        private void AddSceneEntry(Scene s)
        {
            TreeNode n = new TreeNode(s.Name);
            n.Text = n.Name = s.Name;
            sceneTreeView.Nodes[CurrentProject.Scenes[s.Parent].Name].Nodes.Add(n);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            iSceneManager.Register(context);
            context.Start(16, 16);
            frameUpdater.Start();

            logger = new EngineOutputLogger();
            //context.Pause();
        }

        private void PerFrameInfoUpdater_Elapsed(object sender, EventArgs e)
        {
            framerateLabel.Text = "FPS: " + uiManager.RenderRate;
            updaterateLabel.Text = "UPS: " + uiManager.UpdateRate;
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProjectWindow newProjWin = new NewProjectWindow();
            var result = newProjWin.ShowDialog();
            if (result == DialogResult.OK)
            {
                CurrentProject = newProjWin.NewProjectInfo;
                CurrentProject.Scenes.Add(new Scene());
                CurrentProject.Scenes[0].Name = "Root Scene";

                objectTreeView.Nodes.Clear();
                scriptTreeView.Nodes.Clear();
                sceneTreeView.Nodes.Clear();

                TreeNode n = new TreeNode();
                n.Text = n.Name = CurrentProject.Scenes[0].Name;
                sceneTreeView.Nodes.Add(n);


                CurrentProject.SaveProject();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = openPrj.ShowDialog();
            if (result == DialogResult.OK)
            {
                CurrentProject = ProjectInfo.LoadProject(openPrj.FileName);
                TreeNode n = new TreeNode();
                n.Text = n.Name = CurrentProject.Scenes[0].Name;
                sceneTreeView.Nodes.Add(n);
                for (int i = 1; i < CurrentProject.Scenes.Count; i++)
                {
                    AddSceneEntry(CurrentProject.Scenes[i]);
                }
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

        private void showEngineOutput_Click(object sender, EventArgs e)
        {
            EngineOutputWindow engineOut = new EngineOutputWindow(logger);
            engineOut.Show();
        }

        private void textureTool_Click(object sender, EventArgs e)
        {
            Process.Start(typeof(Kokoro.EssencePainter.Form1).Assembly.Location);
        }

        private void addSoundSourceBtn_Click(object sender, EventArgs e)
        {
            if (CurrentProject == null)
            {
                MessageBox.Show("Create/Open a project");
                return;
            }

            if (CurrentProject.CurrentScene == null)
            {
                MessageBox.Show("Add/Select a new Scene");
                return;
            }

            NewSoundSourceWindow soundSrc = new NewSoundSourceWindow(CurrentProject.CurrentScene.SoundSources);
            if (soundSrc.ShowDialog() == DialogResult.OK)
            {
                CurrentProject.CurrentScene.SoundSources.Add(soundSrc.Result);
                AddObjectEntry(soundSrc.Result);
            }
        }

        private void addLight_Click(object sender, EventArgs e)
        {
            if (CurrentProject == null)
            {
                MessageBox.Show("Create/Open a project");
                return;
            }

            if (CurrentProject.CurrentScene == null)
            {
                MessageBox.Show("Add/Select a new Scene");
                return;
            }

            NewLightWindow lightSrc = new NewLightWindow(CurrentProject.CurrentScene.LightSources);
            if (lightSrc.ShowDialog() == DialogResult.OK)
            {
                CurrentProject.CurrentScene.LightSources.Add(lightSrc.Result);
                AddObjectEntry(lightSrc.Result);
            }
        }

        private void createScene_Click(object sender, EventArgs e)
        {
            if (CurrentProject == null)
            {
                MessageBox.Show("Create/Open a project");
                return;
            }

            NewSceneWindow sceneEntry = new NewSceneWindow(CurrentProject.Scenes);
            if (sceneEntry.ShowDialog() == DialogResult.OK)
            {
                CurrentProject.Scenes.Add(sceneEntry.Result);
                AddSceneEntry(sceneEntry.Result);
            }
        }

        private int s_index = -1;
        private void objectTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (objectTreeView.SelectedNode.Parent == null) return;

            ISource s = null;
            if (objectTreeView.SelectedNode.Parent.Name == "Models")
            {
                s = CurrentProject.CurrentScene.MeshSources.Find(a => a.Name == objectTreeView.SelectedNode.Name);
                s_index = CurrentProject.CurrentScene.MeshSources.IndexOf((MeshSource)s);
            }
            else if (objectTreeView.SelectedNode.Parent.Name == "Lights")
            {
                s = CurrentProject.CurrentScene.LightSources.Find(a => a.Name == objectTreeView.SelectedNode.Name);
                s_index = CurrentProject.CurrentScene.LightSources.IndexOf((LightSource)s);
            }
            else if (objectTreeView.SelectedNode.Parent.Name == "Sounds")
            {
                s = CurrentProject.CurrentScene.SoundSources.Find(a => a.Name == objectTreeView.SelectedNode.Name);
                s_index = CurrentProject.CurrentScene.SoundSources.IndexOf((SoundSource)s);
            }

            curObjProperties.SelectedObject = s;
        }

        private void sceneTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CurrentProject.CurrentScene = CurrentProject.Scenes.Find(a => a.Name == sceneTreeView.SelectedNode.Name);

            objectTreeView.Nodes.Clear();
            scriptTreeView.Nodes.Clear();

            TreeNode models = new TreeNode("Models");
            TreeNode lights = new TreeNode("Lights");
            TreeNode sounds = new TreeNode("Sounds");

            models.Name = models.Text = "Models";
            lights.Name = lights.Text = "Lights";
            sounds.Name = sounds.Text = "Sounds";

            objectTreeView.Nodes.Add(models);
            objectTreeView.Nodes.Add(lights);
            objectTreeView.Nodes.Add(sounds);

            for (int i = 0; i < CurrentProject.CurrentScene.MeshSources.Count; i++) AddObjectEntry(CurrentProject.CurrentScene.MeshSources[i]);
            for (int i = 0; i < CurrentProject.CurrentScene.LightSources.Count; i++) AddObjectEntry(CurrentProject.CurrentScene.LightSources[i]);
            for (int i = 0; i < CurrentProject.CurrentScene.SoundSources.Count; i++) AddObjectEntry(CurrentProject.CurrentScene.SoundSources[i]);

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tmp = CurrentProject.CurrentScene;
            CurrentProject.CurrentScene = null;
            CurrentProject?.SaveProject();
            CurrentProject.CurrentScene = tmp;
        }

        private void curObjProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (objectTreeView.SelectedNode.Parent == null) return;



            if (objectTreeView.SelectedNode.Parent.Name == "Models")
            {
                CurrentProject.CurrentScene.MeshSources[s_index] = (MeshSource)curObjProperties.SelectedObject;
            }
            else if (objectTreeView.SelectedNode.Parent.Name == "Lights")
            {
                CurrentProject.CurrentScene.LightSources[s_index] = ((LightSource)curObjProperties.SelectedObject);

            }
            else if (objectTreeView.SelectedNode.Parent.Name == "Sounds")
            {
                CurrentProject.CurrentScene.SoundSources[s_index] = ((SoundSource)curObjProperties.SelectedObject);
            }

            objectTreeView.SelectedNode.Name = objectTreeView.SelectedNode.Text = ((ISource)curObjProperties.SelectedObject).Name;
        }
    }
}
