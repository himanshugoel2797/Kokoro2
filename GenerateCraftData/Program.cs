using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GenerateCraftData
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

    class Program
    {


        static void Main(string[] args)
        {
            Console.WriteLine("Track(0) or Car(1)?");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 0:
                    {
                        TrackData t = new TrackData();
                        Console.WriteLine("Enter Track Name:");
                        t.Name = Console.ReadLine();
                        Console.WriteLine("Enter Track Model path:");
                        t.modelFile = Console.ReadLine();
                        Console.WriteLine("Enter Track Collision path:");
                        t.collisionMesh = Console.ReadLine();
                        Console.WriteLine("Enter Track Texture path:");
                        t.textureFile = Console.ReadLine();
                        Console.WriteLine("Enter Track Path path:");
                        t.path = Console.ReadLine();
                        Console.WriteLine("Enter Track Reverse:");
                        t.Reverse = bool.Parse(Console.ReadLine());

                        XmlSerializer s = new XmlSerializer(typeof(TrackData));
                        using (Stream strm = File.OpenWrite(t.Name + ".xml")) s.Serialize(strm, t);
                    }
                    break;
                case 1:
                    {
                        CraftData t = new CraftData();
                        Console.WriteLine("Enter Craft Name:");
                        t.Name = Console.ReadLine();
                        Console.WriteLine("Enter Craft Model path:");
                        t.modelFile = Console.ReadLine();
                        Console.WriteLine("Enter Craft Scale:");
                        t.Scale = float.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Craft Texture path:");
                        t.textureFile = Console.ReadLine();
                        Console.WriteLine("Enter Craft Rotation X:");
                        float x = float.Parse(Console.ReadLine());

                        Console.WriteLine("Enter Craft Rotation Y:");
                        float y = float.Parse(Console.ReadLine());

                        Console.WriteLine("Enter Craft Rotation Z:");
                        float z = float.Parse(Console.ReadLine());
                        t.rotation = new Vector3(x, y, z);

                        Console.WriteLine($"Enter Front direction X:");
                        x = float.Parse(Console.ReadLine());
                        Console.WriteLine($"Enter Front direction Y:");
                        y = float.Parse(Console.ReadLine());
                        Console.WriteLine($"Enter Front direction Z:");
                        z = float.Parse(Console.ReadLine());
                        t.frontDirection = new Vector3(x, y, z);

                        Console.WriteLine("Enter Craft Mass:");
                        t.Mass = float.Parse(Console.ReadLine());

                        Console.WriteLine("Enter Emitter Count:");
                        int emitterCount = int.Parse(Console.ReadLine());


                        t.particleEmitterLocations = new Vector3[emitterCount];
                        t.particleEmitterTextures = new string[emitterCount];

                        for (int i = 0; i < emitterCount; i++)
                        {
                            Console.WriteLine($"Enter Emitter #${i} Location X:");
                            x = float.Parse(Console.ReadLine());
                            Console.WriteLine($"Enter Emitter #${i} Location Y:");
                            y = float.Parse(Console.ReadLine());
                            Console.WriteLine($"Enter Emitter #${i} Location Z:");
                            z = float.Parse(Console.ReadLine());
                            t.particleEmitterLocations[i] = new Vector3(x, y, z);

                            Console.WriteLine($"Enter Emitter #${i} Texture path:");
                            t.particleEmitterTextures[i] = Console.ReadLine();
                        }

                        XmlSerializer s = new XmlSerializer(typeof(CraftData));
                        using (Stream strm = File.OpenWrite(t.Name + ".xml")) s.Serialize(strm, t);
                    }
                    break;
            }
        }
    }
}
