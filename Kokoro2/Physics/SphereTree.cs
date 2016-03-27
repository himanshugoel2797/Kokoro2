using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Physics
{
    class SphereOctreeNode
    {
        public SphereOctreeNode[] Children = new SphereOctreeNode[8];
        public int curDepth;
        public bool hasSurface;
        public float side;


        public static int GetIndex(int x, int y, int z)
        {
            return ((x >= 0) ? (1 << 2) : 0) | ((y >= 0) ? (1 << 1) : 0) | ((z >= 0) ? 1 : 0);
        }

        public void Add(Vector3 point, int maxDepth)
        {
            Add(point, Vector3.Zero, maxDepth, curDepth);
        }

        private void Add(Vector3 point, Vector3 nC, int maxDepth, int curDepth)
        {
            Vector3 nP = point;
            Vector3 C = nC + new Vector3(point.X >= 0 ? 1 : -1, point.Y >= 0 ? 1 : -1, point.Z >= 0 ? 1 : -1) * side / 2f;
            nP = point - C;

            int index = GetIndex((int)point.X, (int)point.Y, (int)point.Z);

            if (++curDepth < maxDepth)
            {
                if(Children[index] == null)
                {
                    Children[index] = new SphereOctreeNode();
                    Children[index].curDepth = curDepth + 1;
                    Children[index].side = side / 2f;
                }

                Children[GetIndex((int)point.X, (int)point.Y, (int)point.Z)].Add(nP, C, maxDepth, curDepth);
            }
            else hasSurface = true;
        }
    }

    public class SphereOctree
    {
        SphereOctreeNode Root;
        int maxDepth;
        
        public SphereOctree(float side, int maxDepth = 5)
        {
            Root = new SphereOctreeNode();
            Root.curDepth = 0;
            this.maxDepth = maxDepth;
        }

        public void Add(Vector3 point)
        {
            Root.Add(point, maxDepth);
        }
    }

    class SphereTree
    {
        SphereOctree tree;

        public SphereTree(PhysicsBody c)
        {
            tree = c.Voxelize();
        }
    }
}
