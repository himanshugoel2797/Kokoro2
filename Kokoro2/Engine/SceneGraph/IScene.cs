using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.SceneGraph
{
    /// <summary>
    /// Represents a Scene
    /// </summary>
    public interface IScene
    {
        /// <summary>
        /// The parent scene
        /// </summary>
        IScene Parent
        {
            get;
            set;
        }

        /// <summary>
        /// The render handler
        /// </summary>
        /// <param name="interval">The time in ticks since the last frame</param>
        /// <param name="context">The current GraphicsContext</param>
        void Render(double interval, GraphicsContext context);

        /// <summary>
        /// The update handler
        /// </summary>
        /// <param name="interval">The time in ticks since the last frame</param>
        /// <param name="context">The current GraphicsContext</param>
        void Update(double interval, GraphicsContext context);

        /// <summary>
        /// The Resource loader
        /// </summary>
        /// <param name="context">The current GraphicsContext</param>
        void LoadResources(GraphicsContext context);
    }
}
