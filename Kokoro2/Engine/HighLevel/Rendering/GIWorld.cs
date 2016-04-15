using Kokoro2.Engine.HighLevel.Lights;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Rendering
{


    public class GIWorld
    {
        //TODO When not feeling lazy, switch over to a proper binary space partitioning structure, this won't scale well
        private List<GIObject> objects;
        private List<DirectionalLight> dLights;
        private List<PointLight> pLights;
        Bitmap bmp;

        public GIWorld()
        {
            objects = new List<GIObject>();
            dLights = new List<DirectionalLight>();
            pLights = new List<PointLight>();
            bmp = new Bitmap(96, 54);
        }

        public void Update(GraphicsContext c)
        {
            //Got to figure out which would be the best way to upload the lighting data to the gpu
            //Ray cast at a low resolution and upload that as a texture each frame seems reasonable

            //Recalculate the scene lighting by raycasting the scene from the camera's PoV
                    Graphics g = Graphics.FromImage(bmp);
                    g.Clear(Color.White);

            Matrix4 iVP = Matrix4.Invert(c.Camera.Projection * c.Camera.View);

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    //Calculate the NDC coordinates and apply an Inverse ProjView transformation to determine the ray direction
                    //Perform ray tracing using this info


                    Vector4 ndc = new Vector4((float)x / bmp.Width * 2 - 1.0f, -(float)y / bmp.Height * 2 + 1.0f, -1, 1);
                    //Vector4 worldPosRay = Vector4.Transform(ndc, iVP);
                    //Vector3 worldPosRayDir = worldPosRay.Xyz / worldPosRay.W;
                    //Console.WriteLine(worldPosRay);
                    Matrix4 proj = c.Camera.Projection;
                    Matrix4 view = c.Camera.View;

                    Vector4 worldPosRay = Vector4.UnProject(ref proj, view, 96, 54, new Vector2(x, y));
                    Vector3 worldPosRayDir = Vector3.Normalize(worldPosRay.Xyz);
                    
                    for (int i = 0; i < objects.Count; i++)
                    {
                        VoxelData result;

                        if (objects[i].Voxels.RayCast(c.Camera.Position, worldPosRayDir, objects[i].VoxelSide, out result))
                        {
                            //Console.WriteLine("Hit!");
                            bmp.SetPixel(x, y, Color.FromArgb(255, (int)(result.Color.X), (int)(result.Color.Y), (int)(result.Color.Z)));
                        }
                    }

                }
            }

            bmp.Save("tmp.png");

        }

        public int AddObject(GIObject obj)
        {
            objects.Add(obj);
            return objects.Count - 1;
        }

        public void InjectLight(PointLight pLight)
        {
            //Find the nearest object, check if it's within the range of the falloff, if so, determine how much lighting to inject onto it
            pLights.Add(pLight);
        }

        public void InjectLight(DirectionalLight d)
        {
            //Evaluates all nearest objects, injecting light onto them
            dLights.Add(d);
        }
    }
}
