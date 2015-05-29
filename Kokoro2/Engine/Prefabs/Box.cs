using Kokoro2.Engine.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.Prefabs
{
    public class Box : Model
    {
        /// <summary>
        /// Creates a new FullScreenQuad object
        /// </summary>
        public Box(float width, float height, float depth)
            : base()
        {
            Init(1);

            this.DrawMode = DrawMode.Triangles;

            width /= 2;
            height /= 2;
            depth /= 2;

            SetIndices(UpdateMode.Static, new uint[] { 
                0, 1, 2, 3,
                0, 4, 5, 0,
                6, 3, 6, 0,
                0, 2, 4, 5,
                1, 0, 2, 1, 
                5, 7, 6, 3, 
                6, 7, 5, 7, 
                3, 4, 7, 4, 
                2, 7, 2, 5 }, 0);

            SetUVs(UpdateMode.Static, new float[] {
                0,1,
                1,1,
                1,0,
                0,0,
                0,1,
                1,1,
                1,0,
                0,0,
            }, 0);

            SetVertices(UpdateMode.Static, new float[]{
                -width, -height, -depth,    //0
                -width, -height, depth,     //1
                -width, height, depth,      //2
                width, height, -depth,      //3
                -width, height, -depth,     //4
                width, -height, depth,      //5
                width, -height, -depth,     //6
                width, height, depth        //7
            }, 0);

            Bound = new BoundingVolume()
            {
                Max = new Math.Vector3(width, height, depth),
                Min = new Math.Vector3(-width, -height, -depth)
            };

            World = Math.Matrix4.Identity;
        }
    }
}
