using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Math;
using LibNoise;

namespace Kokoro2.Engine.HighLevel.Voxel
{
    public struct Voxel
    {
        public double VoxelType;
        public bool Active;
    }

    public class VoxelChunk
    {
        public Voxel[,,] Info;
        public int Side;

        internal List<float> Vertices, Normals, UVs;
        internal List<uint> Indices;
        internal bool Dirty;

        //Seed = -1 is used to skip automatic generation of data and instead to initialize to a full block
        public VoxelChunk(int side, int seed, Vector3 prevPos)
        {
            this.Side = side;
            Info = new Voxel[Side, Side, Side];

            Perlin p = new Perlin();

            for (int x = 0; x < side; x++)
                for (int y = 0; y < side; y++)
                    for (int z = 0; z < side; z++)
                    {
                        Info[x, y, z] = new Voxel()
                        {
                            Active = true
                        };
                        if (seed != -1)
                        {
                            //TODO finish chunk generation
                            double density = p.GetValue((prevPos.X + x) / (float)side, (prevPos.Y + y) / (float)side, (prevPos.Z + z) / (float)side);
                            Info[x, y, z].VoxelType = density;
                        }
                    }

            Dirty = true;
        }

        public bool IsSurface(int x, int y, int z)
        {
            if (!Info[x, y, z].Active) return false;    //This voxel isn't visible if it isn't active
            else if (x == 0 | y == 0 | z == 0 | x == (Side - 1) | y == (Side - 1) | z == (Side - 1)) return true;      //If anything is on the array boundary, it is definitely a surface if it is enabled
            else if
                ((!Info[(x - 1), y, z].Active | !Info[(x + 1), y, z].Active | !Info[x, (y - 1), z].Active | !Info[x, (y + 1), z].Active | !Info[x, y, (z + 1)].Active | !Info[x, y, (z - 1)].Active))
                return false;
            else return true;
            //perform more thorough checks, if there is no block on any face, then this is a surface voxel
        }
    }

    public class VoxelOctree
    {
        Octree<VoxelChunk> Data;
        public float Side;

        public VoxelOctree(float worldSide, int seed = -1)
        {
            Data = new Octree<VoxelChunk>(worldSide, 1);
            Side = worldSide;
        }
    }
}
