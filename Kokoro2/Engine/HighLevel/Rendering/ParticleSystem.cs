using Kokoro2.Engine.Prefabs;
using Kokoro2.Engine.Shaders;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Rendering
{
    public class ParticleSystem
    {
        public Vector3 EmitterBoxLocation { get; set; }
        public Vector3 EmitterLocation { get; set; }
        public Vector3 EmitterSphereRadius { get; set; }
        public int ParticleCount { get; private set; }
        public Vector3 Impulse { get; set; }
        public float MinMass { get; set; }
        public float MaxMass { get; set; }
        public Material Material
        {
            get { return particleMesh.Material; }
            set { particleMesh.Material = value; }
        }
        public float BloomFactor { get; set; }
        public Matrix4 World;

        private FrameBuffer particleBufferA, particleBufferB;
        private FrameBuffer currentBuffer, otherBuffer;
        private FullScreenQuad fsq;
        private ShaderProgram particleAnimationShader;
        private ShaderProgram particleRenderer;
        private InputDataMesh particleMesh;

        public ParticleSystem(int particleSide, GraphicsContext c)
        {
            ParticleCount = particleSide * particleSide;

            particleBufferA = new FrameBuffer(particleSide, particleSide, c);
            particleBufferA.Add("PosData", FramebufferTextureSource.Create(particleBufferA.Width, particleBufferA.Height, 0, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);
            particleBufferA.Add("dVData", FramebufferTextureSource.Create(particleBufferA.Width, particleBufferA.Height, 0, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment1, c);

            particleBufferB = new FrameBuffer(particleSide, particleSide, c);
            particleBufferB.Add("PosData", FramebufferTextureSource.Create(particleBufferB.Width, particleBufferB.Height, 0, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment0, c);
            particleBufferB.Add("dVData", FramebufferTextureSource.Create(particleBufferB.Width, particleBufferB.Height, 0, PixelComponentType.RGBA16f, PixelType.Float, c), FrameBufferAttachments.ColorAttachment1, c);

            particleAnimationShader = new ShaderProgram(c, VertexShader.Load("SimpleParticleSystem", c), FragmentShader.Load("SimpleParticleSystem", c));
            particleRenderer = new ShaderProgram(c, VertexShader.Load("ParticleRenderer", c), FragmentShader.Load("ParticleRenderer", c));

            fsq = new FullScreenQuad(c);
            fsq.RenderInfo.PushShader(particleAnimationShader);

            //Setup render info
            particleMesh = new InputDataMesh(particleSide * particleSide, c);
            float[] verts = new float[particleSide * particleSide * 3];

            int x = 0, y = 0;

            for (int i = 0; i < ParticleCount; i++)
            {
                verts[i * 3] = x++;
                verts[i * 3 + 1] = y;

                if (x == particleSide)
                {
                    x = 0;
                    y++;
                }
            }
            particleMesh.GenerateMeshData(verts);
            particleMesh.RenderInfo.PushShader(particleRenderer);
            particleMesh.DrawMode = DrawMode.Points;

            //Initialize particle buffer A as the start buffer
            ShaderProgram initShader = new ShaderProgram(c, VertexShader.Load("ParticleSystemInitializer", c), FragmentShader.Load("ParticleSystemInitializer", c));
            fsq.RenderInfo.PushShader(initShader);
            particleBufferA.Bind(c);
            c.ClearColor(0, 0, 0, 0);
            c.Draw(fsq);
            fsq.RenderInfo.PopShader();

            particleBufferA.UnBind(c);

            currentBuffer = particleBufferA;
            otherBuffer = particleBufferB;
        }

        public void Draw(GraphicsContext c)
        {
            if (currentBuffer == particleBufferB)
            {
                currentBuffer = particleBufferA;
                otherBuffer = particleBufferB;
            }
            else if (currentBuffer == particleBufferA)
            {
                otherBuffer = particleBufferA;
                currentBuffer = particleBufferB;
            }
            else throw new Exception();

            currentBuffer.Bind(c);

            particleAnimationShader["DeltaTime"] = 0.1f;
            particleAnimationShader["Impulse"] = Impulse;
            particleAnimationShader["PosData"] = otherBuffer["PosData"];
            particleAnimationShader["dVData"] = otherBuffer["dVData"];
            particleAnimationShader["EmitterPosition"] = EmitterLocation;

            c.Draw(fsq);

            currentBuffer.UnBind(c);
            var backup = c.FaceCulling;
            c.FaceCulling = CullMode.Off;
            c.Blend = true;
            //c.DepthFunction = DepthFunc.Always;
            c.Blending = new BlendFunc()
            {
                Dst = BlendingFactor.One,
                Src = BlendingFactor.SrcAlpha
            };
            particleRenderer["Source"] = EmitterBoxLocation;
            particleRenderer["PosData"] = otherBuffer["PosData"];
            particleRenderer["bloomFactor"] = BloomFactor;
            particleMesh.RenderInfo.World = World;
            //c.DepthWrite = false;
            c.Draw(particleMesh);
            //c.DepthWrite = true;
            c.Blend = false;
            c.FaceCulling = backup;
        }

    }
}
