namespace Kokoro.EssencePainter
{
    partial class NewTextureDialog
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
            this.textureFilePath = new System.Windows.Forms.TextBox();
            this.saveLocBtn = new System.Windows.Forms.Button();
            this.texName = new System.Windows.Forms.TextBox();
            this.nameLbl = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.albedoMapEnabledBox = new System.Windows.Forms.CheckBox();
            this.roughnessMapEnabledBox = new System.Windows.Forms.CheckBox();
            this.reflectivityMapEnabledBox = new System.Windows.Forms.CheckBox();
            this.normalMapEnabledBox = new System.Windows.Forms.CheckBox();
            this.widthSelector = new System.Windows.Forms.NumericUpDown();
            this.heightSelector = new System.Windows.Forms.NumericUpDown();
            this.widthLbl = new System.Windows.Forms.Label();
            this.heightLbl = new System.Windows.Forms.Label();
            this.createBtn = new System.Windows.Forms.Button();
            this.emissionMapEnabledBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.widthSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // textureFilePath
            // 
            this.textureFilePath.Location = new System.Drawing.Point(13, 45);
            this.textureFilePath.Name = "textureFilePath";
            this.textureFilePath.Size = new System.Drawing.Size(322, 20);
            this.textureFilePath.TabIndex = 0;
            // 
            // saveLocBtn
            // 
            this.saveLocBtn.Location = new System.Drawing.Point(341, 43);
            this.saveLocBtn.Name = "saveLocBtn";
            this.saveLocBtn.Size = new System.Drawing.Size(46, 23);
            this.saveLocBtn.TabIndex = 1;
            this.saveLocBtn.Text = "...";
            this.saveLocBtn.UseVisualStyleBackColor = true;
            this.saveLocBtn.Click += new System.EventHandler(this.saveLocBtn_Click);
            // 
            // texName
            // 
            this.texName.Location = new System.Drawing.Point(56, 13);
            this.texName.Name = "texName";
            this.texName.Size = new System.Drawing.Size(331, 20);
            this.texName.TabIndex = 2;
            // 
            // nameLbl
            // 
            this.nameLbl.AutoSize = true;
            this.nameLbl.Location = new System.Drawing.Point(12, 16);
            this.nameLbl.Name = "nameLbl";
            this.nameLbl.Size = new System.Drawing.Size(38, 13);
            this.nameLbl.TabIndex = 3;
            this.nameLbl.Text = "Name:";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "dfc";
            this.saveFileDialog1.Filter = "Kokoro Texture Set|*.dfc";
            this.saveFileDialog1.Title = "Choose a save filename";
            // 
            // albedoMapEnabledBox
            // 
            this.albedoMapEnabledBox.AutoSize = true;
            this.albedoMapEnabledBox.Checked = true;
            this.albedoMapEnabledBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.albedoMapEnabledBox.Location = new System.Drawing.Point(13, 81);
            this.albedoMapEnabledBox.Name = "albedoMapEnabledBox";
            this.albedoMapEnabledBox.Size = new System.Drawing.Size(83, 17);
            this.albedoMapEnabledBox.TabIndex = 4;
            this.albedoMapEnabledBox.Text = "Albedo Map";
            this.albedoMapEnabledBox.UseVisualStyleBackColor = true;
            // 
            // roughnessMapEnabledBox
            // 
            this.roughnessMapEnabledBox.AutoSize = true;
            this.roughnessMapEnabledBox.Location = new System.Drawing.Point(12, 104);
            this.roughnessMapEnabledBox.Name = "roughnessMapEnabledBox";
            this.roughnessMapEnabledBox.Size = new System.Drawing.Size(104, 17);
            this.roughnessMapEnabledBox.TabIndex = 5;
            this.roughnessMapEnabledBox.Text = "Roughness Map";
            this.roughnessMapEnabledBox.UseVisualStyleBackColor = true;
            // 
            // reflectivityMapEnabledBox
            // 
            this.reflectivityMapEnabledBox.AutoSize = true;
            this.reflectivityMapEnabledBox.Location = new System.Drawing.Point(12, 127);
            this.reflectivityMapEnabledBox.Name = "reflectivityMapEnabledBox";
            this.reflectivityMapEnabledBox.Size = new System.Drawing.Size(102, 17);
            this.reflectivityMapEnabledBox.TabIndex = 6;
            this.reflectivityMapEnabledBox.Text = "Reflectivity Map";
            this.reflectivityMapEnabledBox.UseVisualStyleBackColor = true;
            // 
            // normalMapEnabledBox
            // 
            this.normalMapEnabledBox.AutoSize = true;
            this.normalMapEnabledBox.Checked = true;
            this.normalMapEnabledBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.normalMapEnabledBox.Location = new System.Drawing.Point(12, 150);
            this.normalMapEnabledBox.Name = "normalMapEnabledBox";
            this.normalMapEnabledBox.Size = new System.Drawing.Size(83, 17);
            this.normalMapEnabledBox.TabIndex = 7;
            this.normalMapEnabledBox.Text = "Normal Map";
            this.normalMapEnabledBox.UseVisualStyleBackColor = true;
            // 
            // widthSelector
            // 
            this.widthSelector.Location = new System.Drawing.Point(312, 80);
            this.widthSelector.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
            this.widthSelector.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.widthSelector.Name = "widthSelector";
            this.widthSelector.Size = new System.Drawing.Size(75, 20);
            this.widthSelector.TabIndex = 8;
            this.widthSelector.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // heightSelector
            // 
            this.heightSelector.Location = new System.Drawing.Point(312, 103);
            this.heightSelector.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
            this.heightSelector.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.heightSelector.Name = "heightSelector";
            this.heightSelector.Size = new System.Drawing.Size(75, 20);
            this.heightSelector.TabIndex = 9;
            this.heightSelector.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // widthLbl
            // 
            this.widthLbl.AutoSize = true;
            this.widthLbl.Location = new System.Drawing.Point(271, 82);
            this.widthLbl.Name = "widthLbl";
            this.widthLbl.Size = new System.Drawing.Size(38, 13);
            this.widthLbl.TabIndex = 10;
            this.widthLbl.Text = "Width:";
            // 
            // heightLbl
            // 
            this.heightLbl.AutoSize = true;
            this.heightLbl.Location = new System.Drawing.Point(268, 105);
            this.heightLbl.Name = "heightLbl";
            this.heightLbl.Size = new System.Drawing.Size(41, 13);
            this.heightLbl.TabIndex = 11;
            this.heightLbl.Text = "Height:";
            // 
            // createBtn
            // 
            this.createBtn.Location = new System.Drawing.Point(314, 206);
            this.createBtn.Name = "createBtn";
            this.createBtn.Size = new System.Drawing.Size(75, 23);
            this.createBtn.TabIndex = 12;
            this.createBtn.Text = "Create";
            this.createBtn.UseVisualStyleBackColor = true;
            this.createBtn.Click += new System.EventHandler(this.createBtn_Click);
            // 
            // emissionMapEnabledBox
            // 
            this.emissionMapEnabledBox.AutoSize = true;
            this.emissionMapEnabledBox.Location = new System.Drawing.Point(12, 173);
            this.emissionMapEnabledBox.Name = "emissionMapEnabledBox";
            this.emissionMapEnabledBox.Size = new System.Drawing.Size(91, 17);
            this.emissionMapEnabledBox.TabIndex = 13;
            this.emissionMapEnabledBox.Text = "Emission Map";
            this.emissionMapEnabledBox.UseVisualStyleBackColor = true;
            // 
            // NewTextureDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 241);
            this.Controls.Add(this.emissionMapEnabledBox);
            this.Controls.Add(this.createBtn);
            this.Controls.Add(this.heightLbl);
            this.Controls.Add(this.widthLbl);
            this.Controls.Add(this.heightSelector);
            this.Controls.Add(this.widthSelector);
            this.Controls.Add(this.normalMapEnabledBox);
            this.Controls.Add(this.reflectivityMapEnabledBox);
            this.Controls.Add(this.roughnessMapEnabledBox);
            this.Controls.Add(this.albedoMapEnabledBox);
            this.Controls.Add(this.nameLbl);
            this.Controls.Add(this.texName);
            this.Controls.Add(this.saveLocBtn);
            this.Controls.Add(this.textureFilePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "NewTextureDialog";
            this.ShowIcon = false;
            this.Text = "Create New Texture";
            ((System.ComponentModel.ISupportInitialize)(this.widthSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textureFilePath;
        private System.Windows.Forms.Button saveLocBtn;
        private System.Windows.Forms.TextBox texName;
        private System.Windows.Forms.Label nameLbl;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox albedoMapEnabledBox;
        private System.Windows.Forms.CheckBox roughnessMapEnabledBox;
        private System.Windows.Forms.CheckBox reflectivityMapEnabledBox;
        private System.Windows.Forms.CheckBox normalMapEnabledBox;
        private System.Windows.Forms.NumericUpDown widthSelector;
        private System.Windows.Forms.NumericUpDown heightSelector;
        private System.Windows.Forms.Label widthLbl;
        private System.Windows.Forms.Label heightLbl;
        private System.Windows.Forms.Button createBtn;
        private System.Windows.Forms.CheckBox emissionMapEnabledBox;
    }
}