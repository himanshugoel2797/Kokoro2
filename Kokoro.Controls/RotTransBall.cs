using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kokoro.Controls
{
    public partial class RotTransBall : UserControl
    {
        private bool rotMode = true;   //true = rotation, false = translation

        public float XAngle { get; set; }
        public float YAngle { get; set; }
        public float ZAngle { get; set; }

        public float XPosition { get; set; }
        public float YPosition { get; set; }
        public float ZPosition { get; set; }

        public float AngleStep { get; set; }
        public float TranslationStep { get; set; }

        public RotTransBall()
        {
            InitializeComponent();

            AngleStep = 0.25f;
            TranslationStep = 0.2f;

            xRot_valBox.Text = xTrans_valBox.Text = yRot_valBox.Text = yTrans_valBox.Text = zRot_valBox.Text = zTrans_valBox.Text = "0";
        }

        private void UpdateBars()
        {
            if (XAngle < -Math.PI) XAngle = (float)(Math.PI - (-XAngle % Math.PI));
            if (YAngle < -Math.PI) YAngle = (float)(Math.PI - (-YAngle % Math.PI));
            if (ZAngle < -Math.PI) ZAngle = (float)(Math.PI - (-ZAngle % Math.PI));

            if (XAngle > Math.PI) XAngle = (float)(-Math.PI + (XAngle % Math.PI));
            if (YAngle > Math.PI) YAngle = (float)(-Math.PI + (YAngle % Math.PI));
            if (ZAngle > Math.PI) ZAngle = (float)(-Math.PI + (ZAngle % Math.PI));


            xRot_valBox.Text = XAngle.ToString();
            yRot_valBox.Text = YAngle.ToString();
            zRot_valBox.Text = ZAngle.ToString();


            if (rotMode)
            {
                XtrackBar.Maximum = YtrackBar.Maximum = ZtrackBar.Maximum = (int)Math.Ceiling(Math.PI / AngleStep);
                XtrackBar.Minimum = YtrackBar.Minimum = ZtrackBar.Minimum = (int)Math.Ceiling(-Math.PI / AngleStep);


                XtrackBar.Value = (int)(XAngle / AngleStep);
                YtrackBar.Value = (int)(YAngle / AngleStep);
                ZtrackBar.Value = (int)(ZAngle / AngleStep);
            }
            else
            {
                XtrackBar.Value = YtrackBar.Value = ZtrackBar.Value = 0;

                XtrackBar.Maximum = (int)10;
                XtrackBar.Minimum = -XtrackBar.Maximum;

                YtrackBar.Maximum = (int)10;
                YtrackBar.Minimum = -YtrackBar.Maximum;

                ZtrackBar.Maximum = (int)10;
                ZtrackBar.Minimum = -ZtrackBar.Maximum;
            }
        }

        private void rotSelector_CheckedChanged(object sender, EventArgs e)
        {
            rotMode = rotSelector.Checked;
            UpdateBars();
        }

        private void XtrackBar_Scroll(object sender, EventArgs e)
        {
            if (rotMode)
            {
                XAngle = XtrackBar.Value * AngleStep;
                xRot_valBox.Text = XAngle.ToString();
            }
            else
            {
                XPosition += XtrackBar.Value * TranslationStep;
                xTrans_valBox.Text = XPosition.ToString();
                XtrackBar.Value = 0;
            }
        }

        private void YtrackBar_Scroll(object sender, EventArgs e)
        {
            if (rotMode)
            {
                YAngle = YtrackBar.Value * AngleStep;
                yRot_valBox.Text = YAngle.ToString();
            }
            else
            {
                YPosition += YtrackBar.Value * TranslationStep;
                yTrans_valBox.Text = YPosition.ToString();
                YtrackBar.Value = 0;
            }
        }

        private void ZtrackBar_Scroll(object sender, EventArgs e)
        {
            if (rotMode)
            {
                ZAngle = ZtrackBar.Value * AngleStep;
                zRot_valBox.Text = ZAngle.ToString();
            }
            else
            {
                ZPosition += ZtrackBar.Value * TranslationStep;
                zTrans_valBox.Text = ZPosition.ToString();
                ZtrackBar.Value = 0;
            }
        }

        private void Box_TextChanged(object sender, EventArgs e)
        {
            try
            {
                XAngle = float.Parse(xRot_valBox.Text);
            }
            catch (Exception e2) { }

            try
            {
                YAngle = float.Parse(yRot_valBox.Text);
            }
            catch (Exception e2) { }

            try
            {
                ZAngle = float.Parse(zRot_valBox.Text);
            }
            catch (Exception e2) { }

            try
            {
                XPosition = float.Parse(xTrans_valBox.Text);
            }
            catch (Exception e2) { }

            try
            {
                YPosition = float.Parse(yTrans_valBox.Text);
            }
            catch (Exception e2) { }

            try
            {
                ZPosition = float.Parse(zTrans_valBox.Text);
            }
            catch (Exception e2) { }

            UpdateBars();
        }
    }
}
