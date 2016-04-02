using Kokoro.IDE.Editor;
using Kokoro2.Engine;
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
    public partial class EngineOutputWindow : Form
    {
        EngineOutputLogger logger;

        public EngineOutputWindow(EngineOutputLogger logger)
        {
            this.logger = logger;
            InitializeComponent();
        }

        private void UpdateRichTextBox(RichTextBox control, string message, Severity severity)
        {
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

            control.AppendText(message + "\n");

        }

        private void NewMessage(string message, DebugType type, Severity severity)
        {
            string id = message.Split(',')[0].Split(']').Last();
            string msg = message.Split(',')[0];
            string msg2 = message.Replace(msg + ",", "");
            msg = msg.Remove(msg.LastIndexOf(']') + 1);
            message = $"[ID = {id}]{msg}{msg2}";

            switch (type)
            {
                case DebugType.Compatibility:
                    if (compatibilityToolStripMenuItem.Checked) UpdateRichTextBox(richTextBox1, message, severity);
                    break;
                case DebugType.Error:
                    if (errorsToolStripMenuItem.Checked) UpdateRichTextBox(richTextBox1, message, severity);
                    break;
                case DebugType.Marker:
                    if (markerToolStripMenuItem.Checked) UpdateRichTextBox(richTextBox1, message, severity);
                    break;
                case DebugType.Other:
                    if (otherToolStripMenuItem.Checked) UpdateRichTextBox(richTextBox1, message, severity);
                    break;
                case DebugType.Performance:
                    if (performanceToolStripMenuItem.Checked) UpdateRichTextBox(richTextBox1, message, severity);
                    break;
                case DebugType.Undefined:
                    UpdateRichTextBox(richTextBox1, message, severity);
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            while (logger.messages.Count > 0)
            {
                var d = logger.messages.Dequeue();
                NewMessage(d.message, d.debType, d.severity);
            }
        }

        private void EngineOutputWindow_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
