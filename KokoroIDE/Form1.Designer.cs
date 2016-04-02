namespace Kokoro2.IDE
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsContainer = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.graphicsPanel = new System.Windows.Forms.Panel();
            this.membersPanel = new System.Windows.Forms.Panel();
            this.contextSpecificControls = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.PlayPauseGame = new System.Windows.Forms.ToolStripButton();
            this.StopGame = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.addLight = new System.Windows.Forms.ToolStripButton();
            this.addMesh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.pbrViewer = new System.Windows.Forms.ToolStripButton();
            this.contentTabs = new System.Windows.Forms.TabControl();
            this.meshesTab = new System.Windows.Forms.TabPage();
            this.lightsTab = new System.Windows.Forms.TabPage();
            this.curObjProperties = new System.Windows.Forms.PropertyGrid();
            this.scriptsTab = new System.Windows.Forms.TabPage();
            this.meshTreeView = new System.Windows.Forms.TreeView();
            this.scenesTab = new System.Windows.Forms.TabPage();
            this.textureTool = new System.Windows.Forms.ToolStripButton();
            this.sceneTreeView = new System.Windows.Forms.TreeView();
            this.framerateLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.updaterateLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.addScript = new System.Windows.Forms.ToolStripButton();
            this.openPrj = new System.Windows.Forms.OpenFileDialog();
            this.scriptTreeView = new System.Windows.Forms.TreeView();
            this.lightTreeView = new System.Windows.Forms.TreeView();
            this.menuStrip1.SuspendLayout();
            this.toolsContainer.ContentPanel.SuspendLayout();
            this.toolsContainer.TopToolStripPanel.SuspendLayout();
            this.toolsContainer.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.membersPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.contentTabs.SuspendLayout();
            this.meshesTab.SuspendLayout();
            this.lightsTab.SuspendLayout();
            this.scriptsTab.SuspendLayout();
            this.scenesTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1352, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(149, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
            this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.printToolStripMenuItem.Text = "&Print";
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripMenuItem.Image")));
            this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.undoToolStripMenuItem.Text = "&Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.redoToolStripMenuItem.Text = "&Redo";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.cutToolStripMenuItem.Text = "Cu&t";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.copyToolStripMenuItem.Text = "&Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.pasteToolStripMenuItem.Text = "&Paste";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // customizeToolStripMenuItem
            // 
            this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
            this.customizeToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.customizeToolStripMenuItem.Text = "&Customize";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // toolsContainer
            // 
            // 
            // toolsContainer.toolsContentPanel
            // 
            this.toolsContainer.ContentPanel.Controls.Add(this.contextSpecificControls);
            this.toolsContainer.ContentPanel.Controls.Add(this.membersPanel);
            this.toolsContainer.ContentPanel.Controls.Add(this.graphicsPanel);
            this.toolsContainer.ContentPanel.Size = new System.Drawing.Size(1352, 964);
            this.toolsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolsContainer.Location = new System.Drawing.Point(0, 24);
            this.toolsContainer.Name = "toolsContainer";
            this.toolsContainer.Size = new System.Drawing.Size(1352, 989);
            this.toolsContainer.TabIndex = 2;
            this.toolsContainer.Text = "toolStripContainer1";
            // 
            // toolsContainer.TopToolStripPanel
            // 
            this.toolsContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.framerateLabel,
            this.updaterateLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 991);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1352, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // graphicsPanel
            // 
            this.graphicsPanel.Location = new System.Drawing.Point(0, 0);
            this.graphicsPanel.Name = "graphicsPanel";
            this.graphicsPanel.Size = new System.Drawing.Size(960, 540);
            this.graphicsPanel.TabIndex = 0;
            // 
            // membersPanel
            // 
            this.membersPanel.Controls.Add(this.curObjProperties);
            this.membersPanel.Controls.Add(this.contentTabs);
            this.membersPanel.Location = new System.Drawing.Point(966, 3);
            this.membersPanel.Name = "membersPanel";
            this.membersPanel.Size = new System.Drawing.Size(374, 936);
            this.membersPanel.TabIndex = 1;
            // 
            // contextSpecificControls
            // 
            this.contextSpecificControls.Location = new System.Drawing.Point(13, 547);
            this.contextSpecificControls.Name = "contextSpecificControls";
            this.contextSpecificControls.Size = new System.Drawing.Size(947, 392);
            this.contextSpecificControls.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PlayPauseGame,
            this.StopGame,
            this.toolStripSeparator6,
            this.addLight,
            this.addMesh,
            this.addScript,
            this.toolStripSeparator7,
            this.pbrViewer,
            this.textureTool});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(185, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // PlayPauseGame
            // 
            this.PlayPauseGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PlayPauseGame.Image = ((System.Drawing.Image)(resources.GetObject("PlayPauseGame.Image")));
            this.PlayPauseGame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PlayPauseGame.Name = "PlayPauseGame";
            this.PlayPauseGame.Size = new System.Drawing.Size(23, 22);
            this.PlayPauseGame.Text = "Start/Pause";
            // 
            // StopGame
            // 
            this.StopGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StopGame.Image = ((System.Drawing.Image)(resources.GetObject("StopGame.Image")));
            this.StopGame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StopGame.Name = "StopGame";
            this.StopGame.Size = new System.Drawing.Size(23, 22);
            this.StopGame.Text = "Stop";
            this.StopGame.ToolTipText = "Stop";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // addLight
            // 
            this.addLight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addLight.Image = ((System.Drawing.Image)(resources.GetObject("addLight.Image")));
            this.addLight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addLight.Name = "addLight";
            this.addLight.Size = new System.Drawing.Size(23, 22);
            this.addLight.Text = "toolStripButton3";
            this.addLight.ToolTipText = "Add New Light";
            // 
            // addMesh
            // 
            this.addMesh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addMesh.Image = ((System.Drawing.Image)(resources.GetObject("addMesh.Image")));
            this.addMesh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addMesh.Name = "addMesh";
            this.addMesh.Size = new System.Drawing.Size(23, 22);
            this.addMesh.Text = "Add New Mesh";
            this.addMesh.ToolTipText = "Add New Mesh";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // pbrViewer
            // 
            this.pbrViewer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbrViewer.Image = ((System.Drawing.Image)(resources.GetObject("pbrViewer.Image")));
            this.pbrViewer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbrViewer.Name = "pbrViewer";
            this.pbrViewer.Size = new System.Drawing.Size(23, 22);
            this.pbrViewer.Text = "Open Mesh Preview/Convert tool";
            // 
            // contentTabs
            // 
            this.contentTabs.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.contentTabs.Controls.Add(this.meshesTab);
            this.contentTabs.Controls.Add(this.lightsTab);
            this.contentTabs.Controls.Add(this.scriptsTab);
            this.contentTabs.Controls.Add(this.scenesTab);
            this.contentTabs.Location = new System.Drawing.Point(3, 3);
            this.contentTabs.Multiline = true;
            this.contentTabs.Name = "contentTabs";
            this.contentTabs.SelectedIndex = 0;
            this.contentTabs.Size = new System.Drawing.Size(368, 508);
            this.contentTabs.TabIndex = 0;
            // 
            // meshesTab
            // 
            this.meshesTab.Controls.Add(this.meshTreeView);
            this.meshesTab.Location = new System.Drawing.Point(4, 4);
            this.meshesTab.Name = "meshesTab";
            this.meshesTab.Padding = new System.Windows.Forms.Padding(3);
            this.meshesTab.Size = new System.Drawing.Size(341, 500);
            this.meshesTab.TabIndex = 0;
            this.meshesTab.Text = "Meshes";
            this.meshesTab.UseVisualStyleBackColor = true;
            // 
            // lightsTab
            // 
            this.lightsTab.Controls.Add(this.lightTreeView);
            this.lightsTab.Location = new System.Drawing.Point(4, 4);
            this.lightsTab.Name = "lightsTab";
            this.lightsTab.Padding = new System.Windows.Forms.Padding(3);
            this.lightsTab.Size = new System.Drawing.Size(341, 500);
            this.lightsTab.TabIndex = 1;
            this.lightsTab.Text = "Lights";
            this.lightsTab.UseVisualStyleBackColor = true;
            // 
            // curObjProperties
            // 
            this.curObjProperties.Location = new System.Drawing.Point(7, 517);
            this.curObjProperties.Name = "curObjProperties";
            this.curObjProperties.Size = new System.Drawing.Size(360, 416);
            this.curObjProperties.TabIndex = 1;
            // 
            // scriptsTab
            // 
            this.scriptsTab.Controls.Add(this.scriptTreeView);
            this.scriptsTab.Location = new System.Drawing.Point(4, 4);
            this.scriptsTab.Name = "scriptsTab";
            this.scriptsTab.Padding = new System.Windows.Forms.Padding(3);
            this.scriptsTab.Size = new System.Drawing.Size(341, 500);
            this.scriptsTab.TabIndex = 2;
            this.scriptsTab.Text = "Scripts";
            this.scriptsTab.UseVisualStyleBackColor = true;
            // 
            // meshTreeView
            // 
            this.meshTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.meshTreeView.Location = new System.Drawing.Point(3, 3);
            this.meshTreeView.Name = "meshTreeView";
            this.meshTreeView.Size = new System.Drawing.Size(335, 494);
            this.meshTreeView.TabIndex = 0;
            // 
            // scenesTab
            // 
            this.scenesTab.Controls.Add(this.sceneTreeView);
            this.scenesTab.Location = new System.Drawing.Point(4, 4);
            this.scenesTab.Name = "scenesTab";
            this.scenesTab.Padding = new System.Windows.Forms.Padding(3);
            this.scenesTab.Size = new System.Drawing.Size(341, 500);
            this.scenesTab.TabIndex = 3;
            this.scenesTab.Text = "Scenes";
            this.scenesTab.UseVisualStyleBackColor = true;
            // 
            // textureTool
            // 
            this.textureTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.textureTool.Image = ((System.Drawing.Image)(resources.GetObject("textureTool.Image")));
            this.textureTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.textureTool.Name = "textureTool";
            this.textureTool.Size = new System.Drawing.Size(23, 22);
            this.textureTool.Text = "Launch Texture Conversion tool";
            this.textureTool.ToolTipText = "Launch Texture Conversion tool";
            // 
            // sceneTreeView
            // 
            this.sceneTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sceneTreeView.Location = new System.Drawing.Point(3, 3);
            this.sceneTreeView.Name = "sceneTreeView";
            this.sceneTreeView.Size = new System.Drawing.Size(335, 494);
            this.sceneTreeView.TabIndex = 0;
            // 
            // framerateLabel
            // 
            this.framerateLabel.Name = "framerateLabel";
            this.framerateLabel.Size = new System.Drawing.Size(29, 17);
            this.framerateLabel.Text = "FPS:";
            // 
            // updaterateLabel
            // 
            this.updaterateLabel.Name = "updaterateLabel";
            this.updaterateLabel.Size = new System.Drawing.Size(31, 17);
            this.updaterateLabel.Text = "UPS:";
            // 
            // addScript
            // 
            this.addScript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addScript.Image = ((System.Drawing.Image)(resources.GetObject("addScript.Image")));
            this.addScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addScript.Name = "addScript";
            this.addScript.Size = new System.Drawing.Size(23, 22);
            this.addScript.Text = "Add New Script";
            // 
            // openPrj
            // 
            this.openPrj.DefaultExt = "hg";
            this.openPrj.Filter = "Project Files|*.hg";
            this.openPrj.SupportMultiDottedExtensions = true;
            // 
            // scriptTreeView
            // 
            this.scriptTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptTreeView.Location = new System.Drawing.Point(3, 3);
            this.scriptTreeView.Name = "scriptTreeView";
            this.scriptTreeView.Size = new System.Drawing.Size(335, 494);
            this.scriptTreeView.TabIndex = 1;
            // 
            // lightTreeView
            // 
            this.lightTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightTreeView.Location = new System.Drawing.Point(3, 3);
            this.lightTreeView.Name = "lightTreeView";
            this.lightTreeView.Size = new System.Drawing.Size(335, 494);
            this.lightTreeView.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1352, 1013);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolsContainer);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Kokoro Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolsContainer.ContentPanel.ResumeLayout(false);
            this.toolsContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolsContainer.TopToolStripPanel.PerformLayout();
            this.toolsContainer.ResumeLayout(false);
            this.toolsContainer.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.membersPanel.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contentTabs.ResumeLayout(false);
            this.meshesTab.ResumeLayout(false);
            this.lightsTab.ResumeLayout(false);
            this.scriptsTab.ResumeLayout(false);
            this.scenesTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolsContainer;
        private System.Windows.Forms.Panel contextSpecificControls;
        private System.Windows.Forms.Panel membersPanel;
        private System.Windows.Forms.Panel graphicsPanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton PlayPauseGame;
        private System.Windows.Forms.ToolStripButton StopGame;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton addLight;
        private System.Windows.Forms.ToolStripButton addMesh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton pbrViewer;
        private System.Windows.Forms.PropertyGrid curObjProperties;
        private System.Windows.Forms.TabControl contentTabs;
        private System.Windows.Forms.TabPage meshesTab;
        private System.Windows.Forms.TabPage lightsTab;
        private System.Windows.Forms.TabPage scriptsTab;
        private System.Windows.Forms.TreeView meshTreeView;
        private System.Windows.Forms.TabPage scenesTab;
        private System.Windows.Forms.ToolStripButton textureTool;
        private System.Windows.Forms.TreeView sceneTreeView;
        private System.Windows.Forms.ToolStripStatusLabel framerateLabel;
        private System.Windows.Forms.ToolStripStatusLabel updaterateLabel;
        private System.Windows.Forms.ToolStripButton addScript;
        private System.Windows.Forms.OpenFileDialog openPrj;
        private System.Windows.Forms.TreeView lightTreeView;
        private System.Windows.Forms.TreeView scriptTreeView;
    }
}