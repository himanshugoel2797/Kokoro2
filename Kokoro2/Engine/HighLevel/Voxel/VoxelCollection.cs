using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Voxel
{
    public class VoxelCollection
    {
        public Texture Atlas;
        public Dictionary<double, VoxelProperties> VoxelIDs;

        public VoxelCollection(Texture TexAtlas)
        {
            VoxelIDs = new Dictionary<double, VoxelProperties>();
            VoxelIDs[0] = new VoxelProperties()
            {
                AtlasDimensions = new Vector4(0, 0, 1, 1),
                Visible = false
            };      //By default, 0 density = empty

            Atlas = TexAtlas;
        }

        public VoxelProperties this[double id]
        {
            get
            {
                if (VoxelIDs.ContainsKey(id))
                {
                    return VoxelIDs[id];
                }
                else
                {
                    double? val = FindClosest(VoxelIDs.Keys.AsEnumerable(), id);
                    if (val != null) return VoxelIDs[(double)val];
                    else return VoxelIDs[0];    //NOTE this might be an issue later
                }
            }
            set
            {
                VoxelIDs[id] = value;
            }
        }

        private double? FindClosest(IEnumerable<double> numbers, double x)
        {
            return
                (from number in numbers
                 let difference = System.Math.Abs(number - x)
                 orderby difference, System.Math.Abs(number), number descending
                 select (double?)number)
                .FirstOrDefault();
        }

    }

    public struct VoxelProperties
    {
        public bool Visible;
        public Vector4 AtlasDimensions;
        public string Name;
        public float Mass;

        //TODO figure out which side maps to which edge
        public float SideLengthA;
        public float SideLengthB;
        public float SideLengthC;
        public float SideLengthD;
        public float SideLengthE;
        public float SideLengthF;
        public float SideLengthG;
        public float SideLengthH;
        public float SideLengthI;
        public float SideLengthJ;
        public float SideLengthK;
        public float SideLengthL;

    }
}
