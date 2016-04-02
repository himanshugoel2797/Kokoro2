﻿using System;
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

    public class Model : IEngineObject
    {
        #region Graphics Methods
        public RenderInfo RenderInfo;
        public GeometryInfo GeometryInfo;

        private VertexArrayLL Buffer { get { return GeometryInfo.Buffer; } set { GeometryInfo.Buffer = value; } }


        protected void SetUVs(float[] uvs, int index)
        {
            Buffer[3].AppendData(uvs);
        }

        protected void SetNormals(float[] norms, int index)
        {
            Buffer[2].AppendData(norms);
        }

        protected void SetVertices(float[] verts, int index)
        {
            Buffer[1].AppendData(verts);
        }

        protected void SetIndices(uint[] indices, int index)
        {
            Buffer[0].AppendData(indices);
        }

        #region Data Updates
        protected void UpdateUVs(float[] uvs, int index)
        {
            Buffer[3].BufferData(uvs, 0, uvs.Length * sizeof(float));
        }
        protected void UpdateNormals(float[] norms, int index)
        {
            Buffer[2].BufferData(norms, 0, norms.Length * sizeof(float));
        }
        protected void UpdateVertices(float[] verts, int index)
        {
            Buffer[1].BufferData(verts, 0, verts.Length * sizeof(float));
        }
        protected void UpdateIndices(uint[] indices, int index)
        {
            Buffer[0].BufferData(indices, 0, indices.Length * sizeof(uint));
        }
        #endregion

        public Material Material { get { return RenderInfo.Material; } set { RenderInfo.Material = value; } }
        public DrawMode DrawMode { get { return RenderInfo.DrawMode; } set { RenderInfo.DrawMode = value; } }
        public BoundingBox Bound;

        public ShaderProgram Shader
        {
            get
            {
                return RenderInfo.Shader;
            }
        }
        #endregion

        #region Physics Methods
        public PhysicsInfo PhysicsInfo;
        #endregion

        public ulong ID
        {
            get; set;
        }

        public GraphicsContext ParentContext
        {
            get; set;
        }

        public Model()
        {
            Material = new Material();
            DrawMode = Engine.DrawMode.Triangles;

            Kokoro2.Engine.ObjectAllocTracker.NewCreated(this);
        }

        ~Model()
        {
            Kokoro2.Engine.ObjectAllocTracker.ObjectDestroyed(this);
        }

        public void Dispose()
        {
            //Nothing unless we end up needing to setup the defragmentation mechanism
        }
    }
}
