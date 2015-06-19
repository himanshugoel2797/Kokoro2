#if OPENGL
using Kokoro3.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GLControl = OpenTK.GLControl;
using DisplayDevice = OpenTK.DisplayDevice;
using Kokoro3.OpenGL;
using System.Diagnostics;


//TODO use AMD Compressor for Kokoro3.Sandbox
//TODO Finish RenderSurface implementation
//TODO setup shader uniform system - implement TextureSlot management class and make it cooperate with LLDevice so the engine can figure out which slot to use to minimize binding switches
//TODO finish multidrawindirect with buffertextures
//TODO write test for multidrawindirect
//TODO bring over primitive generation
//TODO setup compute shader pipeline
//TODO setup physics
//TODO combine physics, audio and graphics into entity

namespace Kokoro3.Common
{
    public class GameDevice
    {
        public SceneDevice CurrentSceneDevice;
        public string Name;

        public Control GraphicsControl;

        public Action<double> Update;
        public Action<double> Render;

        private Action<GameDevice> initializer;
        System.IO.FileStream tmp = System.IO.File.Create("EngineLog.txt");

        public GameDevice()
        {
            System.Diagnostics.Debug.Listeners.Add(new TextWriterTraceListener(tmp));
            System.Diagnostics.Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine("Engine Started");
            GraphicsControl = new GLControl();
        }

        #region Gameloop
        public void Start(Action<GameDevice> initialize)
        {
            GraphicsControl.Paint += GraphicsControl_Paint;
            Application.Idle += Application_Idle;
            initializer = initialize;
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            if (Update != null) Update(16);
            GraphicsControl.Invalidate();
        }

        private void GraphicsControl_Paint(object sender, PaintEventArgs e)
        {
            if (initializer != null)
            {
                initializer(this);
                initializer = null;
            }
            GraphicsDevice.CheckError();
            if (Render != null) Render(16);
        }
        #endregion

        #region Graphics
        #region Display Management
        public void SetResolution(Vector4 res) { SetResolution((int)res.X, (int)res.Y, (int)res.Z, res.W); }
        public void SetResolution(int width, int height, int bpp, float refreshRate)
        {
            DisplayDevice.Default.ChangeResolution(width, height, bpp, refreshRate);
        }

        public Vector4[] EnumerateSupportedResolutions()
        {
            var tmp = DisplayDevice.Default.AvailableResolutions;
            Vector4[] toRet = new Vector4[tmp.Count];

            for (int i = 0; i < toRet.Length; i++)
            {
                toRet[i] = new Vector4(tmp[i].Width, tmp[i].Height, tmp[i].BitsPerPixel, tmp[i].RefreshRate);
            }
            return toRet;
        }
        #endregion

        #region Buffer management
        public void SwapBuffer()
        {
            (GraphicsControl as GLControl).SwapBuffers();
        }
        Vector4 color;
        public Vector4 ClearColor
        {
            get { return color; }
            set
            {
                color = value;
                GraphicsDevice.SetClearColor(value);
            }
        }
        public void ClearColorBuffer() { GraphicsDevice.ClearColorBuffer(); }
        public void ClearDepthBuffer() { GraphicsDevice.ClearDepthBuffer(); }
        #endregion
        #endregion

        ~GameDevice()
        {
            tmp.Dispose();
        }
    }
}
#endif