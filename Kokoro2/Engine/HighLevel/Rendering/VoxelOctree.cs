using Kokoro2.Engine.HighLevel.Lights;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Rendering
{
    public class VoxelData
    {
        public Vector3 Color;
        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 LightingColors;
    }

    public class VoxelTree
    {
        public enum Axis
        {
            X = 0, Y = 1, Z = 2
        }

        public VoxelTree Left, Right;
        public Axis EntryAxis;
        public Dictionary<Vector3, VoxelData> Voxels;
        public VoxelData[] VoxelValues;
        public float Center;
        public float Side;

        private bool? prevOption = null;    //False = left, true = Right

        public VoxelTree() : this(Axis.X) { }

        private VoxelTree(Axis Axis)
        {
            EntryAxis = Axis;
        }

        private static Axis FindLargestAxis(Vector3 sz, out float center)
        {
            Axis a;
            float maxAxis = System.Math.Max(System.Math.Max(sz.X, sz.Y), sz.Z);
            if (maxAxis == sz.X) a = Axis.X;
            else if (maxAxis == sz.Y) a = Axis.Y;
            else a = Axis.Z;
            center = maxAxis * 0.5f;
            return a;
        }

        internal void OptimizeTree()
        {

            if (Voxels != null && Voxels.Count > 50)
            {
                //Loop over all the voxels and attempt to determine which group forms the largest axis, subdivide along that axis
                float toLeft = 0, toRight = 0;
                float maxExtent = float.NegativeInfinity, minExtent = float.PositiveInfinity;

                var voxData = Voxels.Values.ToArray();

                for (int i = 1; i < voxData.Length; i++)
                {
                    var tmp = voxData[i];

                    if (tmp.Position[(int)EntryAxis] > maxExtent) maxExtent = tmp.Position[(int)EntryAxis];
                    if (tmp.Position[(int)EntryAxis] < minExtent) minExtent = tmp.Position[(int)EntryAxis];

                    if (tmp.Position[(int)EntryAxis] < Center) toLeft++;
                    else toRight++;
                }

                if (Left == null)
                {
                    Left = new VoxelTree((Axis)(((int)EntryAxis + 1) % 3));
                    Left.Voxels = new Dictionary<Vector3, VoxelData>();
                }
                if (Right == null)
                {
                    Right = new VoxelTree((Axis)(((int)EntryAxis + 1) % 3));
                    Right.Voxels = new Dictionary<Vector3, VoxelData>();
                }
                float leftWeight = toLeft / Voxels.Count;

                float rightWeight = toRight / Voxels.Count;

                if (leftWeight > rightWeight)
                {
                    Center += (leftWeight) * minExtent;
                }
                else
                {
                    Center += (rightWeight) * maxExtent;
                }

                for (int i = 0; i < Voxels.Count; i++)
                {
                    var tmp = Voxels.Values.ElementAt(i);

                    if (tmp.Position[(int)EntryAxis] < Center) Left.Voxels[tmp.Position] = tmp;
                    else Right.Voxels[tmp.Position] = tmp;
                }
                Voxels = null;
            }

            //Walk down the tree and redistribute sets larger than 100 voxels
            Left?.OptimizeTree();
            Right?.OptimizeTree();
        }

        public void AddObject(VoxelData data, Vector3 Center, float side)
        {
            this.Center = Center[(int)EntryAxis];
            this.Side = side;

            if (data.Position[(int)EntryAxis] < this.Center)
            {
                if (Left == null)
                {
                    Left = new VoxelTree((Axis)(((int)EntryAxis + 1) % 3));
                    Left.Voxels = new Dictionary<Vector3, VoxelData>();
                }
                Left.Voxels[data.Position] = data;
            }
            else
            {
                if (Right == null)
                {
                    Right = new VoxelTree((Axis)(((int)EntryAxis + 1) % 3));
                    Right.Voxels = new Dictionary<Vector3, VoxelData>();
                }
                Right.Voxels[data.Position] = data;
            }
        }


        public bool RayCast(Vector3 p, Vector3 d, float vSide, out VoxelData data)
        {
            data = null;
            if (Voxels != null)
            {
                float intersectionDist = float.PositiveInfinity;
                int index = -1;

                if (VoxelValues == null) VoxelValues = Voxels.Values.ToArray();
                var voxData = VoxelValues;

                BoundingBox b2 = new BoundingBox();

                for (int i = 0; i < voxData.Length; i++)
                {
                    b2.Min = voxData[i].Position - Vector3.One * vSide * 0.5f;
                    b2.Max = voxData[i].Position + Vector3.One * vSide * 0.5f;
                    if (GI_IntersectionTests.intersect(b2, p, d))
                    {
                        float dist = (voxData[i].Position - p).LengthSquared;
                        if (dist < intersectionDist)
                        {
                            intersectionDist = dist;
                            index = i;
                            data = voxData[i];
                        }
                    }
                }

                return index != -1;
            }

            if (prevOption == null)
            {
                //Traverse the ray to the center plane
                if (p[(int)EntryAxis] < Center)
                {
                    bool res = false;
                    if (Left != null) res = Left.RayCast(p, d, vSide, out data);
                    else return false;

                    if (res)
                    {
                        prevOption = false;
                    }
                    return res;
                }
                else
                {
                    bool res = false;
                    if (Right != null) res = Right.RayCast(p, d, vSide, out data);
                    else return false;

                    if (res)
                    {
                        prevOption = true;
                    }
                    return res;
                }
            }
            else
            {
                bool res = false;
                if (prevOption == true) res = Right.RayCast(p, d, vSide, out data);
                else if (prevOption == false) res = Left.RayCast(p, d, vSide, out data);

                if (res == false)
                {
                    prevOption = null;
                    return RayCast(p, d, vSide, out data);
                }
                return res;
            }
        }

    }
}
