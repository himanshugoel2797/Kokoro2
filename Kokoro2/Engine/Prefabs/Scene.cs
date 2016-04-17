using Kokoro2.Math;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.Prefabs
{
    public class Scene
    {
        public List<Scene> Children { get; set; }
        public Scene Parent { get; set; }
        public Matrix4 Transform { get; set; }
        public List<Model> Models { get; set; }

        public Scene()
        {
            Children = new List<Scene>();
            Models = new List<Model>();
        }

        public static Scene Load(string filename, GraphicsContext c, bool useVFS = false)
        {
            Scene s = new Scene();

            VertexMesh.Model_m tmp = null;
            if (useVFS)
            {
                tmp = Serializer.Deserialize<VertexMesh.Model_m>(VFS.FSReader.OpenFile(filename, false));
            }
            else
            {
                tmp = Serializer.Deserialize<VertexMesh.Model_m>(System.IO.File.OpenRead(filename));
            }

            Action<VertexMesh.SceneGraph_m, Scene, Scene> build = null;
            List<Model> mods = new List<Model>();

            for (int i = 0; i < tmp.Mesh.Length; i++)
            {
                mods.Add(new VertexMesh(tmp.Mesh[i], c));
            }

            build = (a, n, p) =>
            {
                n.Transform = a.Transform;
                n.Parent = p;

                if (a.Objects != null)
                {
                    for (int i = 0; i < a.Objects.Length; i++)
                    {
                        n.Models.Add(mods[a.Objects[i]]);
                    }
                }

                if (a.Children != null)
                {
                    for (int i = 0; i < a.Children.Length; i++)
                    {
                        n.Children.Add(new Scene());
                        build(a.Children[i], n.Children[n.Children.Count - 1], n);
                    }
                }
            };
            build(tmp.Scene, s, null);

            return s;
        }
    }
}
