using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kokoro2.Debug
{
    public partial class LogViewer : Form
    {
        public bool Pause;
        public bool Active;

        public LogViewer()
        {
            InitializeComponent();
        }

        private void UpdateRichTextBox(RichTextBox control, string message, Severity severity)
        {
            if (!Active) return;
            switch (severity)
            {
                case Severity.High:
                    control.ForeColor = Color.DarkRed;
                    break;
                case Severity.Low:
                    control.ForeColor = Color.Green;
                    break;
                case Severity.Medium:
                    control.ForeColor = Color.Orange;
                    break;
                case Severity.Notification:
                    control.ForeColor = Color.Black;
                    break;
            }

            //Clear the box if there's too much info
            if (control.Lines.Length > 1500) control.Text = "";

            control.Text += message + "\n";

        }

        public void NewMessage(string message, DebugType type, Severity severity)
        {
            if (!Active) return;
            this.BeginInvoke(new MethodInvoker(() =>
                    {
                        switch (type)
                        {
                            case DebugType.Compatibility:
                                UpdateRichTextBox(CompatibilityBox, message, severity);
                                break;
                            case DebugType.Error:
                                UpdateRichTextBox(ErrorsBox, message, severity);
                                break;
                            case DebugType.Marker:
                                UpdateRichTextBox(MarkerBox, message, severity);
                                break;
                            case DebugType.Other:
                                UpdateRichTextBox(OtherBox, message, severity);
                                break;
                            case DebugType.Performance:
                                UpdateRichTextBox(PerformanceBox, message, severity);
                                break;
                            case DebugType.Undefined:
                                UpdateRichTextBox(ErrorsBox, message, severity);
                                break;
                        }
                    }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Active) return;
            Pause = !Pause;
        }

        private void LogViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Active) return;
            Pause = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!Active) return;
            e.Cancel = true;
            //base.OnClosing(e);
        }
    }
}
