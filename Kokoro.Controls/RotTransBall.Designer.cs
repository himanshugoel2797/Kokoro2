namespace Kokoro.Controls
{
    partial class RotTransBall
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.XtrackBar = new System.Windows.Forms.TrackBar();
            this.rotSelector = new System.Windows.Forms.RadioButton();
            this.transSelector = new System.Windows.Forms.RadioButton();
            this.xRot_valBox = new System.Windows.Forms.TextBox();
            this.xTrans_valBox = new System.Windows.Forms.TextBox();
            this.xAxisLabel = new System.Windows.Forms.Label();
            this.yAxisLabel = new System.Windows.Forms.Label();
            this.yTrans_valBox = new System.Windows.Forms.TextBox();
            this.yRot_valBox = new System.Windows.Forms.TextBox();
            this.YtrackBar = new System.Windows.Forms.TrackBar();
            this.zAxisLabel = new System.Windows.Forms.Label();
            this.zTrans_valBox = new System.Windows.Forms.TextBox();
            this.zRot_valBox = new System.Windows.Forms.TextBox();
            this.ZtrackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.XtrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YtrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZtrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // XtrackBar
            // 
            this.XtrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.XtrackBar.LargeChange = 1;
            this.XtrackBar.Location = new System.Drawing.Point(3, 3);
            this.XtrackBar.Maximum = 25;
            this.XtrackBar.Name = "XtrackBar";
            this.XtrackBar.Size = new System.Drawing.Size(270, 45);
            this.XtrackBar.TabIndex = 0;
            this.XtrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.XtrackBar.Scroll += new System.EventHandler(this.XtrackBar_Scroll);
            // 
            // rotSelector
            // 
            this.rotSelector.AutoSize = true;
            this.rotSelector.Checked = true;
            this.rotSelector.Location = new System.Drawing.Point(4, 213);
            this.rotSelector.Name = "rotSelector";
            this.rotSelector.Size = new System.Drawing.Size(65, 17);
            this.rotSelector.TabIndex = 3;
            this.rotSelector.TabStop = true;
            this.rotSelector.Text = "Rotation";
            this.rotSelector.UseVisualStyleBackColor = true;
            this.rotSelector.CheckedChanged += new System.EventHandler(this.rotSelector_CheckedChanged);
            // 
            // transSelector
            // 
            this.transSelector.AutoSize = true;
            this.transSelector.Location = new System.Drawing.Point(89, 213);
            this.transSelector.Name = "transSelector";
            this.transSelector.Size = new System.Drawing.Size(77, 17);
            this.transSelector.TabIndex = 4;
            this.transSelector.Text = "Translation";
            this.transSelector.UseVisualStyleBackColor = true;
            // 
            // xRot_valBox
            // 
            this.xRot_valBox.Location = new System.Drawing.Point(32, 42);
            this.xRot_valBox.Name = "xRot_valBox";
            this.xRot_valBox.Size = new System.Drawing.Size(73, 20);
            this.xRot_valBox.TabIndex = 5;
            this.xRot_valBox.TextChanged += new System.EventHandler(this.Box_TextChanged);
            // 
            // xTrans_valBox
            // 
            this.xTrans_valBox.Location = new System.Drawing.Point(111, 42);
            this.xTrans_valBox.Name = "xTrans_valBox";
            this.xTrans_valBox.Size = new System.Drawing.Size(73, 20);
            this.xTrans_valBox.TabIndex = 6;
            // 
            // xAxisLabel
            // 
            this.xAxisLabel.AutoSize = true;
            this.xAxisLabel.Location = new System.Drawing.Point(12, 45);
            this.xAxisLabel.Name = "xAxisLabel";
            this.xAxisLabel.Size = new System.Drawing.Size(14, 13);
            this.xAxisLabel.TabIndex = 7;
            this.xAxisLabel.Text = "X";
            // 
            // yAxisLabel
            // 
            this.yAxisLabel.AutoSize = true;
            this.yAxisLabel.Location = new System.Drawing.Point(13, 110);
            this.yAxisLabel.Name = "yAxisLabel";
            this.yAxisLabel.Size = new System.Drawing.Size(14, 13);
            this.yAxisLabel.TabIndex = 11;
            this.yAxisLabel.Text = "Y";
            // 
            // yTrans_valBox
            // 
            this.yTrans_valBox.Location = new System.Drawing.Point(112, 107);
            this.yTrans_valBox.Name = "yTrans_valBox";
            this.yTrans_valBox.Size = new System.Drawing.Size(73, 20);
            this.yTrans_valBox.TabIndex = 10;
            // 
            // yRot_valBox
            // 
            this.yRot_valBox.Location = new System.Drawing.Point(33, 107);
            this.yRot_valBox.Name = "yRot_valBox";
            this.yRot_valBox.Size = new System.Drawing.Size(73, 20);
            this.yRot_valBox.TabIndex = 9;
            // 
            // YtrackBar
            // 
            this.YtrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.YtrackBar.Location = new System.Drawing.Point(4, 68);
            this.YtrackBar.Maximum = 25;
            this.YtrackBar.Name = "YtrackBar";
            this.YtrackBar.Size = new System.Drawing.Size(270, 45);
            this.YtrackBar.TabIndex = 8;
            this.YtrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.YtrackBar.Scroll += new System.EventHandler(this.YtrackBar_Scroll);
            // 
            // zAxisLabel
            // 
            this.zAxisLabel.AutoSize = true;
            this.zAxisLabel.Location = new System.Drawing.Point(13, 175);
            this.zAxisLabel.Name = "zAxisLabel";
            this.zAxisLabel.Size = new System.Drawing.Size(14, 13);
            this.zAxisLabel.TabIndex = 15;
            this.zAxisLabel.Text = "Z";
            // 
            // zTrans_valBox
            // 
            this.zTrans_valBox.Location = new System.Drawing.Point(112, 172);
            this.zTrans_valBox.Name = "zTrans_valBox";
            this.zTrans_valBox.Size = new System.Drawing.Size(73, 20);
            this.zTrans_valBox.TabIndex = 14;
            // 
            // zRot_valBox
            // 
            this.zRot_valBox.Location = new System.Drawing.Point(33, 172);
            this.zRot_valBox.Name = "zRot_valBox";
            this.zRot_valBox.Size = new System.Drawing.Size(73, 20);
            this.zRot_valBox.TabIndex = 13;
            // 
            // ZtrackBar
            // 
            this.ZtrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ZtrackBar.Location = new System.Drawing.Point(4, 133);
            this.ZtrackBar.Maximum = 25;
            this.ZtrackBar.Name = "ZtrackBar";
            this.ZtrackBar.Size = new System.Drawing.Size(270, 45);
            this.ZtrackBar.TabIndex = 12;
            this.ZtrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ZtrackBar.Scroll += new System.EventHandler(this.ZtrackBar_Scroll);
            // 
            // RotTransBall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.zAxisLabel);
            this.Controls.Add(this.zTrans_valBox);
            this.Controls.Add(this.zRot_valBox);
            this.Controls.Add(this.ZtrackBar);
            this.Controls.Add(this.yAxisLabel);
            this.Controls.Add(this.yTrans_valBox);
            this.Controls.Add(this.yRot_valBox);
            this.Controls.Add(this.YtrackBar);
            this.Controls.Add(this.xAxisLabel);
            this.Controls.Add(this.xTrans_valBox);
            this.Controls.Add(this.xRot_valBox);
            this.Controls.Add(this.transSelector);
            this.Controls.Add(this.rotSelector);
            this.Controls.Add(this.XtrackBar);
            this.Name = "RotTransBall";
            this.Size = new System.Drawing.Size(300, 240);
            ((System.ComponentModel.ISupportInitialize)(this.XtrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YtrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZtrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar XtrackBar;
        private System.Windows.Forms.RadioButton rotSelector;
        private System.Windows.Forms.RadioButton transSelector;
        private System.Windows.Forms.TextBox xRot_valBox;
        private System.Windows.Forms.TextBox xTrans_valBox;
        private System.Windows.Forms.Label xAxisLabel;
        private System.Windows.Forms.Label yAxisLabel;
        private System.Windows.Forms.TextBox yTrans_valBox;
        private System.Windows.Forms.TextBox yRot_valBox;
        private System.Windows.Forms.TrackBar YtrackBar;
        private System.Windows.Forms.Label zAxisLabel;
        private System.Windows.Forms.TextBox zTrans_valBox;
        private System.Windows.Forms.TextBox zRot_valBox;
        private System.Windows.Forms.TrackBar ZtrackBar;
    }
}
