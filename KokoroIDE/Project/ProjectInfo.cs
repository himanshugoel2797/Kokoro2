using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Kokoro.IDE.Project
{
    public class ProjectInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SaveDir { get; set; }
        
        public Scene CurrentScene { get; set; }
        public List<Scene> Scenes { get; set; }

        public ProjectInfo()
        {
            Scenes = new List<Scene>();
        }

        public void SaveProject()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectInfo));
            using (FileStream f = File.Create(Path.Combine(SaveDir, Name.Trim(Path.GetInvalidFileNameChars()) + ".hg"))) serializer.Serialize(f, this);
        }

        public static ProjectInfo LoadProject(string hgFile)
        {
            if (!File.Exists(hgFile)) return null;

            ProjectInfo pI = null;
            XmlSerializer deserializer = new XmlSerializer(typeof(ProjectInfo));
            using (FileStream f = File.OpenRead(hgFile)) pI = (ProjectInfo)deserializer.Deserialize(f);

            return pI;
        }
    }
}
