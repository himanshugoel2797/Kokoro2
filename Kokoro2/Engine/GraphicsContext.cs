using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

using Kokoro2.Engine.HighLevel.Cameras;
using Kokoro2.Math;
using Kokoro2.Engine.Input;

//Conditional compilation used to manage multiplatform support
#if OPENGL
using Kokoro2.OpenGL;    //If building for OpenGL
#if PC
using Kokoro2.OpenGL.PC;
using System.Windows.Forms;     //If building for OpenGL on PC (Windows, Linux, Mac)
#endif

#elif OPENGL_AZDO
#if PC
using Kokoro2.OpenGL.AZDO;
#endif

#endif

namespace Kokoro2.Engine
{
    /// <summary>
    /// The Face Culling Modes
    /// </summary>
    public enum CullMode { Off, Back, Front }

    /// <summary>
    /// The Blending Factors
    /// </summary>
    public enum BlendingFactor
    {
        Zero = 0,
        One,
        SrcAlpha,
        OneMinusSrcAlpha,
        DstAlpha,
        OneMinusDstAlpha,
        DstColor,
        OneMinusDstColor,
        SrcAlphaSaturate,
        ConstantColorExt,
        ConstantColor,
        OneMinusConstantColorExt,
        OneMinusConstantColor,
        ConstantAlphaExt,
        ConstantAlpha,
        OneMinusConstantAlpha,
        OneMinusConstantAlphaExt,
        Src1Alpha,
        Src1Color,
        OneMinusSrc1Color,
        OneMinusSrc1Alpha
    }

    /// <summary>
    /// The Blend Function
    /// </summary>
    public struct BlendFunc
    {
        public BlendingFactor Src;
        public BlendingFactor Dst;
    }

    /// <summary>
    /// The GraphicsContext acts as a high level wrapper to the lower level functionality exposed by the platform dependant code
    /// </summary>
    public class GraphicsContext : GraphicsContextLL
    {
        #region State Machine Properties
        /// <summary>
        /// Enable/Disable Wireframe rendering
        /// </summary>
        public bool Wireframe
        {
            get
            {
                return base.GetWireframe();
            }
            set
            {
                base.SetWireframe(value);
            }
        }

        public bool TransformFeedback
        {
            get
            {
                return GraphicsContextLL.transformEnabled;
            }
            set
            {
                if (value) GraphicsContextLL.EnableFeedback();
                else GraphicsContextLL.DisableFeedback();
            }
        }

        /// <summary>
        /// Enable/Disable writing to the Depth buffer
        /// </summary>
        public bool DepthWrite
        {
            get
            {
                return base.GetDepthWrite();
            }
            set
            {
                base.SetDepthWrite(value);
            }
        }

        /// <summary>
        /// Enable/Disable face culling
        /// </summary>
        public CullMode FaceCulling
        {
            get
            {
                return base.GetCullMode();
            }
            set
            {
                base.SetCullMode(value);
            }
        }

        /// <summary>
        /// Get/Set the Depth Function
        /// </summary>
        public DepthFunc DepthFunction
        {
            get
            {
                return base.GetDepthFunc();
            }
            set
            {
                base.SetDepthFunc(value);
            }
        }

        /// <summary>
        /// Set the Projection Matrix
        /// </summary>
        public Matrix4 Projection { get
            {
                return Camera.Projection;
            }
            set
            {
                Camera.Projection = value;
            }
            }

        /// <summary>
        /// Set the View Matrix
        /// </summary>
        public Matrix4 View
        {
            get
            {
                return Camera.View;
            }
            set
            {
                Camera.View = value;
            }
        }

        /// <summary>
        /// Set the current Camera
        /// </summary>
        public Camera Camera { get; set; }

        /// <summary>
        /// Get/Set the rendering viewport
        /// </summary>
        public Vector4 Viewport
        {
            get
            {
                return base.GetViewport();
            }
            set
            {
                base.SetViewport(value);
            }
        }

