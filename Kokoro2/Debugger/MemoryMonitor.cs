using Kokoro2.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#if OPENGL
#if PC
using Kokoro2.OpenGL.PC;
using deb = Kokoro2.OpenGL.PC.Debug;
#endif
#endif

namespace Kokoro2.Debug
{
    public partial class MemoryMonitor : Form
    {
        public bool Active;

        public MemoryMonitor()
        {
            InitializeComponent();


            glStatusList.Items.Clear();
            glStatusList.Items.Add("WireFrame");

            glStatusList.Items.Add("DepthWrite");
            glStatusList.Items.Add("FaceCulling");
            glStatusList.Items.Add("DepthFunction");
            glStatusList.Items.Add("Projection");
            glStatusList.Items.Add("View");
            glStatusList.Items.Add("ZFar");
            glStatusList.Items.Add("ZNear");
            glStatusList.Items.Add("MSAALevel");
            glStatusList.Items.Add("Viewport");

            glStatusList.Items[0].SubItems.Add(
                    new ListViewItem.ListViewSubItem(glStatusList.Items[0], "")
                    );

            glStatusList.Items[1].SubItems.Add(
                new ListViewItem.ListViewSubItem(glStatusList.Items[1], ""));
            glStatusList.Items[2].SubItems.Add(
                new ListViewItem.ListViewSubItem(glStatusList.Items[2], ""));
            glStatusList.Items[3].SubItems.Add(
                new ListViewItem.ListViewSubItem(glStatusList.Items[3], "")
                );
            glStatusList.Items[4].SubItems.Add(
                new ListViewItem.ListViewSubItem(glStatusList.Items[4], "")
                );
            glStatusList.Items[5].SubItems.Add(
                new ListViewItem.ListViewSubItem(glStatusList.Items[5], "")
                );
            glStatusList.Items[6].SubItems.Add(
                new ListViewItem.ListViewSubItem(glStatusList.Items[6], "")
                );
            glStatusList.Items[7].SubItems.Add(
                new ListViewItem.ListViewSubItem(glStatusList.Items[7], "")
                );
            glStatusList.Items[8].SubItems.Add(
                new ListViewItem.ListViewSubItem(glStatusList.Items[8], "")
                );
            glStatusList.Items[9].SubItems.Add(
     new ListViewItem.ListViewSubItem(glStatusList.Items[9], "")
     );

        }

        #region Performance Monitor
        double upFrames = 0, renFrames = 0;
        public void PostMSPU(double mspu)
        {
            /*this.Invoke(new MethodInvoker(() =>
               {
                   if (upFrames % 10 == 0)
                   {
                       chart1.Series["Update"].Points.AddXY(upFrames, mspu);
                   }
                   upFrames++;
               }));*/
        }


        //TODO Apped performance data to a CSV file for logging purposes
        public void PostMSPR(double mspr)
        {
            /*this.Invoke(new MethodInvoker(() =>
            {
                if (renFrames % 10 == 0)
                {
                    chart1.Series["Render"].Points.AddXY(renFrames, mspr);
                }
                renFrames++;
                if (renFrames == chart1.ChartAreas[0].AxisX.Minimum + 3400)
                {
                    chart1.ChartAreas[0].AxisX.Minimum += 3400;
                    chart1.ChartAreas[0].AxisX.Maximum += 3400;
                    chart1.Series["Render"].Points.Clear();
                    chart1.Series["Update"].Points.Clear();
                }
            }));*/
        }
        #endregion

