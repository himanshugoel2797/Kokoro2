namespace Kokoro2.Debug
{
    partial class LogViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogViewer));
            this.button1 = new System.Windows.Forms.Button();
            this.viewer = new System.Windows.Forms.TabControl();
            this.Errors = new System.Windows.Forms.TabPage();
            this.ErrorsBox = new System.Windows.Forms.RichTextBox();
            this.Performance = new System.Windows.Forms.TabPage();
            this.PerformanceBox = new System.Windows.Forms.RichTextBox();
            this.Marker = new System.Windows.Forms.TabPage();
            this.MarkerBox = new System.Windows.Forms.RichTextBox();
            this.Compatibility = new System.Windows.Forms.TabPage();
            this.CompatibilityBox = new System.Windows.Forms.RichTextBox();
            this.Other = new System.Windows.Forms.TabPage();
            this.OtherBox = new System.Windows.Forms.RichTextBox();
            this.viewer.SuspendLayout();
            this.Errors.SuspendLayout();
            this.Performance.SuspendLayout();
            this.Marker.SuspendLayout();
            this.Compatibility.SuspendLayout();
            this.Other.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 538);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Pause/Resume";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // viewer
            // 
            this.viewer.Controls.Add(this.Errors);
            this.viewer.Controls.Add(this.Performance);
            this.viewer.Controls.Add(this.Marker);
            this.viewer.Controls.Add(this.Compatibility);
            this.viewer.Controls.Add(this.Other);
            this.viewer.Location = new System.Drawing.Point(13, 13);
            this.viewer.Name = "viewer";
            this.viewer.SelectedIndex = 0;
            this.viewer.Size = new System.Drawing.Size(541, 519);
            this.viewer.TabIndex = 2;
            // 
            // Errors
            // 
            this.Errors.Controls.Add(this.ErrorsBox);
            this.Errors.Location = new System.Drawing.Point(4, 22);
            this.Errors.Name = "Errors";
            this.Errors.Padding = new System.Windows.Forms.Padding(3);
            this.Errors.Size = new System.Drawing.Size(533, 493);
            this.Errors.TabIndex = 0;
            this.Errors.Text = "Errors";
            this.Errors.UseVisualStyleBackColor = true;
            // 
            // ErrorsBox
            // 
            this.ErrorsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorsBox.Location = new System.Drawing.Point(3, 3);
            this.ErrorsBox.Name = "ErrorsBox";
            this.ErrorsBox.ReadOnly = true;
            this.ErrorsBox.Size = new System.Drawing.Size(527, 487);
            this.ErrorsBox.TabIndex = 0;
            this.ErrorsBox.Text = "";
            // 
            // Performance
            // 
            this.Performance.Controls.Add(this.PerformanceBox);
            this.Performance.Location = new System.Drawing.Point(4, 22);
            this.Performance.Name = "Performance";
            this.Performance.Padding = new System.Windows.Forms.Padding(3);
            this.Performance.Size = new System.Drawing.Size(533, 493);
            this.Performance.TabIndex = 1;
            this.Performance.Text = "Performance";
            this.Performance.UseVisualStyleBackColor = true;
            // 
            // PerformanceBox
            // 
            this.PerformanceBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PerformanceBox.Location = new System.Drawing.Point(3, 3);
            this.PerformanceBox.Name = "PerformanceBox";
            this.PerformanceBox.Size = new System.Drawing.Size(527, 487);
            this.PerformanceBox.TabIndex = 1;
            this.PerformanceBox.Text = "";
            // 
            // Marker
            // 
            this.Marker.Controls.Add(this.MarkerBox);
            this.Marker.Location = new System.Drawing.Point(4, 22);
            this.Marker.Name = "Marker";
            this.Marker.Size = new System.Drawing.Size(533, 493);
            this.Marker.TabIndex = 2;
            this.Marker.Text = "Marker";
            this.Marker.UseVisualStyleBackColor = true;
            // 
            // MarkerBox
            // 
            this.MarkerBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MarkerBox.Location = new System.Drawing.Point(0, 0);
            this.MarkerBox.Name = "MarkerBox";
            this.MarkerBox.Size = new System.Drawing.Size(533, 493);
            this.MarkerBox.TabIndex = 1;
            this.MarkerBox.Text = "";
            // 
            // Compatibility
            // 
            this.Compatibility.Controls.Add(this.CompatibilityBox);
            this.Compatibility.Location = new System.Drawing.Point(4, 22);
            this.Compatibility.Name = "Compatibility";
            this.Compatibility.Size = new System.Drawing.Size(533, 493);
            this.Compatibility.TabIndex = 4;
            this.Compatibility.Text = "Compatibility";
            this.Compatibility.UseVisualStyleBackColor = true;
            // 
            // CompatibilityBox
            // 
            this.CompatibilityBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CompatibilityBox.Location = new System.Drawing.Point(0, 0);
            this.CompatibilityBox.Name = "CompatibilityBox";
            this.CompatibilityBox.Size = new System.Drawing.Size(533, 493);
            this.CompatibilityBox.TabIndex = 1;
            this.CompatibilityBox.Text = "";
            // 
            // Other
            // 
            this.Other.Controls.Add(this.OtherBox);
            this.Other.Location = new System.Drawing.Point(4, 22);
            this.Other.Name = "Other";
            this.Other.Size = new System.Drawing.Size(533, 493);
            this.Other.TabIndex = 3;
            this.Other.Text = "Other";
            this.Other.UseVisualStyleBackColor = true;
            // 
            // OtherBox
            // 
            this.OtherBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OtherBox.Location = new System.Drawing.Point(0, 0);
            this.OtherBox.Name = "OtherBox";
            this.OtherBox.Size = new System.Drawing.Size(533, 493);
            this.OtherBox.TabIndex = 1;
            this.OtherBox.Text = "";
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 573);
            this.Controls.Add(this.viewer);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogViewer";
            this.Text = "Log Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogViewer_FormClosing);
            this.viewer.ResumeLayout(false);
            this.Errors.ResumeLayout(false);
            this.Performance.ResumeLayout(false);
            this.Marker.ResumeLayout(false);
            this.Compatibility.ResumeLayout(false);
            this.Other.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl viewer;
        private System.Windows.Forms.TabPage Errors;
        private System.Windows.Forms.TabPage Performance;
        private System.Windows.Forms.TabPage Marker;
        private System.Windows.Forms.TabPage Other;
        private System.Windows.Forms.TabPage Compatibility;
        private System.Windows.Forms.RichTextBox ErrorsBox;
        private System.Windows.Forms.RichTextBox PerformanceBox;
        private System.Windows.Forms.RichTextBox MarkerBox;
        private System.Windows.Forms.RichTextBox CompatibilityBox;
        private System.Windows.Forms.RichTextBox OtherBox;
    }
}