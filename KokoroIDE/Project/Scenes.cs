using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro.IDE.Project
{
    public class Scene : ISource
    {
        public string Name
        {
            get; set;
        }

        public List<LightSource> LightSources { get; set; }
        public List<SoundSource> SoundSources { get; set; }
        public List<MeshSource> MeshSources { get; set; }
        public List<int> ChildScenes { get; set; }
        public int Parent { get; set; }

        public Scene()
        {
            LightSources = new List<LightSource>();
            SoundSources = new List<SoundSource>();
            MeshSources = new List<MeshSource>();
            ChildScenes = new List<int>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
