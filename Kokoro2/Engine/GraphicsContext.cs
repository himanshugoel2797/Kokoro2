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
    public class GraphicsContext : GraphicsContextLL, IEngineObject
    {

        #region State Machine Properties

        public bool IsFocused
        {
            get { return base.aGetFocus(); }
            set { base.aSetFocus(value); }
        }

        public Action<GraphicsContext> FocusPollHandler { get; set; }

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

        /// <summary>
        /// Enable/Disable Transform Feedback
        /// </summary>
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
        /// Manage all the objects created by this control
        /// </summary>
        public EngineObjectManager EngineObjects { get; set; }

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
        /// Enable/Disable depth clamping
        /// </summary>
        public bool DepthClamp
        {
            get
            {
                return base.GetDepthClamp();
            }
            set
            {
                base.SetDepthClamp(value);
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
        public Matrix4 Projection
        {
            get
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
        public void ClearColor(float r, float g, float b, float a)
        {
            base.aClear(r, g, b, a);
        }
        /// <summary>
        /// Clear the screen
        /// </summary>
        /// <param name="col">The color to clear the screen with (all 0 to 1)</param>
        public void ClearColor(Vector4 col) { ClearColor(col.X, col.Y, col.Z, col.W); }

        public void ClearDepth() { base.aClearDepth(); }
        #endregion

        #region Draw
        public void Draw(RenderInfo r, GeometryInfo g)
        {
            var CurrentShader = r.Shader;
            CurrentShader["ZNear"] = ZNear;
            CurrentShader["ZFar"] = ZFar;
            CurrentShader["EyePos"] = Camera.Position;
            CurrentShader["EyeDir"] = Camera.Direction;
            CurrentShader["Fcoef"] = 2.0f / (float)System.Math.Log(ZFar + 1.0, 2);

            if (r.Material.AlbedoMap != null) CurrentShader["AlbedoMap"] = r.Material.AlbedoMap;
            if (r.Material.GlossinessMap != null) CurrentShader["GlossinessMap"] = r.Material.GlossinessMap;
            if (r.Material.SpecularMap != null) CurrentShader["SpecularMap"] = r.Material.SpecularMap;
            else if (r.Material.AlbedoMap != null) CurrentShader["SpecularMap"] = r.Material.AlbedoMap;
            CurrentShader["World"] = r.World;
            CurrentShader["View"] = View;
            CurrentShader["Projection"] = Projection;
            CurrentShader.Apply(this);

            //Bind the geometry
            g.Buffer.Bind();
            GraphicsContextLL.Draw(r.DrawMode, 0, (uint)g.PrimitiveCount, 0);
            g.Buffer.UnBind();


            CurrentShader.Cleanup(this);
        }

        public void Draw(Model m)
        {
            Draw(m.RenderInfo, m.GeometryInfo);
        }

        //TODO implement overloads with multiple RenderInfo objects and one GeometryInfo object and vice versa
        #endregion

        #region Game Loop
        /// <summary>
        /// The thread running the Update Loop
        /// </summary>
        public Thread UpdateThread { get; private set; }
        /// <summary>
        /// The thread on which the engine resource manager runs
        /// </summary>
        public Thread ResourceManagerThread { get; private set; }

        /// <summary>
        /// The thread running the Render Loop
        /// </summary>
        public Thread RenderThread { get; private set; }

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
        /// The Resource Manager handler - Use for startup resource loading
        /// </summary>
        public Action<GraphicsContext> ResourceLoader { get; set; }

        /// <summary>
        /// The Resize event handler - Use to resize anything that depends on the window information
        /// </summary>
        public Action<GraphicsContext> WindowResized { get; set; }

        /// <summary>
        /// The Disposing event handler - Use to cleanly free any objects or save any data when this context is destroyed
        /// </summary>
        public Action Disposing { get; set; }

        public ulong ID
        {
            get;
            set;
        }

        public GraphicsContext ParentContext
        {
            get; set;
        }

        private bool stopGame = false, pause = false;

        /// <summary>
        /// Pause engine execution
        /// </summary>
        public void Pause()
        {
            pause = true;
        }

        /// <summary>
        /// Resume engine execution
        /// </summary>
        public void Resume()
        {
            pause = false;
        }

        /// <summary>
        /// Stop engine execution and exit
        /// </summary>
        public void Stop()
        {
            stopGame = true;
        }

        /// <summary>
        /// Start the game loop
        /// </summary>
        public void Start(int tpf, int tpu)
        {

            #region LL executor
            //Update handler thread
            UpdateThread = new Thread(() =>
            {
                GameLooper(tpu, (a, b) =>
                {
                    Keyboard.Update();
                    Mouse.Update();

                    Update?.Invoke(a, b);
                });
            });

            bool tmpCtrl = false, prevFrameDone = true;
            RenderThread = new Thread(() =>
            {
                GameLooper(tpf, (a, b) =>
                {
                    while (!prevFrameDone)
                    {
                        Thread.Sleep(1);
                        if (stopGame) return;
                    }
                    prevFrameDone = false;
                    ViewportControl.BeginInvoke(new MethodInvoker(() =>
                    {
                        prevFrameDone = false;
                        if (inited)
                        {
                            if (!tmpCtrl)
                            {
                                Initialize(this);
                                //Push the accumulated command buffers for this thread right now
                                tmpCtrl = true;
                            }

                            ResourceLoader?.Invoke(this);
                            ResourceLoader = null;

                            FocusPollHandler?.Invoke(this);

                            Window_RenderFrame(0);
                            Render(a, b);
                        }
                        prevFrameDone = true;

                    }));
                });
            });
            #endregion


            var tmp = Initialize;
            Initialize = (GraphicsContext c) =>
                        {
                            ErrorLogger.StartLogger();

                            ZNear = 0.0001f;
                            ZFar = 1000000f;
                            DepthWrite = true;
                            Viewport = new Vector4(0, 0, WindowSize.X, WindowSize.Y);
                            ObjectAllocTracker.NewCreated(this);

                        };
            Initialize += tmp;
            Initialize += (GraphicsContext c) =>
            {
                //Spawn threads for each: Update, Physics, Animation, Render
                UpdateThread.Start();
            };

            RenderThread.Start();
        }

        /// <summary>
        /// Swap the backbuffer and frontbuffer
        /// </summary>
        public void SwapBuffers()
        {
            swapBuffers();
        }

        private void GameLooper(int timestep, Action<double, GraphicsContext> handler)
        {
            Stopwatch s = Stopwatch.StartNew();
            //TODO: Implement skipping to avoid the spiral of death
            while (!stopGame)
            {
                while (pause) Thread.Sleep(timestep);
                if (handler != null)
                {
                    lock (handler)
                    {
                        handler((timestep == 0) ? s.ElapsedMilliseconds : timestep, this);
                    }
                    //Each thread is required to push its commandbuffer when done
                }

                if (timestep != 0 && timestep > s.ElapsedMilliseconds)
                {
                    try
                    {
                        Thread.Sleep(timestep - (int)s.ElapsedMilliseconds);
                    }
                    catch (InvalidOperationException) { }
                    catch (ArgumentOutOfRangeException) { }
                }
                s.Reset();
                s.Start();
            }
        }

        #endregion

        /// <summary>
        /// Create a new GraphicsContext
        /// </summary>
        /// <param name="WindowSize">The size of the Window</param>
        public GraphicsContext(Vector2 WindowSize)
                    : base((int)WindowSize.X, (int)WindowSize.Y)
        {
            EngineObjects = new EngineObjectManager();
            ParentContext = this;
            ID = EngineObjectManager.RegisterContext();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Disposing?.Invoke();
                    Window.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GraphicsContext() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
