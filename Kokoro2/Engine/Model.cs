using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Kokoro2.Math;
using Kokoro2.Engine.Shaders;
using Kokoro2.Physics;

#if OPENGL
#if PC
using Kokoro2.OpenGL.PC;
#endif

#elif OPENGL_AZDO
#if PC
using Kokoro2.OpenGL.AZDO;
#endif
#endif

namespace Kokoro2.Engine
{
    public enum DrawMode
    {
        Triangles,
        TriangleStrip,
        Lines,
        LineStrip,
        Points,
        Patches
    }

    public enum BufferUse
    {
        Array, Index, Uniform, ShaderStorage, Indirect
    }

    public class Model : IDisposable
    {
        internal static VertexArrayLL staticBuffer;
        protected static long[] staticBufferOffset;
        protected static long[] staticBufferLength;

        internal static VertexArrayLL dynamicBuffer;
        protected static long[] dynamicBufferOffset;
        protected static long[] dynamicBufferLength;


        protected string filepath;
        protected uint[][] offsets;
        protected uint[] lengths;

        protected void Init(int num)
        {
            offsets = new uint[num][];
            lengths = new uint[num];
            Materials = new Material[num];
            for (int a = 0; a < num; a++)
            {
                offsets[a] = new uint[4];
                Materials[a] = new Material();
            }
        }
        protected void PreAlloc(UpdateMode mode, int index, int len)
        {
            if (mode == UpdateMode.Dynamic)
            {
                offsets[index][0] = dynamicBuffer[0].AllocData(len * sizeof(uint) * 1);
                offsets[index][1] = dynamicBuffer[1].AllocData(len * sizeof(float) * 3);
                offsets[index][2] = dynamicBuffer[2].AllocData(len * sizeof(float) * 3);
                offsets[index][3] = dynamicBuffer[3].AllocData(len * sizeof(float) * 2);
            }
            else if (mode == UpdateMode.Static)
            {
                offsets[index][0] = staticBuffer[0].AllocData(len * sizeof(float) * 1);
                offsets[index][1] = staticBuffer[1].AllocData(len * sizeof(float) * 3);
                offsets[index][2] = staticBuffer[2].AllocData(len * sizeof(float) * 3);
                offsets[index][3] = staticBuffer[3].AllocData(len * sizeof(float) * 2);
            }
            lengths[index] = (uint)len;
            //this.mode = mode;
        }
        protected void SetUVs(UpdateMode mode, float[] uvs, int index)
        {
            if (mode == UpdateMode.Dynamic)
            {
                offsets[index][3] = dynamicBuffer[3].AppendData(uvs);
            }
            else if (mode == UpdateMode.Static)
            {
                offsets[index][3] = staticBuffer[3].AppendData(uvs);
            }
        }
        protected void SetNormals(UpdateMode mode, float[] norms, int index)
        {
            if (mode == UpdateMode.Dynamic)
            {
                offsets[index][2] = dynamicBuffer[2].AppendData(norms);
            }
            else if (mode == UpdateMode.Static)
            {
                offsets[index][2] = staticBuffer[2].AppendData(norms);
            }
        }
        protected void SetVertices(UpdateMode mode, float[] verts, int index)
        {
            if (mode == UpdateMode.Dynamic)
            {
                offsets[index][1] = dynamicBuffer[1].AppendData(verts);
            }
            else if (mode == UpdateMode.Static)
            {
                offsets[index][1] = staticBuffer[1].AppendData(verts);
            }
        }
        protected void SetIndices(UpdateMode mode, uint[] indices, int index)
        {
            if (mode == UpdateMode.Dynamic)
            {
                offsets[index][0] = dynamicBuffer[0].AppendData(indices);
            }
            else if (mode == UpdateMode.Static)
            {
                offsets[index][0] = staticBuffer[0].AppendData(indices);
            }
            if (lengths[index] < indices.Length) lengths[index] = (uint)indices.Length;
            this.mode = mode;
        }

