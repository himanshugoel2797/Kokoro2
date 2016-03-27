using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Physics
{
    class OctreeNode
    {
        public const int ObjectsPerNode = 50;

        public OctreeNode Parent;
        public int currentSide;
        public Vector3 Center;
        public OctreeNode[] Children = new OctreeNode[8];
        public Dictionary<int, long> objects;

        private int minSide;
        private int objCount;

        public static int GetIndex(int x, int y, int z)
        {
            return ((x >= 0) ? (1 << 2) : 0) | ((y >= 0) ? (1 << 1) : 0) | ((z >= 0) ? 1 : 0);
        }

        public OctreeNode(long p, Vector3 center, int currentSide, int minSide)
        {
            Parent = p;
            Center = center;
            this.minSide = minSide;
            this.currentSide = currentSide;
            objects = new Dictionary<int, long>(ObjectsPerNode);
            objCount = 0;
        }

        public void Add(long a)
        {

        }
    }

    class PhysicsOctree
    {
        OctreeNode Root;
        int topLevelSide = 1 << 10;

        public PhysicsOctree()
        {
            Root = new OctreeNode(null, Vector3.Zero, topLevelSide, 1 << 3);
        }

        public void Add(BroadphaseSphere a)
        {
            Root.Add(a);
        }
    }
}