        /// <summary>
        /// Get the aspect ratio of the viewport
        /// </summary>
        public double AspectRatio
        {
            get
            {
                return Viewport.Z / Viewport.W;
            }
        }

        public float PointSize
        {
            set
            {
                SetPointSize(value);
            }
        }

        /// <summary>
        /// Set the Blend function
        /// </summary>
        public BlendFunc Blending
        {
            get
            {
                return GetBlendFunc();
            }
            set
            {
                SetBlendFunc(value);
            }
        }

        public bool Blend
        {
            get
            {
                return GetBlendStatus();
            }
            set
            {
                SetBlendStatus(value);
            }
        }

        /// <summary>
        /// The Far-clipping plane
        /// </summary>
        public float ZFar
        {
            get
            {
                return base.GetZFar();
            }
            set
            {
                base.SetZFar(value);
            }
        }

        /// <summary>
        /// The Near-clipping plane
        /// </summary>
        public float ZNear
        {
            get
            {
                return base.GetZNear();
            }
            set
            {
                base.SetZNear(value);
            }
        }

        /// <summary>
        /// Get/Set the MSAA sample count
        /// </summary>
        public int MSAALevel
        {
            get
            {
                return base.GetMSAALevel();
            }
            set
            {
                base.ResetMSAA();
                base.InitializeMSAA(value);
            }
        }

        /// <summary>
        /// Set the Viewport Window Size
        /// </summary>
        public Vector2 WindowSize
        {
            get
            {
                return GetWinSize();
            }
            set
            {
                SetWinSize(value);
            }
        }

        public int PatchSize
        {
            set
            {
                SetPatchSize(value);
            }
        }
        #endregion

        //TODO fix MSAA
        #region MSAA
        internal void SetMSAABuffer() { base.SetMSAA(); }
        #endregion

        #region Clear
        /// <summary>
        /// Clear the screen
        /// </summary>
        /// <param name="r">The red component from 0 to 1</param>
        /// <param name="g">The green component from 0 to 1</param>
        /// <param name="b">The blue component from 0 to 1</param>
        /// <param name="a">The alpha component from 0 to 1</param>
        public void Clear(float r, float g, float b, float a)
        {
            base.aClear(r, g, b, a);
        }
        /// <summary>
        /// Clear the screen
        /// </summary>
        /// <param name="col">The color to clear the screen with (all 0 to 1)</param>
        public void Clear(Vector4 col) { Clear(col.X, col.Y, col.Z, col.W); }
        #endregion

        #region Draw
        public void Draw(Model m)
        {
            base.AddDraw(m);
        }

        public void DrawBatch(Model[] m, Shaders.ShaderProgram shader)
        {
            base.AddDrawBatch(m, shader);
        }
        #endregion

        #region Game Loop
        /// <summary>
        /// The thread running the Update Loop
        /// </summary>
        public Thread UpdateThread { get; private set; }
        /// <summary>
        /// The thread on which the engine resource manager runs
        /// </summary>
        public Thread ResourceManagerThread { get; private set; }     //NOTE: The resource manager also deals with balancing the world octree, as a result it manages the resources in the tree by unloading any objects which are too far away for current use

