#if OPENGL && PC

using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro.Math;
using System.Runtime.InteropServices;

namespace Kokoro.OpenGL.PC
{
    static class HelperClass
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }

    public class VertexBufferLL : IDisposable
    {

        public Matrix4 World { get; set; }
        public int VertexCount { get; set; }
        public uint IndexCount { get; set; }
        public Engine.DrawMode DrawMode { get; set; }

        private int vaID;
        private int vboID;
        private int iboID;
        private int uvID;
        private int normID;
        private int tanID;
        private Type indexType;


        public VertexBufferLL()
        {
            vaID = GL.GenVertexArray();
            DrawMode = Engine.DrawMode.Triangles;
        }

        public void SetVertices<T>(T[] vertices, int index, int length) where T : struct
        {
            SetVertices(vertices.SubArray(index, length));
        }
        public void SetVertices<T>(T[] vertices) where T : struct
        {
            GL.BindVertexArray(vaID);

            vboID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.BufferData<T>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Marshal.SizeOf(typeof(T))), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            VertexCount = vertices.Length;
            GL.BindVertexArray(0);
        }

        public void SetUVs<T>(T[] uvs) where T : struct
        {
            GL.BindVertexArray(vaID);

            uvID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, uvID);
            GL.BufferData<T>(BufferTarget.ArrayBuffer, (IntPtr)(uvs.Length * Marshal.SizeOf(typeof(T))), uvs, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(0);
        }

        public void SetNormals<T>(T[] normals) where T : struct
        {
            GL.BindVertexArray(vaID);

            normID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, normID);
            GL.BufferData<T>(BufferTarget.ArrayBuffer, (IntPtr)(normals.Length * Marshal.SizeOf(typeof(T))), normals, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(0);
        }

        public void SetTangents<T>(T[] normals) where T : struct
        {
            GL.BindVertexArray(vaID);

            tanID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, tanID);
            GL.BufferData<T>(BufferTarget.ArrayBuffer, (IntPtr)(normals.Length * Marshal.SizeOf(typeof(T))), normals, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(0);
        }

        public void SetIndices<T>(T[] indices) where T : struct
        {
            GL.BindVertexArray(vaID);

            iboID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, iboID);
            GL.BufferData<T>(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * Marshal.SizeOf(typeof(T))), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            IndexCount = (uint)indices.Length;
            indexType = typeof(T);
            GL.BindVertexArray(0);
        }

        public void Draw(int count)
        {
            DrawElementsType drawEl = (indexType == typeof(uint)) ? DrawElementsType.UnsignedInt : DrawElementsType.UnsignedShort;
            GL.DrawElements((BeginMode)Enum.Parse(typeof(BeginMode), DrawMode.ToString()), count, drawEl, 0);
            //GL.Finish();
        }

        public void Bind()
        {
            int vbo = vboID;
            int vao = vaID;
            int uv = uvID;
            int normal = normID;


            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, iboID);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.VertexAttribDivisor(0, 0);

            if (uv != 0)
            {
                GL.EnableVertexAttribArray(1);
                GL.BindBuffer(BufferTarget.ArrayBuffer, uv);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
                GL.VertexAttribDivisor(1, 0);
            }

            if (normal != 0)
            {
                GL.EnableVertexAttribArray(2);
                GL.BindBuffer(BufferTarget.ArrayBuffer, normal);
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
                GL.VertexAttribDivisor(2, 0);
            }

            if (tanID != 0)
            {
                GL.EnableVertexAttribArray(3);
                GL.BindBuffer(BufferTarget.ArrayBuffer, tanID);
                GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 0, 0);
                GL.VertexAttribDivisor(3, 0);
            }
        }

        public static void UnBind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(vaID);
            vaID = -1;

            if (vboID != 0) GL.DeleteBuffer(vboID);
            vboID = -1;

            if (iboID != 0) GL.DeleteBuffer(iboID);
            iboID = -1;

            if (uvID != 0) GL.DeleteBuffer(uvID);
            uvID = -1;

            if (normID != 0) GL.DeleteBuffer(normID);
            normID = -1;
        }
    }
}
#endif