using Kokoro2.Engine;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Akane
{
    public class AkaneManager
    {
        private Form1 GameWindow;

        public Action<double, AkaneManager> Update { get; set; }
        public Action<double, AkaneManager> Render { get; set; }
        public Action<AkaneManager> Initialize { get; set; }

        public float AspectRatio;
        public string Title
        {
            get
            {
                return GameWindow.Text;
            }
            set
            {
                GameWindow.BeginInvoke(new MethodInvoker(() => { GameWindow.Text = value; }));
            }
        }
        public Matrix4 Projection { get { return context.Projection; } set { context.Projection = value; } }

        internal GraphicsContext context;
        public AkaneManager(Vector2 WindowSize)
        {
            GameWindow = new Form1();
            GameWindow.ClientSize = new System.Drawing.Size((int)WindowSize.X, (int)WindowSize.Y);

            context = new GraphicsContext(WindowSize);
            context.ViewportControl.Dock = DockStyle.Fill;
            GameWindow.Controls.Add(context.ViewportControl);

        }

        public void Start()
        {
            context.Update += _update;
            context.Render += _render;
            context.Initialize += _init;

            context.Start(160000, 160000);
            GameWindow.ShowDialog();


        }

        public void Clear(Vector4 col)
        {
            context.ClearColor(col);
        }

        private void _update(double time, GraphicsContext context)
        {
            World.Input.Update();
            Update(time, this);
        }

        private void _render(double time, GraphicsContext context)
        {
            Render(time, this);
        }

        private void _init(GraphicsContext context)
        {
            context.Camera = new Kokoro2.Engine.HighLevel.Cameras.Camera(context);
            context.View = Matrix4.Identity;
            context.DepthFunction = DepthFunc.Always;
            //context.DepthWrite = false;
            //context.View = Matrix4.LookAt(Vector3.Zero - Vector3.UnitZ, Vector3.Zero, Vector3.UnitY);
            //context.Wireframe = true;

            context.Blending = new BlendFunc()
            {
                Src = BlendingFactor.SrcAlpha,
                Dst = BlendingFactor.OneMinusSrcAlpha
            };

            Initialize(this);
        }

    }
}
