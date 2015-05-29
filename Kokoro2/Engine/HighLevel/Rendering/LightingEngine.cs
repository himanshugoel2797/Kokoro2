using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro2.Math;
using Kokoro2.Engine;
using Kokoro2.Debug;
using Kokoro2.Engine.SceneGraph;
using Kokoro2.Engine.Prefabs;
using Kokoro2.Engine.Shaders;
using Kokoro2.Engine.HighLevel.Cameras;


namespace Kokoro2.Engine.HighLevel.Rendering
{
    public class LightingEngine
    {
        private GBuffer FrontFaces;
        private FrameBuffer hemisphericalSamples, radiositySamplesPassA, radiositySamplesPassB;
        private ShaderProgram hemisphereSampler, compositor, radiosityPassA, radiosityPassB;
        private Model fsq = new FullScreenQuad();

        public ShaderProgram Shader
        {
            get
            {
                return FrontFaces.GBufferShader;
            }
        }

        public Action<double, GraphicsContext> Draw { get; set; }

        public LightingEngine(int width, int height, GraphicsContext context)
        {
            FrontFaces = new GBuffer(width, height, context);
            
            hemisphericalSamples = new FrameBuffer(width/2, height/2, PixelComponentType.RGBA8, context);

            radiositySamplesPassA = new FrameBuffer(width/10, height/10, PixelComponentType.RGBA8, context);
            radiositySamplesPassA.Add("LightPositions", new FrameBufferTexture(width/10, height/10, PixelFormat.BGRA, PixelComponentType.RGBA8, PixelType.Float), FrameBufferAttachments.ColorAttachment1, context);
            
            radiositySamplesPassB = new FrameBuffer(width / 4, height / 4, PixelComponentType.RGBA16f, context);

            hemisphereSampler = new ShaderProgram(VertexShader.Load("Shaders/HemisphereSampler"), FragmentShader.Load("Shaders/HemisphereSampler"));
            hemisphereSampler["FNormal"] = FrontFaces["Normal0"];
            hemisphereSampler["FRGBA"] = FrontFaces["RGBA0"];
            hemisphereSampler["FDepth"] = FrontFaces["Depth0"];

            radiosityPassA = new ShaderProgram(VertexShader.Load("Shaders/Radiosity PassA"), FragmentShader.Load("Shaders/Radiosity PassA"));
            radiosityPassA["FNormal"] = FrontFaces["Normal0"];
            radiosityPassA["FRGBA"] = FrontFaces["RGBA0"];
            radiosityPassA["FDepth"] = FrontFaces["Depth0"];

            radiosityPassB = new ShaderProgram(VertexShader.Load("Shaders/Radiosity PassB"), FragmentShader.Load("Shaders/Radiosity PassB"));
            radiosityPassB["FNormal"] = FrontFaces["Normal0"];
            radiosityPassB["FRGBA"] = FrontFaces["RGBA0"];
            radiosityPassB["FDepth"] = FrontFaces["Depth0"];
            radiosityPassB["RadiosityPassA"] = radiositySamplesPassA["Color"];
            radiosityPassB["VPLPositions"] = radiositySamplesPassA["LightPositions"];
            
            compositor = new ShaderProgram(VertexShader.Load("Shaders/Compositor"), FragmentShader.Load("Shaders/Compositor"));
            compositor["RGBA0"] = FrontFaces["RGBA0"];      //The front facing colors
            compositor["Depth0"] = FrontFaces["Depth0"];
            compositor["Normal0"] = FrontFaces["Normal0"];
            compositor["HemisphereSample"] = hemisphericalSamples["Color"];
            compositor["RadiositySample"] = radiositySamplesPassB["Color"];
        }

        public void Render(double interval, GraphicsContext context)
        {
            //Draw the front faces
            FrontFaces.Bind(context);
            context.Clear(1, 0, 0, 0);
            context.FaceCulling = CullMode.Back;    //Cull back faces
            if (Draw != null) Draw(interval, context);

            //Perform hemisphere sampling on the rendered data, using both the front face buffer and the back face buffer
            hemisphericalSamples.Bind(context);
            context.Clear(1, 0, 0, 0);
            fsq.Materials[0].Shader = hemisphereSampler;
            fsq.Draw(context);      //Perform the sampling

            radiositySamplesPassA.Bind(context);
            context.Clear(1, 0, 0, 0);
            fsq.Materials[0].Shader = radiosityPassA;
            fsq.Draw(context);

            radiositySamplesPassB.Bind(context);
            context.Clear(1, 0, 0, 0);
            fsq.Materials[0].Shader = radiosityPassB;
            fsq.Draw(context);

            //Reset the framebuffer bindings
            radiositySamplesPassB.UnBind(context);
            context.Viewport = new Vector4(0, 0, context.WindowSize.X, context.WindowSize.Y);

            //Finally composite the results to the backbuffer
            fsq.Materials[0].Shader = compositor;
            fsq.Draw(context);
        }


    }
}