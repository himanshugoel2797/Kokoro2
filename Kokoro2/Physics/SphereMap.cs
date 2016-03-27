using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Physics
{
    class SphereMap
    {
        Dictionary<int, List<long>> X;
        Dictionary<int, List<long>> Y;
        Dictionary<int, List<long>> Z;

        public SphereMap()
        {
            X = new Dictionary<int, List<long>>();
            Y = new Dictionary<int, List<long>>();
            Z = new Dictionary<int, List<long>>();
        }

        public List<long> this[Vector3 a]
        {
            get
            {
                double x = a.X - System.Math.Floor(a.X);
                double y = a.Y - System.Math.Floor(a.Y);
                double z = a.Z - System.Math.Floor(a.Z);

                //Now retrieve the sets
                if (!X.ContainsKey((int)a.X) | !Y.ContainsKey((int)a.Y) | !Z.ContainsKey((int)a.Z))
                    return null;

                var x_set = X[(int)a.X];
                var y_set = Y[(int)a.Y];
                var z_set = Z[(int)a.Z];

                if (x_set.Count == 0 | y_set.Count == 0 | z_set.Count == 0)
                    return null;
                return x_set.Intersect(y_set).Intersect(z_set).ToList();
            }
        }

        public List<long> this[Vector3 a, float rad]
        {
            get
            {
                double x = System.Math.Ceiling(a.X - System.Math.Floor(a.X) + rad);
                double y = System.Math.Ceiling(a.Y - System.Math.Floor(a.Y) + rad);
                double z = System.Math.Ceiling(a.Z - System.Math.Floor(a.Z) + rad);

                List<long> toRet = new List<long>();

                for (int x0 = (int)-x; x0 <= x; x0++)
                    for (int y0 = (int)-y; y0 <= y; y0++)
                        for (int z0 = (int)-z; z0 <= z; z0++)
                        {
                            toRet.AddRange(this[new Vector3(x0, y0, z0)]);
                        }

                return toRet;
            }
        }

        public void AddEntry(Vector3 loc, long id)
        {
            loc.Round();
            if (X[(int)loc.X] == null) X[(int)loc.X] = new List<long>();
            X[(int)loc.X].Add(id);

            if (Y[(int)loc.Y] == null) Y[(int)loc.Y] = new List<long>();
            Y[(int)loc.Y].Add(id);

            if (Z[(int)loc.Z] == null) Z[(int)loc.Z] = new List<long>();
            Z[(int)loc.Z].Add(id);
        }

        public void FinalizeData()
        {
            for (int i = 0; i < X.Count; i++)
            {
                X[X.Keys.ElementAt(i)].Sort();
            }

            for (int i = 0; i < Y.Count; i++)
            {
                Y[Y.Keys.ElementAt(i)].Sort();
            }

            for (int i = 0; i < Z.Count; i++)
            {
                Z[Z.Keys.ElementAt(i)].Sort();
            }
        }
    }
}