        #region Data Updates
        protected void UpdateUVs(float[] uvs, int index)
        {
            if (mode == UpdateMode.Dynamic)
            {
                dynamicBuffer[3].BufferData(uvs, (int)offsets[index][3], uvs.Length * sizeof(float));
            }
            else if (mode == UpdateMode.Static)
            {
                staticBuffer[3].BufferData(uvs, (int)offsets[index][3], uvs.Length * sizeof(float));
            }
        }
        protected void UpdateNormals(float[] norms, int index)
        {
            if (mode == UpdateMode.Dynamic)
            {
                dynamicBuffer[2].BufferData(norms, (int)offsets[index][2], norms.Length * sizeof(float));
            }
            else if (mode == UpdateMode.Static)
            {
                staticBuffer[2].BufferData(norms, (int)offsets[index][2], norms.Length * sizeof(float));
            }
        }
        protected void UpdateVertices(float[] verts, int index)
        {
            if (mode == UpdateMode.Dynamic)
            {
                dynamicBuffer[1].BufferData(verts, (int)offsets[index][1], verts.Length * sizeof(float));
            }
            else if (mode == UpdateMode.Static)
            {
                staticBuffer[1].BufferData(verts, (int)offsets[index][1], verts.Length * sizeof(float));
            }
        }
        protected void UpdateIndices(uint[] indices, int index)
        {
            if (mode == UpdateMode.Dynamic)
            {
                dynamicBuffer[0].BufferData(indices, (int)offsets[index][0], indices.Length * sizeof(uint));
            }
            else if (mode == UpdateMode.Static)
            {
                staticBuffer[0].BufferData(indices, (int)offsets[index][0], indices.Length * sizeof(uint));
            }
            //The length might increase, this should actually be handled by the allocater, to make sure to request enough space to not overwrite anything
        }
        #endregion

        public Matrix4 World { get; set; }
        public Material[] Materials { get; set; }
        public DrawMode DrawMode { get; set; }
        public BoundingBox Bound;

        UpdateMode mode;
        protected bool IsDataReady = true;
        protected object syncObject = new object();

        static Model()
        {
            int numBufs = 4;

            /*
             * 0: Index
             * 1: Vertex
             * 2: Normal
             * 3: UV
             */

            //TODO: Everything is switched over to Shader storage buffers so the vertex data can be written to in all shader stages, assign a fourth buffer to store indexed material information
            if (staticBuffer != null) staticBuffer.Dispose();
            staticBufferOffset = new long[numBufs];
            staticBufferLength = new long[numBufs]; /*How much should we allocate?*/  //Current limit = 10 Million Elements
            staticBufferLength[0] = 10000000;
            staticBufferLength[1] = 30000000;
            staticBufferLength[2] = 30000000;
            staticBufferLength[3] = 20000000;
            staticBuffer = new VertexArrayLL(4, staticBufferLength[0], UpdateMode.Dynamic, new BufferUse[] { BufferUse.Index, BufferUse.Array, BufferUse.Array, BufferUse.Array }, new int[] { 1, 3, 3, 2 });


            if (dynamicBuffer != null) dynamicBuffer.Dispose();
            dynamicBufferOffset = new long[numBufs];
            dynamicBufferLength = new long[numBufs]; /*How much should we allocate?*/ //Current limit = 1 Million Elements
            dynamicBufferLength[0] = 1000000;
            dynamicBufferLength[1] = 3000000;
            dynamicBufferLength[2] = 3000000;
            dynamicBufferLength[3] = 2000000;
            dynamicBuffer = new VertexArrayLL(4, dynamicBufferLength[0], UpdateMode.Dynamic, new BufferUse[] { BufferUse.Index, BufferUse.Array, BufferUse.Array, BufferUse.Array }, new int[] { 1, 3, 3, 2 });
        }

        public static void ResetModelStorage()
        {
            staticBuffer.ResetAll();
            dynamicBuffer.ResetAll();
        }

        public Model()
        {
            Materials = new Material[] { new Material() };
            DrawMode = Engine.DrawMode.Triangles;
#if DEBUG
            Kokoro2.Debug.ObjectAllocTracker.NewCreated(this, 0, "Model");
#endif
        }
#if DEBUG
        ~Model()
        {
            Kokoro2.Debug.ObjectAllocTracker.ObjectDestroyed(this, 0, "Model");
        }
#endif

        //Use this to build a list of all the commands to send to the appropriate multidraw indirect buffers
        public void Draw(GraphicsContext context)
        {
            //lock (syncObject)
            {
                if (IsDataReady)
                {
                    //Append a draw command to the MDI queue
                    for (int a = 0; a < offsets.Length; a++)
                    {
                        //Apply the Material
                        Materials[a].Apply(context, this);      //Material pipeline will just setup textures and uniform buffer parameters somehow

                        if ((mode == UpdateMode.Static)) staticBuffer.Bind();
                        else dynamicBuffer.Bind();

                        GraphicsContextLL.Draw(DrawMode, offsets[a][0] / sizeof(uint), lengths[a], offsets[a][1] / (3 * sizeof(float)));   //Send the draw call

                        if ((mode == UpdateMode.Static)) staticBuffer.UnBind();
                        else dynamicBuffer.UnBind();

                        //Cleanup the Material
                        Materials[a].Cleanup(context, this);    //Queue the material to be cleaned out after everything has been done
                    }
                }
            }
        }

        public void Dispose()
        {
            //Nothing unless we end up needing to setup the defragmentation mechanism
        }
    }
}
