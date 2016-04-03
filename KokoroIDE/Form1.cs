﻿using Kokoro.IDE;
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
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = openPrj.ShowDialog();
            if (result == DialogResult.OK)
            {
                CurrentProject = ProjectInfo.LoadProject(openPrj.FileName);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            context.Stop();
        }

        private void showEngineOutput_Click(object sender, EventArgs e)
        {
            EngineOutputWindow engineOut = new EngineOutputWindow(logger);
            engineOut.Show();
        }
    }
}
