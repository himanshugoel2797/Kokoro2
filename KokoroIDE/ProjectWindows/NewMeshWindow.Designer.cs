namespace Kokoro.IDE.ProjectWindows
{
    partial class NewMeshWindow
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
            this.nameBox = new System.Windows.Forms.TextBox();
            this.nameLbl = new System.Windows.Forms.Label();
            this.meshBox = new System.Windows.Forms.TextBox();
            this.meshDataLbl = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.brsBtn = new System.Windows.Forms.Button();
            this.createBtn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(57, 10);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(297, 20);
            this.nameBox.TabIndex = 0;
            // 
            // nameLbl
            // 
            this.nameLbl.AutoSize = true;
            this.nameLbl.Location = new System.Drawing.Point(13, 13);
            this.nameLbl.Name = "nameLbl";
            this.nameLbl.Size = new System.Drawing.Size(38, 13);
            this.nameLbl.TabIndex = 1;
            this.nameLbl.Text = "Name:";
            // 
            // meshBox
            // 
            this.meshBox.Location = new System.Drawing.Point(16, 63);
            this.meshBox.Name = "meshBox";
            this.meshBox.Size = new System.Drawing.Size(259, 20);
            this.meshBox.TabIndex = 2;
            // 
            // meshDataLbl
            // 
            this.meshDataLbl.AutoSize = true;
            this.meshDataLbl.Location = new System.Drawing.Point(12, 39);
            this.meshDataLbl.Name = "meshDataLbl";
            this.meshDataLbl.Size = new System.Drawing.Size(36, 13);
            this.meshDataLbl.TabIndex = 3;
            this.meshDataLbl.Text = "Mesh:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Custom",
            "Cube",
            "Plane",
            "Sphere"});
            this.comboBox1.Location = new System.Drawing.Point(54, 36);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(300, 21);
            this.comboBox1.TabIndex = 4;
            // 
            // brsBtn
            // 
            this.brsBtn.Location = new System.Drawing.Point(279, 61);
            this.brsBtn.Name = "brsBtn";
            this.brsBtn.Size = new System.Drawing.Size(75, 23);
            this.brsBtn.TabIndex = 5;
            this.brsBtn.Text = "Browse";
            this.brsBtn.UseVisualStyleBackColor = true;
            // 
            // createBtn
            // 
            this.createBtn.Location = new System.Drawing.Point(279, 90);
            this.createBtn.Name = "createBtn";
            this.createBtn.Size = new System.Drawing.Size(75, 23);
            this.createBtn.TabIndex = 6;
            this.createBtn.Text = "Create";
            this.createBtn.UseVisualStyleBackColor = true;
            this.createBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // NewMeshWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 120);
            this.Controls.Add(this.createBtn);
            this.Controls.Add(this.brsBtn);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.meshDataLbl);
            this.Controls.Add(this.meshBox);
            this.Controls.Add(this.nameLbl);
            this.Controls.Add(this.nameBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewMeshWindow";
            this.Text = "New Mesh Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label nameLbl;
        private System.Windows.Forms.TextBox meshBox;
        private System.Windows.Forms.Label meshDataLbl;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button brsBtn;
        private System.Windows.Forms.Button createBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}