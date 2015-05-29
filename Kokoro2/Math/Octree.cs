using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Math
{
    /// <summary>
    /// Implement an Octree data structure
    /// </summary>
    /// <typeparam name="T">The type of the information the Octree stores</typeparam>
    public class Octree<T>
    {
        /// <summary>
        /// The information for this Octree cell
        /// </summary>
        public T Information;

        /// <summary>
        /// TopForwardLeft Cell
        /// </summary>
        public Octree<T> TopForwardLeft;
        /// <summary>
        /// TopForwardRight Cell
        /// </summary>
        public Octree<T> TopForwardRight;
        /// <summary>
        /// TopBackLeft Cell
        /// </summary>
        public Octree<T> TopBackLeft;
        /// <summary>
        /// TopBackRight Cell
        /// </summary>
        public Octree<T> TopBackRight;

        /// <summary>
        /// BottomForwardLeft Cell
        /// </summary>
        public Octree<T> BottomForwardLeft;
        /// <summary>
        /// BottomForwardRight Cell
        /// </summary>
        public Octree<T> BottomForwardRight;
        /// <summary>
        /// BottomBackLeft Cell
        /// </summary>
        public Octree<T> BottomBackLeft;
        /// <summary>
        /// BottomBackRight Cell
        /// </summary>
        public Octree<T> BottomBackRight;

        /// <summary>
        /// The Parent of this octree node
        /// </summary>
        public Octree<T> Parent;
    }
}
