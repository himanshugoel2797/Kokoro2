using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.SceneGraph
{
    /// <summary>
    /// Manages Scene and SceneManager objects
    /// </summary>
    public class SceneManager : IScene, IEngineObject
    {
        private Dictionary<string, IScene> scenes;
        private IScene curScene;

        public Dictionary<string, IScene> Scenes
        {
            get
            {
                return scenes;
            }
            set
            {
                scenes = value;
            }
        }

        /// <summary>
        /// Create a new instance of a SceneManager
        /// </summary>
        public SceneManager(GraphicsContext c)
        {
            ParentContext = c;
            ID = c.EngineObjects.RegisterObject(-1);
            c.Disposing += Dispose;

            ObjectAllocTracker.NewCreated(this);

            scenes = new Dictionary<string, IScene>();
        }

        /// <summary>
        /// Subscribe this scene maanager to the game loop
        /// </summary>
        /// <param name="context">The current GraphicsContext</param>
        public void Register(GraphicsContext context)
        {
            context.Render += this.Render;
            context.Update += this.Update;
            context.ResourceLoader += this.LoadResources;
        }

        /// <summary>
        /// Add a Scene object for this SceneManager to manage
        /// </summary>
        /// <param name="key">The identifier for the scene object to add</param>
        /// <param name="scene">The scene object to add</param>
        public void Add(string key, IScene scene)
        {
            scene.Parent = this;
            scenes.Add(key, scene);
        }

        /// <summary>
        /// Remove a scene object this SceneManager is managing
        /// </summary>
        /// <param name="key">The identifier for the scene object to remove</param>
        public void Remove(string key)
        {
            scenes[key].Parent = null;
            scenes.Remove(key);
        }

        /// <summary>
        /// Set the currently active scene
        /// </summary>
        /// <param name="scene">The identifier for the scene to make active</param>
        public void Activate(string scene)
        {
            curScene = scenes[scene];
        }

        /// <summary>
        /// Update the scene
        /// </summary>
        /// <param name="interval">The time in ticks since the last update</param>
        /// <param name="context">The current GraphicsContext</param>
        public void Update(double interval, GraphicsContext context)
        {
            if (curScene != null) curScene.Update(interval, context);
        }

        /// <summary>
        /// Render the scene
        /// </summary>
        /// <param name="interval">The time in ticks since the last render</param>
        /// <param name="context">The current GraphicsContext</param>
        public void Render(double interval, GraphicsContext context)
        {
            if (curScene != null) curScene.Render(interval, context);
        }

        public void LoadResources(GraphicsContext context)
        {
            if (curScene != null) curScene.LoadResources(context);
        }

        public IScene Parent
        {
            get;
            set;
        }

        public ulong ID
        {
            get;
            set;
        }

        public GraphicsContext ParentContext
        {
            get;
            set;
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
                }

                ParentContext.EngineObjects.UnregisterObject(ID);
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~SceneManager()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

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
