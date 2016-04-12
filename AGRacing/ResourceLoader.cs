using Kokoro2.Engine;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AGRacing
{
    public struct TrackData
    {
        public string Name;
        public string modelFile;
        public string collisionMesh;
        public string textureFile;
        public string path;
        public bool Reverse;
    }

    public struct CraftData
    {
        public string Name;
        public string modelFile;
        public string textureFile;
        public float Scale;
        public float Mass;
        public Vector3 frontDirection;
        public Vector3 rotation;
        public Vector3[] particleEmitterLocations;
        public string[] particleEmitterTextures;
    }

    /// <summary>
    /// Parse the resource data and add it all to a database
    /// </summary>
    static class ResourceLoader
    {
        public static bool LoadComplete = false;

        public static Dictionary<string, TrackData> TrackData;
        public static Dictionary<string, CraftData> ShipData;

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

                TrackData = new Dictionary<string, TrackData>();
                string[] lns = File.ReadAllLines("Resources/Proc/Data/Tracks.txt");
                TrackNames = new string[lns.Length];

                for (int i = 0; i < lns.Length; i++)
                {
                    var tmp = lns[i].Split(',');
                    if (tmp.Length < 2)
                        throw new Exception("Tracks.txt is corrupt!");

                    tmp[1] = "Resources/Proc/Data/" + tmp[1];

                    XmlSerializer s = new XmlSerializer(typeof(TrackData));

                    TrackData.Add(tmp[0], (TrackData)s.Deserialize(File.OpenRead(tmp[1])));
                    TrackNames[i] = tmp[0];
                }

                ShipData = new Dictionary<string, CraftData>();
                lns = File.ReadAllLines("Resources/Proc/Data/Ships.txt");
                ShipNames = new string[lns.Length];

                for (int i = 0; i < lns.Length; i++)
                {
                    var tmp = lns[i].Split(',');
                    if (tmp.Length < 2)
                        throw new Exception("Ships.txt is corrupt!");

                    tmp[1] = "Resources/Proc/Data/" + tmp[1];

                    XmlSerializer s = new XmlSerializer(typeof(CraftData));

                    ShipData.Add(tmp[0], (CraftData)s.Deserialize(File.OpenRead(tmp[1])));
                    ShipNames[i] = tmp[0];
                }

                LoadComplete = true;
            });
            t.Start();
        }

        public static Track LoadTrack(string name, GraphicsContext c)
        {
            if (!TrackData.ContainsKey(name)) throw new ArgumentException();
            return new Track(TrackData[name], c);
        }

        public static Ship LoadShip(string name, IShipController controller, GraphicsContext c)
        {
            if (!ShipData.ContainsKey(name)) throw new ArgumentException();
            return new Ship(ShipData[name], controller, c);
        }
    }
}
