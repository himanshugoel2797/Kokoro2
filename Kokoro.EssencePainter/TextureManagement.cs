using Kokoro.EssencePainter.Editor;
using Kokoro2.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Kokoro.EssencePainter
{
    public partial class Form1
    {
        private Dictionary<string, TextureSet> textureSets = new Dictionary<string, TextureSet>();
        private Dictionary<string, Bitmap> textures = new Dictionary<string, Bitmap>();
        private Dictionary<string, FileSystemWatcher> watchers = new Dictionary<string, FileSystemWatcher>();
        private string activeSet = "";

        private TreeNode TextureSetToTreeNodes(TextureSet t)
        {
            TreeNode parent = new TreeNode();
            parent.Tag = t;
            parent.Text = t.Name;

            if (t.NormalMap) parent.Nodes.Add($"Normal Map (${t.NormalMapFile})");
            if (t.ReflectivityMap) parent.Nodes.Add($"Reflectivity Map (${t.ReflectivityMapFile})");
            if (t.RoughnessMap) parent.Nodes.Add($"Roughness Map (${t.RoughnessMapFile})");
            if (t.AlbedoMap) parent.Nodes.Add($"Albedo Map (${t.AlbedoMapFile})");
            if (t.EmissiveMap) parent.Nodes.Add($"Emissive Map (${t.EmissiveMapFile})");

            return parent;
        }

        public void AddNewTextureSet(TextureSet set)
        {
            textureSets.Add(set.Name, set);
            texturesTree.Nodes.Add(TextureSetToTreeNodes(set));

            XmlSerializer s = new XmlSerializer(typeof(TextureSet));
            using (Stream strm = File.Open(set.File, FileMode.Create)) s.Serialize(strm, set);

            //if (set.NormalMap && !textures.ContainsKey(set.NormalMapFile)) textures.Add(set.NormalMapFile, new Bitmap(set.Width, set.Height));
            //if (set.ReflectivityMap && !textures.ContainsKey(set.ReflectivityMapFile)) textures.Add(set.ReflectivityMapFile, new Bitmap(set.Width, set.Height));
            //if (set.RoughnessMap && !textures.ContainsKey(set.RoughnessMapFile)) textures.Add(set.RoughnessMapFile, new Bitmap(set.Width, set.Height));
            //if (set.AlbedoMap && !textures.ContainsKey(set.AlbedoMapFile)) textures.Add(set.AlbedoMapFile, new Bitmap(set.Width, set.Height));
            //if (set.EmissiveMap && !textures.ContainsKey(set.EmissiveMapFile)) textures.Add(set.EmissiveMapFile, new Bitmap(set.Width, set.Height));

            if (set.NormalMap | set.ReflectivityMap | set.RoughnessMap | set.AlbedoMap | set.EmissiveMap)
            {
                FileSystemWatcher w = new FileSystemWatcher(Path.GetDirectoryName(set.NormalMapFile));
                w.Changed += File_Changed;
                w.NotifyFilter = NotifyFilters.LastWrite;
                w.Filter = "*.png";
                w.EnableRaisingEvents = true;
                if (!watchers.ContainsKey(set.Name))
                    watchers.Add(set.Name, w);
            }
        }

        public void SaveTextureSet(TextureSet set)
        {
            XmlSerializer s = new XmlSerializer(typeof(TextureSet));
            using (Stream strm = File.Open(set.File, FileMode.Create)) s.Serialize(strm, set);
        }

        private void File_Changed(object sender, FileSystemEventArgs e)
        {
            if (activeSet == "") return;

            if (textureSets[activeSet].AlbedoMapFile == e.FullPath)
            {
                editorManager.LoadTextureSet(textureSets[activeSet]);
            }
            else

            if (textureSets[activeSet].EmissiveMapFile == e.FullPath)
            {
                Thread.Sleep(1000);
                editorManager.LoadTextureSet(textureSets[activeSet]);
            }
            else

            if (textureSets[activeSet].ReflectivityMapFile == e.FullPath)
            {
                Thread.Sleep(1000);
                editorManager.LoadTextureSet(textureSets[activeSet]);
            }
            else

            if (textureSets[activeSet].RoughnessMapFile == e.FullPath)
            {
                Thread.Sleep(1000);
                editorManager.LoadTextureSet(textureSets[activeSet]);
            }
            else

            if (textureSets[activeSet].NormalMapFile == e.FullPath)
            {
                Thread.Sleep(1000);
                editorManager.LoadTextureSet(textureSets[activeSet]);
            }
        }

        public void AddExistingTextureSet(string setLoc)
        {
            TextureSet set;
            using (Stream strm = File.OpenRead(setLoc)) set = (TextureSet)new XmlSerializer(typeof(TextureSet)).Deserialize(strm);

            textureSets.Add(set.Name, set);
            texturesTree.Nodes.Add(TextureSetToTreeNodes(set));

            if (set.NormalMap | set.ReflectivityMap | set.RoughnessMap | set.AlbedoMap | set.EmissiveMap)
            {
                FileSystemWatcher w = new FileSystemWatcher(Path.GetDirectoryName(set.NormalMapFile));
                w.Changed += File_Changed;
                w.NotifyFilter = NotifyFilters.LastWrite;
                w.Filter = "*.png";
                w.EnableRaisingEvents = true;
                if (!watchers.ContainsKey(set.Name))
                    watchers.Add(set.Name, w);
            }
            ////Load in the textures if they haven't been loaded in already
            //if (set.NormalMap && !textures.ContainsKey(set.NormalMapFile)) textures.Add(set.NormalMapFile, new Bitmap(set.NormalMapFile));
            //if (set.ReflectivityMap && !textures.ContainsKey(set.ReflectivityMapFile)) textures.Add(set.ReflectivityMapFile, new Bitmap(set.ReflectivityMapFile));
            //if (set.RoughnessMap && !textures.ContainsKey(set.RoughnessMapFile)) textures.Add(set.RoughnessMapFile, new Bitmap(set.RoughnessMapFile));
            //if (set.AlbedoMap && !textures.ContainsKey(set.AlbedoMapFile)) textures.Add(set.AlbedoMapFile, new Bitmap(set.AlbedoMapFile));
            //if (set.EmissiveMap && !textures.ContainsKey(set.EmissiveMapFile)) textures.Add(set.EmissiveMapFile, new Bitmap(set.EmissiveMapFile));
        }

        public void LoadTextureSetToDatabase(TextureSet set)
        {
            //Load in the textures if they haven't been loaded in already
            if (set.NormalMap && !textures.ContainsKey(set.NormalMapFile)) textures.Add(set.NormalMapFile, new Bitmap(set.NormalMapFile));
            if (set.ReflectivityMap && !textures.ContainsKey(set.ReflectivityMapFile)) textures.Add(set.ReflectivityMapFile, new Bitmap(set.ReflectivityMapFile));
            if (set.RoughnessMap && !textures.ContainsKey(set.RoughnessMapFile)) textures.Add(set.RoughnessMapFile, new Bitmap(set.RoughnessMapFile));
            if (set.AlbedoMap && !textures.ContainsKey(set.AlbedoMapFile)) textures.Add(set.AlbedoMapFile, new Bitmap(set.AlbedoMapFile));
            if (set.EmissiveMap && !textures.ContainsKey(set.EmissiveMapFile)) textures.Add(set.EmissiveMapFile, new Bitmap(set.EmissiveMapFile));
        }
    }
}
