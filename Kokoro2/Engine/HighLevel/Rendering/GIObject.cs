using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Rendering
{
    public class GIObject
    {
        public VoxelTree Voxels;
        public float VoxelSide;
        public BoundingBox box;
        public float Scale;
        public Vector3 Position;
        public Quaternion Orientation;



        internal GIObject(VoxelTree v, float VoxelSide, BoundingBox b)
        {
            Voxels = v;
            this.VoxelSide = VoxelSide;
            this.box = b;
            Scale = 1;
            Position = Vector3.Zero;
            Orientation = Quaternion.Identity;
        }
    }
}
