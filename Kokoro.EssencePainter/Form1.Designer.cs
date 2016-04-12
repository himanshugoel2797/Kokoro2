namespace Kokoro.EssencePainter
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
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.newTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.openMaterialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.openMaterialBtn = new System.Windows.Forms.Button();
            this.hardnessBar = new System.Windows.Forms.TrackBar();
            this.redoBtn = new System.Windows.Forms.Button();
            this.undoBtn = new System.Windows.Forms.Button();
            this.materialList = new System.Windows.Forms.ListView();
            this.fgColorButton = new System.Windows.Forms.Button();
            this.bgColorButton = new System.Windows.Forms.Button();
            this.brushSizeBar = new System.Windows.Forms.TrackBar();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.graphicsPanel = new System.Windows.Forms.Panel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.texMatTabs = new System.Windows.Forms.TabControl();
            this.texTab = new System.Windows.Forms.TabPage();
            this.texturesTree = new System.Windows.Forms.TreeView();
            this.matLayersTab = new System.Windows.Forms.TabPage();
            this.layerView = new System.Windows.Forms.ListBox();
            this.saveAsMatBtn = new System.Windows.Forms.Button();
            this.materialNameBox = new System.Windows.Forms.TextBox();
            this.saveMatBtn = new System.Windows.Forms.Button();
            this.emissiveMapBtn = new System.Windows.Forms.Button();
            this.roughnessMapBtn = new System.Windows.Forms.Button();
            this.reflectivityMapBtn = new System.Windows.Forms.Button();
            this.albedoMapBtn = new System.Windows.Forms.Button();
            this.normMapBtn = new System.Windows.Forms.Button();
            this.openModelDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveModelDialog = new System.Windows.Forms.SaveFileDialog();
            this.openTextureSetDialog = new System.Windows.Forms.OpenFileDialog();
            this.foreColorSelect = new System.Windows.Forms.ColorDialog();
            this.backColorSelect = new System.Windows.Forms.ColorDialog();
            this.normalMapLoadDialog = new System.Windows.Forms.OpenFileDialog();
            this.albedoMapLoadDialog = new System.Windows.Forms.OpenFileDialog();
            this.reflectivityMapLoadDialog = new System.Windows.Forms.OpenFileDialog();
            this.roughnessMapLoadDialog = new System.Windows.Forms.OpenFileDialog();
            this.emissiveMapLoadDialog = new System.Windows.Forms.OpenFileDialog();
            this.openMaterialDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveMaterialDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.removeMaterial = new System.Windows.Forms.Button();
            this.drawModeSelect = new System.Windows.Forms.ComboBox();
            this.activeTexturePieceList = new System.Windows.Forms.CheckedListBox();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hardnessBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brushSizeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.texMatTabs.SuspendLayout();
            this.texTab.SuspendLayout();
            this.matLayersTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1375, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator6,
            this.newTextureToolStripMenuItem,
            this.openTextureToolStripMenuItem,
            this.toolStripSeparator7,
            this.openMaterialToolStripMenuItem,
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
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.openToolStripMenuItem.Text = "Open Mesh";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(146, 6);
            // 
            // newTextureToolStripMenuItem
            // 
            this.newTextureToolStripMenuItem.Name = "newTextureToolStripMenuItem";
            this.newTextureToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.newTextureToolStripMenuItem.Text = "New Texture";
            this.newTextureToolStripMenuItem.Click += new System.EventHandler(this.newTextureToolStripMenuItem_Click);
            // 
            // openTextureToolStripMenuItem
            // 
            this.openTextureToolStripMenuItem.Name = "openTextureToolStripMenuItem";
            this.openTextureToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.openTextureToolStripMenuItem.Text = "Open Texture";
            this.openTextureToolStripMenuItem.Click += new System.EventHandler(this.openTextureToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(146, 6);
            // 
            // openMaterialToolStripMenuItem
            // 
            this.openMaterialToolStripMenuItem.Name = "openMaterialToolStripMenuItem";
            this.openMaterialToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.openMaterialToolStripMenuItem.Text = "Open Material";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(146, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(146, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
            this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.printToolStripMenuItem.Text = "&Print";
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripMenuItem.Image")));
            this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(146, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
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
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(141, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
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
            this.customizeToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.customizeToolStripMenuItem.Text = "&Customize";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
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
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1375, 735);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1375, 784);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(111, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.activeTexturePieceList);
            this.splitContainer1.Panel1.Controls.Add(this.drawModeSelect);
            this.splitContainer1.Panel1.Controls.Add(this.removeMaterial);
            this.splitContainer1.Panel1.Controls.Add(this.openMaterialBtn);
            this.splitContainer1.Panel1.Controls.Add(this.hardnessBar);
            this.splitContainer1.Panel1.Controls.Add(this.redoBtn);
            this.splitContainer1.Panel1.Controls.Add(this.undoBtn);
            this.splitContainer1.Panel1.Controls.Add(this.materialList);
            this.splitContainer1.Panel1.Controls.Add(this.fgColorButton);
            this.splitContainer1.Panel1.Controls.Add(this.bgColorButton);
            this.splitContainer1.Panel1.Controls.Add(this.brushSizeBar);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1375, 735);
            this.splitContainer1.SplitterDistance = 286;
            this.splitContainer1.TabIndex = 1;
            // 
            // openMaterialBtn
            // 
            this.openMaterialBtn.Location = new System.Drawing.Point(11, 241);
            this.openMaterialBtn.Name = "openMaterialBtn";
            this.openMaterialBtn.Size = new System.Drawing.Size(156, 23);
            this.openMaterialBtn.TabIndex = 7;
            this.openMaterialBtn.Text = "Open Material";
            this.openMaterialBtn.UseVisualStyleBackColor = true;
            this.openMaterialBtn.Click += new System.EventHandler(this.openMaterialBtn_Click);
            // 
            // hardnessBar
            // 
            this.hardnessBar.Location = new System.Drawing.Point(188, 22);
            this.hardnessBar.Maximum = 100;
            this.hardnessBar.Minimum = 1;
            this.hardnessBar.Name = "hardnessBar";
            this.hardnessBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.hardnessBar.Size = new System.Drawing.Size(45, 274);
            this.hardnessBar.TabIndex = 6;
            this.hardnessBar.TickFrequency = 5;
            this.hardnessBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.hardnessBar.Value = 1;
            // 
            // redoBtn
            // 
            this.redoBtn.Location = new System.Drawing.Point(92, 209);
            this.redoBtn.Name = "redoBtn";
            this.redoBtn.Size = new System.Drawing.Size(75, 23);
            this.redoBtn.TabIndex = 5;
            this.redoBtn.Text = "Redo";
            this.redoBtn.UseVisualStyleBackColor = true;
            this.redoBtn.Click += new System.EventHandler(this.redoBtn_Click);
            // 
            // undoBtn
            // 
            this.undoBtn.Location = new System.Drawing.Point(11, 209);
            this.undoBtn.Name = "undoBtn";
            this.undoBtn.Size = new System.Drawing.Size(75, 23);
            this.undoBtn.TabIndex = 4;
            this.undoBtn.Text = "Undo";
            this.undoBtn.UseVisualStyleBackColor = true;
            this.undoBtn.Click += new System.EventHandler(this.undoBtn_Click);
            // 
            // materialList
            // 
            this.materialList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialList.Location = new System.Drawing.Point(3, 303);
            this.materialList.Name = "materialList";
            this.materialList.Size = new System.Drawing.Size(278, 427);
            this.materialList.TabIndex = 3;
            this.materialList.UseCompatibleStateImageBehavior = false;
            this.materialList.View = System.Windows.Forms.View.List;
            this.materialList.SelectedIndexChanged += new System.EventHandler(this.materialList_SelectedIndexChanged);
            // 
            // fgColorButton
            // 
            this.fgColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fgColorButton.Location = new System.Drawing.Point(48, 34);
            this.fgColorButton.Name = "fgColorButton";
            this.fgColorButton.Size = new System.Drawing.Size(80, 54);
            this.fgColorButton.TabIndex = 2;
            this.fgColorButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.fgColorButton.UseVisualStyleBackColor = true;
            this.fgColorButton.Click += new System.EventHandler(this.fgColorButton_Click);
            // 
            // bgColorButton
            // 
            this.bgColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bgColorButton.Location = new System.Drawing.Point(12, 12);
            this.bgColorButton.Name = "bgColorButton";
            this.bgColorButton.Size = new System.Drawing.Size(80, 54);
            this.bgColorButton.TabIndex = 1;
            this.bgColorButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bgColorButton.UseVisualStyleBackColor = true;
            this.bgColorButton.Click += new System.EventHandler(this.bgColorButton_Click);
            // 
            // brushSizeBar
            // 
            this.brushSizeBar.Location = new System.Drawing.Point(239, 22);
            this.brushSizeBar.Maximum = 512;
            this.brushSizeBar.Minimum = 1;
            this.brushSizeBar.Name = "brushSizeBar";
            this.brushSizeBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.brushSizeBar.Size = new System.Drawing.Size(45, 274);
            this.brushSizeBar.TabIndex = 0;
            this.brushSizeBar.TickFrequency = 5;
            this.brushSizeBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.brushSizeBar.Value = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.graphicsPanel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1085, 733);
            this.splitContainer2.SplitterDistance = 814;
            this.splitContainer2.TabIndex = 0;
            // 
            // graphicsPanel
            // 
            this.graphicsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphicsPanel.Location = new System.Drawing.Point(0, 0);
            this.graphicsPanel.Name = "graphicsPanel";
            this.graphicsPanel.Size = new System.Drawing.Size(812, 731);
            this.graphicsPanel.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.texMatTabs);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.saveAsMatBtn);
            this.splitContainer3.Panel2.Controls.Add(this.materialNameBox);
            this.splitContainer3.Panel2.Controls.Add(this.saveMatBtn);
            this.splitContainer3.Panel2.Controls.Add(this.emissiveMapBtn);
            this.splitContainer3.Panel2.Controls.Add(this.roughnessMapBtn);
            this.splitContainer3.Panel2.Controls.Add(this.reflectivityMapBtn);
            this.splitContainer3.Panel2.Controls.Add(this.albedoMapBtn);
            this.splitContainer3.Panel2.Controls.Add(this.normMapBtn);
            this.splitContainer3.Size = new System.Drawing.Size(265, 731);
            this.splitContainer3.SplitterDistance = 263;
            this.splitContainer3.TabIndex = 0;
            // 
            // texMatTabs
            // 
            this.texMatTabs.Controls.Add(this.texTab);
            this.texMatTabs.Controls.Add(this.matLayersTab);
            this.texMatTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texMatTabs.Location = new System.Drawing.Point(0, 0);
            this.texMatTabs.Name = "texMatTabs";
            this.texMatTabs.SelectedIndex = 0;
            this.texMatTabs.Size = new System.Drawing.Size(265, 263);
            this.texMatTabs.TabIndex = 0;
            // 
            // texTab
            // 
            this.texTab.Controls.Add(this.texturesTree);
            this.texTab.Location = new System.Drawing.Point(4, 22);
            this.texTab.Name = "texTab";
            this.texTab.Padding = new System.Windows.Forms.Padding(3);
            this.texTab.Size = new System.Drawing.Size(257, 237);
            this.texTab.TabIndex = 0;
            this.texTab.Text = "Textures";
            this.texTab.UseVisualStyleBackColor = true;
            // 
            // texturesTree
            // 
            this.texturesTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texturesTree.Location = new System.Drawing.Point(3, 3);
            this.texturesTree.Name = "texturesTree";
            this.texturesTree.Size = new System.Drawing.Size(251, 231);
            this.texturesTree.TabIndex = 0;
            this.texturesTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.texturesTree_AfterSelect);
            // 
            // matLayersTab
            // 
            this.matLayersTab.Controls.Add(this.layerView);
            this.matLayersTab.Location = new System.Drawing.Point(4, 22);
            this.matLayersTab.Name = "matLayersTab";
            this.matLayersTab.Padding = new System.Windows.Forms.Padding(3);
            this.matLayersTab.Size = new System.Drawing.Size(257, 238);
            this.matLayersTab.TabIndex = 1;
            this.matLayersTab.Text = "Layers";
            this.matLayersTab.UseVisualStyleBackColor = true;
            // 
            // layerView
            // 
            this.layerView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layerView.FormattingEnabled = true;
            this.layerView.Location = new System.Drawing.Point(3, 3);
            this.layerView.Name = "layerView";
            this.layerView.Size = new System.Drawing.Size(251, 232);
            this.layerView.TabIndex = 0;
            // 
            // saveAsMatBtn
            // 
            this.saveAsMatBtn.Location = new System.Drawing.Point(124, 7);
            this.saveAsMatBtn.Name = "saveAsMatBtn";
            this.saveAsMatBtn.Size = new System.Drawing.Size(125, 23);
            this.saveAsMatBtn.TabIndex = 8;
            this.saveAsMatBtn.Text = "Save New Material";
            this.saveAsMatBtn.UseVisualStyleBackColor = true;
            this.saveAsMatBtn.Click += new System.EventHandler(this.saveAsMatBtn_Click);
            // 
            // materialNameBox
            // 
            this.materialNameBox.Location = new System.Drawing.Point(7, 36);
            this.materialNameBox.Name = "materialNameBox";
            this.materialNameBox.Size = new System.Drawing.Size(238, 20);
            this.materialNameBox.TabIndex = 7;
            // 
            // saveMatBtn
            // 
            this.saveMatBtn.Location = new System.Drawing.Point(7, 7);
            this.saveMatBtn.Name = "saveMatBtn";
            this.saveMatBtn.Size = new System.Drawing.Size(111, 23);
            this.saveMatBtn.TabIndex = 6;
            this.saveMatBtn.Text = "Save Material";
            this.saveMatBtn.UseVisualStyleBackColor = true;
            this.saveMatBtn.Click += new System.EventHandler(this.saveMatBtn_Click);
            // 
            // emissiveMapBtn
            // 
            this.emissiveMapBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.emissiveMapBtn.Location = new System.Drawing.Point(7, 325);
            this.emissiveMapBtn.Name = "emissiveMapBtn";
            this.emissiveMapBtn.Size = new System.Drawing.Size(118, 114);
            this.emissiveMapBtn.TabIndex = 5;
            this.emissiveMapBtn.Text = "Emissive";
            this.emissiveMapBtn.UseVisualStyleBackColor = true;
            this.emissiveMapBtn.Click += new System.EventHandler(this.emissiveMapBtn_Click);
            // 
            // roughnessMapBtn
            // 
            this.roughnessMapBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.roughnessMapBtn.Location = new System.Drawing.Point(131, 205);
            this.roughnessMapBtn.Name = "roughnessMapBtn";
            this.roughnessMapBtn.Size = new System.Drawing.Size(118, 114);
            this.roughnessMapBtn.TabIndex = 4;
            this.roughnessMapBtn.Text = "Roughness";
            this.roughnessMapBtn.UseVisualStyleBackColor = true;
            this.roughnessMapBtn.Click += new System.EventHandler(this.roughnessMapBtn_Click);
            // 
            // reflectivityMapBtn
            // 
            this.reflectivityMapBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.reflectivityMapBtn.Location = new System.Drawing.Point(7, 205);
            this.reflectivityMapBtn.Name = "reflectivityMapBtn";
            this.reflectivityMapBtn.Size = new System.Drawing.Size(118, 114);
            this.reflectivityMapBtn.TabIndex = 3;
            this.reflectivityMapBtn.Text = "Reflectivity";
            this.reflectivityMapBtn.UseVisualStyleBackColor = true;
            this.reflectivityMapBtn.Click += new System.EventHandler(this.reflectivityMapBtn_Click);
            // 
            // albedoMapBtn
            // 
            this.albedoMapBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.albedoMapBtn.Location = new System.Drawing.Point(131, 85);
            this.albedoMapBtn.Name = "albedoMapBtn";
            this.albedoMapBtn.Size = new System.Drawing.Size(118, 114);
            this.albedoMapBtn.TabIndex = 2;
            this.albedoMapBtn.Text = "Albedo";
            this.albedoMapBtn.UseVisualStyleBackColor = true;
            this.albedoMapBtn.Click += new System.EventHandler(this.albedoMapBtn_Click);
            // 
            // normMapBtn
            // 
            this.normMapBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.normMapBtn.Location = new System.Drawing.Point(7, 85);
            this.normMapBtn.Name = "normMapBtn";
            this.normMapBtn.Size = new System.Drawing.Size(118, 114);
            this.normMapBtn.TabIndex = 1;
            this.normMapBtn.Text = "Normal";
            this.normMapBtn.UseVisualStyleBackColor = true;
            this.normMapBtn.Click += new System.EventHandler(this.normMapBtn_Click);
            // 
            // openModelDialog
            // 
            this.openModelDialog.DefaultExt = "ko";
            this.openModelDialog.Filter = "Kokoro Mesh|*.ko|Object File|*.obj|Collada File|*.dae";
            this.openModelDialog.Title = "Open Model";
            // 
            // saveModelDialog
            // 
            this.saveModelDialog.DefaultExt = "ko";
            this.saveModelDialog.Filter = "Kokoro Mesh|*.ko";
            this.saveModelDialog.Title = "Choose a location for the converted mesh";
            // 
            // openTextureSetDialog
            // 
            this.openTextureSetDialog.Filter = "Kokoro Texture Set|*.dfc";
            this.openTextureSetDialog.Title = "Select a Texture set to load";
            // 
            // foreColorSelect
            // 
            this.foreColorSelect.AnyColor = true;
            this.foreColorSelect.FullOpen = true;
            // 
            // backColorSelect
            // 
            this.backColorSelect.AnyColor = true;
            this.backColorSelect.FullOpen = true;
            // 
            // openMaterialDialog
            // 
            this.openMaterialDialog.Filter = "Kokoro Material|*.dfc";
            // 
            // saveMaterialDialog
            // 
            this.saveMaterialDialog.Filter = "Kokoro Material |*.dfc";
            // 
            // removeMaterial
            // 
            this.removeMaterial.Location = new System.Drawing.Point(11, 273);
            this.removeMaterial.Name = "removeMaterial";
            this.removeMaterial.Size = new System.Drawing.Size(156, 23);
            this.removeMaterial.TabIndex = 8;
            this.removeMaterial.Text = "Remove Material";
            this.removeMaterial.UseVisualStyleBackColor = true;
            this.removeMaterial.Click += new System.EventHandler(this.removeMaterial_Click);
            // 
            // drawModeSelect
            // 
            this.drawModeSelect.FormattingEnabled = true;
            this.drawModeSelect.Items.AddRange(new object[] {
            "Brush",
            "Line"});
            this.drawModeSelect.Location = new System.Drawing.Point(12, 94);
            this.drawModeSelect.Name = "drawModeSelect";
            this.drawModeSelect.Size = new System.Drawing.Size(155, 21);
            this.drawModeSelect.TabIndex = 10;
            // 
            // activeTexturePieceList
            // 
            this.activeTexturePieceList.FormattingEnabled = true;
            this.activeTexturePieceList.Location = new System.Drawing.Point(12, 122);
            this.activeTexturePieceList.Name = "activeTexturePieceList";
            this.activeTexturePieceList.Size = new System.Drawing.Size(155, 79);
            this.activeTexturePieceList.TabIndex = 11;
            this.activeTexturePieceList.SelectedIndexChanged += new System.EventHandler(this.activeTexturePieceList_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1375, 784);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "Form1";
            this.Text = "Essence Painter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hardnessBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.brushSizeBar)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.texMatTabs.ResumeLayout(false);
            this.texTab.ResumeLayout(false);
            this.matLayersTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel graphicsPanel;
        private System.Windows.Forms.OpenFileDialog openModelDialog;
        private System.Windows.Forms.SaveFileDialog saveModelDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem newTextureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openTextureToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TabControl texMatTabs;
        private System.Windows.Forms.TabPage texTab;
        private System.Windows.Forms.TabPage matLayersTab;
        private System.Windows.Forms.TreeView texturesTree;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.OpenFileDialog openTextureSetDialog;
        private System.Windows.Forms.Button fgColorButton;
        private System.Windows.Forms.Button bgColorButton;
        private System.Windows.Forms.TrackBar brushSizeBar;
        private System.Windows.Forms.ColorDialog foreColorSelect;
        private System.Windows.Forms.ColorDialog backColorSelect;
        private System.Windows.Forms.ListView materialList;
        private System.Windows.Forms.ListBox layerView;
        private System.Windows.Forms.Button redoBtn;
        private System.Windows.Forms.Button undoBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem openMaterialToolStripMenuItem;
        private System.Windows.Forms.Button roughnessMapBtn;
        private System.Windows.Forms.Button reflectivityMapBtn;
        private System.Windows.Forms.Button albedoMapBtn;
        private System.Windows.Forms.Button normMapBtn;
        private System.Windows.Forms.Button emissiveMapBtn;
        private System.Windows.Forms.TrackBar hardnessBar;
        private System.Windows.Forms.TextBox materialNameBox;
        private System.Windows.Forms.Button saveMatBtn;
        private System.Windows.Forms.Button openMaterialBtn;
        private System.Windows.Forms.OpenFileDialog normalMapLoadDialog;
        private System.Windows.Forms.OpenFileDialog albedoMapLoadDialog;
        private System.Windows.Forms.OpenFileDialog reflectivityMapLoadDialog;
        private System.Windows.Forms.OpenFileDialog roughnessMapLoadDialog;
        private System.Windows.Forms.OpenFileDialog emissiveMapLoadDialog;
        private System.Windows.Forms.OpenFileDialog openMaterialDialog;
        private System.Windows.Forms.SaveFileDialog saveMaterialDialog;
        private System.Windows.Forms.Button saveAsMatBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button removeMaterial;
        private System.Windows.Forms.ComboBox drawModeSelect;
        private System.Windows.Forms.CheckedListBox activeTexturePieceList;
    }
}