        #region GL Status
        public void PostGLStatus(GraphicsContext context)
        {
            if (!Active) return;
            this.BeginInvoke(new MethodInvoker(() =>
            {
                glStatusList.Items[0].SubItems[1] =
                    new ListViewItem.ListViewSubItem(glStatusList.Items[0], context.Wireframe.ToString());

                glStatusList.Items[1].SubItems[1] =
                    new ListViewItem.ListViewSubItem(glStatusList.Items[1], context.DepthWrite.ToString());
                glStatusList.Items[2].SubItems[1] =
                    new ListViewItem.ListViewSubItem(glStatusList.Items[2], context.FaceCulling.ToString());
                glStatusList.Items[3].SubItems[1] =
                    new ListViewItem.ListViewSubItem(glStatusList.Items[3], context.DepthFunction.ToString())
                   ;
                glStatusList.Items[4].SubItems[1] =
                    new ListViewItem.ListViewSubItem(glStatusList.Items[4], context.Projection.ToString())
                   ;
                glStatusList.Items[5].SubItems[1] =
                    new ListViewItem.ListViewSubItem(glStatusList.Items[5], context.View.ToString())
                   ;
                glStatusList.Items[6].SubItems[1] =
                    new ListViewItem.ListViewSubItem(glStatusList.Items[6], context.ZFar.ToString())
                   ;
                glStatusList.Items[7].SubItems[1] =
                    new ListViewItem.ListViewSubItem(glStatusList.Items[7], context.ZNear.ToString())
                   ;
                glStatusList.Items[8].SubItems[1] =
                    new ListViewItem.ListViewSubItem(glStatusList.Items[8], context.MSAALevel.ToString())
                   ;
                glStatusList.Items[9].SubItems[1] =
                    new ListViewItem.ListViewSubItem(glStatusList.Items[9], context.Viewport.ToString());
            }
            ));
        }
        #endregion

        #region Object Alloc Viewer
        private Dictionary<Type, int> totalCounts = new Dictionary<Type, int>();
        private Dictionary<Type, int> objects = new Dictionary<Type, int>();
        private Dictionary<Type, int> curFrameObjectsAlloc = new Dictionary<Type, int>();
        private Dictionary<Type, int> curFrameObjectsDeAlloc = new Dictionary<Type, int>();
        private Dictionary<Type, int> sizes = new Dictionary<Type, int>();
        public void ObjectAllocated(Type type)
        {
            if (!Active) return;
            if (!objects.ContainsKey(type))
            {
                sizes.Add(type, 0);
                objects.Add(type, 0);
                curFrameObjectsAlloc.Add(type, 0);
                curFrameObjectsDeAlloc.Add(type, 0);
                totalCounts.Add(type, 0);
            }
            objects[type]++;    //New instance has been allocated, so increase allocation count
            totalCounts[type]++;
            curFrameObjectsAlloc[type]++;   //Update instances allocated for this frame
            UpdateList();
        }

        public void ObjectFreed(Type type)
        {

            if (!Active) return;
            if (!objects.ContainsKey(type))
            {
                sizes.Add(type, 0);
                objects.Add(type, 1);
                curFrameObjectsAlloc.Add(type, 1);
                curFrameObjectsDeAlloc.Add(type, 0);
                totalCounts.Add(type, 1);
            }

            objects[type]--;
            curFrameObjectsDeAlloc[type]++;
            sizes[type]++;
            UpdateList();
        }

        private void UpdateList()
        {
            if (!Active) return;
            try
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    objProperties.Items.Clear();

                    foreach (Type t in objects.Keys.ToArray())
                    {
                        objProperties.Items.Add(

                            new ListViewItem(new string[] {
                            t.Name,
                            sizes[t].ToString(),
                            totalCounts[t].ToString(),
                            objects[t].ToString(),
                            curFrameObjectsAlloc[t].ToString(),
                            curFrameObjectsDeAlloc[t].ToString() })

                            );

                    }
                }
                ));
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        public void MarkLoop()
        {
            if (!Active) return;
            foreach (Type key in objects.Keys.ToArray())
            {
                curFrameObjectsAlloc[key] = 0;
                curFrameObjectsDeAlloc[key] = 0;
            }
        }
        #endregion

        #region Texture Viewer
        Dictionary<string, int> textures = new Dictionary<string, int>();
        public void AddTexture(string name, int id)
        {
            try
            {
                if (!textures.ContainsKey(name)) textures.Add(name, id);
                else textures[name] = id;

                objectsList.Items.Add(name, name, 0);
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }
        public void RemoveTexture(string name)
        {
            try
            {
                if (textures.ContainsKey(name)) textures.Remove(name);

                for (int i = 0; i < objectsList.Items.Count; i++)
                {
                    if (objectsList.Items[i].Text.StartsWith(name))
                    {
                        objectsList.Items.RemoveAt(i);
                        i = objectsList.Items.Count;
                    }
                }
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }
        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!Active) return;
            //base.OnClosing(e);
            e.Cancel = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (!Active) return;
            //base.OnClosed(e);
        }

        private void objectsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Active) return;
#if DEBUG
            if (objectsList.SelectedIndices.Count > 0) textureBox.Image = deb.TexToBMP(textures[objectsList.Items[objectsList.SelectedIndices[0]].Text]);
#endif
        }

    }
}
