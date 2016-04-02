namespace Kokoro.IDE
{
    partial class EngineOutputWindow
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
            this.components = new System.ComponentModel.Container();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.errorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.performanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compatibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(441, 695);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.errorsToolStripMenuItem,
            this.performanceToolStripMenuItem,
            this.compatibilityToolStripMenuItem,
            this.otherToolStripMenuItem,
            this.markerToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(147, 114);
            // 
            // errorsToolStripMenuItem
            // 
            this.errorsToolStripMenuItem.Checked = true;
            this.errorsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.errorsToolStripMenuItem.Name = "errorsToolStripMenuItem";
            this.errorsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.errorsToolStripMenuItem.Text = "Errors";
            // 
            // performanceToolStripMenuItem
            // 
            this.performanceToolStripMenuItem.Checked = true;
            this.performanceToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.performanceToolStripMenuItem.Name = "performanceToolStripMenuItem";
            this.performanceToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.performanceToolStripMenuItem.Text = "Performance";
            // 
            // compatibilityToolStripMenuItem
            // 
            this.compatibilityToolStripMenuItem.Checked = true;
            this.compatibilityToolStripMenuItem.CheckOnClick = true;
            this.compatibilityToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.compatibilityToolStripMenuItem.Name = "compatibilityToolStripMenuItem";
            this.compatibilityToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.compatibilityToolStripMenuItem.Text = "Compatibility";
            // 
            // otherToolStripMenuItem
            // 
            this.otherToolStripMenuItem.Checked = true;
            this.otherToolStripMenuItem.CheckOnClick = true;
            this.otherToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            this.otherToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.otherToolStripMenuItem.Text = "Other";
            // 
            // markerToolStripMenuItem
            // 
            this.markerToolStripMenuItem.Checked = true;
            this.markerToolStripMenuItem.CheckOnClick = true;
            this.markerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.markerToolStripMenuItem.Name = "markerToolStripMenuItem";
            this.markerToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.markerToolStripMenuItem.Text = "Marker";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // EngineOutputWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 695);
            this.Controls.Add(this.richTextBox1);
            this.Name = "EngineOutputWindow";
            this.ShowIcon = false;
            this.Text = "Engine Output";
            this.Load += new System.EventHandler(this.EngineOutputWindow_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem errorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem performanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compatibilityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markerToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
    }
}