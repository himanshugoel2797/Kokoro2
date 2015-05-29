namespace Kokoro2.Debug
{
    partial class MemoryMonitor
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemoryMonitor));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.objList = new System.Windows.Forms.TabPage();
            this.objProperties = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.perfMon = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.texView = new System.Windows.Forms.TabPage();
            this.objectsList = new System.Windows.Forms.ListView();
            this.textureBox = new System.Windows.Forms.PictureBox();
            this.glStatus = new System.Windows.Forms.TabPage();
            this.glStatusList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.logicViewer = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.objList.SuspendLayout();
            this.perfMon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.texView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textureBox)).BeginInit();
            this.glStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.objList);
            this.tabControl1.Controls.Add(this.perfMon);
            this.tabControl1.Controls.Add(this.texView);
            this.tabControl1.Controls.Add(this.glStatus);
            this.tabControl1.Controls.Add(this.logicViewer);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(879, 561);
            this.tabControl1.TabIndex = 0;
            // 
            // objList
            // 
            this.objList.Controls.Add(this.objProperties);
            this.objList.Location = new System.Drawing.Point(4, 22);
            this.objList.Name = "objList";
            this.objList.Padding = new System.Windows.Forms.Padding(3);
            this.objList.Size = new System.Drawing.Size(871, 535);
            this.objList.TabIndex = 0;
            this.objList.Text = "Objects";
            this.objList.UseVisualStyleBackColor = true;
            // 
            // objProperties
            // 
            this.objProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader8,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.objProperties.Location = new System.Drawing.Point(8, 7);
            this.objProperties.Name = "objProperties";
            this.objProperties.Size = new System.Drawing.Size(855, 520);
            this.objProperties.TabIndex = 1;
            this.objProperties.UseCompatibleStateImageBehavior = false;
            this.objProperties.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            this.columnHeader3.Width = 191;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Single Size (bytes)";
            this.columnHeader8.Width = 147;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Count";
            this.columnHeader4.Width = 68;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Allocated (Total)";
            this.columnHeader5.Width = 102;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Allocated (Frame)";
            this.columnHeader6.Width = 108;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Destroyed (Frame)";
            this.columnHeader7.Width = 113;
            // 
            // perfMon
            // 
            this.perfMon.Controls.Add(this.chart1);
            this.perfMon.Location = new System.Drawing.Point(4, 22);
            this.perfMon.Name = "perfMon";
            this.perfMon.Padding = new System.Windows.Forms.Padding(3);
            this.perfMon.Size = new System.Drawing.Size(871, 535);
            this.perfMon.TabIndex = 1;
            this.perfMon.Text = "Performance Monitor";
            this.perfMon.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.AxisX.Interval = 60D;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.Maximum = 3600D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.Title = "Frame";
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.Maximum = 200D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.AxisY.MinorTickMark.Enabled = true;
            chartArea1.AxisY.Title = "Milliseconds";
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.Name = "Update";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Legend = "Legend1";
            series2.Name = "Render";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(865, 529);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // texView
            // 
            this.texView.Controls.Add(this.objectsList);
            this.texView.Controls.Add(this.textureBox);
            this.texView.Location = new System.Drawing.Point(4, 22);
            this.texView.Name = "texView";
            this.texView.Size = new System.Drawing.Size(871, 535);
            this.texView.TabIndex = 2;
            this.texView.Text = "Texture Viewer";
            this.texView.UseVisualStyleBackColor = true;
            // 
            // objectsList
            // 
            this.objectsList.Location = new System.Drawing.Point(9, 4);
            this.objectsList.Name = "objectsList";
            this.objectsList.Size = new System.Drawing.Size(224, 523);
            this.objectsList.TabIndex = 1;
            this.objectsList.UseCompatibleStateImageBehavior = false;
            this.objectsList.View = System.Windows.Forms.View.List;
            this.objectsList.SelectedIndexChanged += new System.EventHandler(this.objectsList_SelectedIndexChanged);
            // 
            // textureBox
            // 
            this.textureBox.Location = new System.Drawing.Point(239, 4);
            this.textureBox.Name = "textureBox";
            this.textureBox.Size = new System.Drawing.Size(624, 523);
            this.textureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.textureBox.TabIndex = 0;
            this.textureBox.TabStop = false;
            // 
            // glStatus
            // 
            this.glStatus.Controls.Add(this.glStatusList);
            this.glStatus.Location = new System.Drawing.Point(4, 22);
            this.glStatus.Name = "glStatus";
            this.glStatus.Size = new System.Drawing.Size(871, 535);
            this.glStatus.TabIndex = 3;
            this.glStatus.Text = "GL Status";
            this.glStatus.UseVisualStyleBackColor = true;
            // 
            // glStatusList
            // 
            this.glStatusList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.glStatusList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glStatusList.Location = new System.Drawing.Point(0, 0);
            this.glStatusList.Name = "glStatusList";
            this.glStatusList.Size = new System.Drawing.Size(871, 535);
            this.glStatusList.TabIndex = 0;
            this.glStatusList.UseCompatibleStateImageBehavior = false;
            this.glStatusList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Property";
            this.columnHeader1.Width = 329;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 521;
            // 
            // logicViewer
            // 
            this.logicViewer.Location = new System.Drawing.Point(4, 22);
            this.logicViewer.Name = "logicViewer";
            this.logicViewer.Padding = new System.Windows.Forms.Padding(3);
            this.logicViewer.Size = new System.Drawing.Size(871, 535);
            this.logicViewer.TabIndex = 4;
            this.logicViewer.Text = "Logic Viewer";
            this.logicViewer.UseVisualStyleBackColor = true;
            // 
            // MemoryMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 561);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MemoryMonitor";
            this.Text = "Memory Monitor";
            this.tabControl1.ResumeLayout(false);
            this.objList.ResumeLayout(false);
            this.perfMon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.texView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textureBox)).EndInit();
            this.glStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage objList;
        private System.Windows.Forms.TabPage perfMon;
        private System.Windows.Forms.TabPage texView;
        private System.Windows.Forms.TabPage glStatus;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ListView glStatusList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.PictureBox textureBox;
        private System.Windows.Forms.ListView objProperties;
        private System.Windows.Forms.TabPage logicViewer;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ListView objectsList;
    }
}