using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AGRacing
{
    /// <summary>
    /// Parse the resource data and add it all to a database
    /// </summary>
    static class ResourceLoader
    {
        public static bool LoadComplete = false;

        public static Dictionary<string, string> TrackData;
        public static Dictionary<string, string> ShipData;

        public static string[] TrackNames
        {
            get; private set;
        }

        public static string[] ShipNames
        {
            get; private set;
        }

        static ResourceLoader()
        {
            Thread t = new Thread(() =>
            {
                LoadComplete = false;

                TrackData = new Dictionary<string, string>();
                string[] lns = File.ReadAllLines("Resources/Proc/Data/Tracks.txt");
                TrackNames = new string[lns.Length];

                for(int i = 0; i < lns.Length; i++)
                {
                    var tmp = lns[i].Split(',');
                    if (tmp.Length != 3)
                        throw new Exception("Tracks.txt is corrupt!");

                    TrackData.Add(tmp[1], lns[i]);
                    TrackNames[i] = tmp[1];
                }

                ShipData = new Dictionary<string, string>();
                lns = File.ReadAllLines("Resources/Proc/Data/Ships.txt");
                ShipNames = new string[lns.Length];

                for(int i = 0; i < lns.Length; i++)
                {
                    var tmp = lns[i].Split(',');
                    if (tmp.Length < 3)
                        throw new Exception("Ships.txt is corrupt!");

                    ShipData.Add(tmp[1], lns[i]);
                    ShipNames[i] = tmp[1];
                }

                LoadComplete = true;
            });
            t.Start();
        }

        public static Track LoadTrack(string name)
        {
            if (!TrackData.ContainsKey(name)) throw new ArgumentException();
            return new Track(TrackData[name]);
        }

        public static Ship LoadShip(string name, IShipController controller)
        {
            if (!ShipData.ContainsKey(name)) throw new ArgumentException();
            return new Ship(ShipData[name], controller);
        }
    }
}
