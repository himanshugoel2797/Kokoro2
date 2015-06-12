using Kokoro2.Math;

namespace Kokoro2.Engine.HighLevel.Voxel
{
    public struct OctreeDrawInfo
    {
        public int index;
        public int corners;
        public Vector3 position;
        public Vector3 averageNormal;
        internal QefData qef;
    }
}