        /// <summary>
        /// The Update handler
        /// </summary>
        public Action<double, GraphicsContext> Update { get; set; }
        /// <summary>
        /// The Render handler
        /// </summary>
        public Action<double, GraphicsContext> Render { get; set; }
        /// <summary>
        /// The initialization handler
        /// </summary>
        public Action<GraphicsContext> Initialize { get; set; }
        /// <summary>
        /// The Resource Manager handler - Use for Async resource loading
        /// </summary>
        public Action<GraphicsContext> ResourceManager { get; set; }
        /// <summary>
        /// Start the game loop
        /// </summary>
        public void Start(int tpf, int tpu)
        {
            //Update handler thread
            UpdateThread = new Thread(() =>
            {
                Stopwatch su = Stopwatch.StartNew();
                //TODO: Implement skipping to prevent the spiral of death
                while (true)
                {
                    if (Update != null)
                    {
                        lock (Update)
                        {
                            //Update all input data
                            Keyboard.Update();
                            Mouse.Update();

                            //Call update handler
                            Update((tpu == 0) ? GetNormTicks(su) : tpu, this);
                        }
                    }

                    if (tpu != 0 && tpu > GetNormTicks(su))
                    {
                        try
                        {
                            Thread.Sleep(TimeSpan.FromTicks((long)tpu - (long)GetNormTicks(su)));
                        }
                        catch (Exception) { }
                    }
                    Kokoro2.Debug.ObjectAllocTracker.PostUPS(GetNormTicks(su));
                    su.Reset();
                    su.Start();
                }
            });

            ResourceManagerThread = new Thread(() =>
            {
                GameLooper(160000, (a, b) =>
                {

                });
            });

            #region LL executor
            Stopwatch s = new Stopwatch();
            bool tmpCtrl = false;
            ViewportControl.Paint += (a, b) =>
            {
                //TODO setup command buffer system
                if (inited)
                {
                    if (!tmpCtrl)
                    {
                        Initialize(this);
                        //Push the accumulated command buffers for this thread right now
                        tmpCtrl = true;
                    }

                    if (!s.IsRunning) s.Start();

                    if (ResourceManager != null) ResourceManager(this);
                    ResourceManager = null;

                    Window_RenderFrame(0);
                    Render(tpf, this);
                }

                Kokoro2.Debug.ObjectAllocTracker.PostFPS(GetNormTicks(s));
                ViewportControl.Invalidate();
                s.Reset();
                s.Start();
            };
            #endregion

            var tmp = Initialize;
            Initialize = (GraphicsContext c) =>
            {
                Debug.ErrorLogger.StartLogger(true);
                Debug.ErrorLogger.AddMessage(0, "Engine Started", Debug.DebugType.Marker, Debug.Severity.Notification);

                ZNear = 0.0001f;
                ZFar = 1000000f;
                DepthWrite = true;
                Viewport = new Vector4(0, 0, WindowSize.X, WindowSize.Y);

            };
            Initialize += tmp;
            Initialize += (GraphicsContext c) =>
            {

                //Spawn threads for each: Update, Physics, Animation, Render
                UpdateThread.Start();
                //TODO ResourceManagerThread.Start();
            };
        }

        /// <summary>
        /// Swap the backbuffer and frontbuffer
        /// </summary>
        public void SwapBuffers()
        {
            swapBuffers();
        }

        private void GameLooper(double timestep, Action<double, GraphicsContext> handler)
        {
            Stopwatch s = Stopwatch.StartNew();
            //TODO: Implement skipping to avoid the spiral of death
            while (true)
            {
                if (handler != null)
                {
                    lock (handler)
                    {
                        handler((timestep == 0) ? GetNormTicks(s) : timestep, this);
                    }
                    //Each thread is required to push its commandbuffer when done
                }

                if (timestep != 0 && timestep > GetNormTicks(s))
                {
                    try
                    {
                        Thread.Sleep(TimeSpan.FromTicks((long)timestep - (long)GetNormTicks(s)));
                    }
                    catch (InvalidOperationException) { }
                    catch (ArgumentOutOfRangeException) { }
                }
                s.Reset();
                s.Start();
            }
        }

        private double GetNormTicks(Stopwatch s)
        {
            return (double)(s.ElapsedTicks * 1000 * 10000) / (Stopwatch.Frequency);
        }

        #endregion

        /// <summary>
        /// Create a new GraphicsContext
        /// </summary>
        /// <param name="WindowSize">The size of the Window</param>
        public GraphicsContext(Vector2 WindowSize)
            : base((int)WindowSize.X, (int)WindowSize.Y)
        {
            Debug.DebuggerManager.ShowDebugger();
            Debug.ObjectAllocTracker.NewCreated(this, 0, "GraphicsContext Created");
        }


    }
}
