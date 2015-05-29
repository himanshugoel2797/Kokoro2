using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AnimationMaker
{

    public struct FrameData
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
    }

    public partial class Form1 : Form
    {
        public Dictionary<string, FrameData> Frames { get; set; }
        public Dictionary<string, List<string>> Animations { get; set; }
        string spriteImg;
        string atlasPath;
        Bitmap baseImg;

        public Form1()
        {
            InitializeComponent();
            Animations = new Dictionary<string, List<string>>();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void importTPXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "TexturePacker XML | *.xml";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                atlasPath = openFileDialog1.FileName;
                Frames = new Dictionary<string, FrameData>();
                using (XmlReader doc = XmlReader.Create(openFileDialog1.FileName))
                    while (doc.Read())
                        if (doc.IsStartElement())
                        {
                            switch (doc.Name)
                            {
                                case "TextureAtlas":
                                    spriteImg = doc["imagePath"];
                                    break;
                                case "sprite":
                                    Frames[doc["n"]] = new FrameData()
                                    {
                                        X = Convert.ToInt32(doc["x"]),
                                        Y = Convert.ToInt32(doc["y"]),
                                        Width = Convert.ToInt32(doc["w"]),
                                        Height = Convert.ToInt32(doc["h"])
                                    };
                                    break;
                            }
                        }

                if (!Path.IsPathRooted(spriteImg))
                {
                    spriteImg = Path.Combine(Path.GetDirectoryName(openFileDialog1.FileName), spriteImg);
                }

                baseImg = new Bitmap(spriteImg);

                var tmp = Frames.ToArray();
                Array.Sort(tmp, new AlphanumComparatorFast());

                foreach (KeyValuePair<string, FrameData> entries in tmp)
                {
                    imageList1.Images.Add(entries.Key, baseImg.Clone(new Rectangle(entries.Value.X, entries.Value.Y, entries.Value.Width, entries.Value.Height), System.Drawing.Imaging.PixelFormat.DontCare));
                    listView1.Items.Add(entries.Key, entries.Key);
                }

                //listView1.Sort();

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            FrameData cur = Frames[listView1.SelectedItems[0].Text];
            pictureBox1.Image = baseImg.Clone(
                new Rectangle(cur.X, cur.Y, cur.Width, cur.Height)
                , System.Drawing.Imaging.PixelFormat.DontCare);

            textBox1.Text = listView1.SelectedItems[0].Text;
            textBox2.Text = cur.X.ToString();
            textBox3.Text = cur.Y.ToString();
            textBox4.Text = cur.Width.ToString();
            textBox5.Text = cur.Height.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            InputBox inputBox = new InputBox();
            if (inputBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Animations.ContainsKey(inputBox.AnimationName))
                {
                    MessageBox.Show("The animation name must be unique!");
                    return;
                }
                comboBox1.Items.Add(inputBox.AnimationName);
                Animations[inputBox.AnimationName] = new List<string>();
                comboBox1.SelectedItem = inputBox.AnimationName;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            frameNo = 0;
            listView2.Clear();
            foreach (string s in Animations[comboBox1.SelectedItem.ToString()])
            {
                listView2.Items.Add(s, s);
            }
            lV2LastIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirm delete?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                Animations.Remove(comboBox1.SelectedItem.ToString());
                comboBox1.Items.Remove(comboBox1.SelectedItem);
                comboBox1.ResetText();
            }
        }

        int lV2LastIndex = 0;
        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0) return;
            FrameData cur = Frames[listView2.SelectedItems[0].Text];
            pictureBox1.Image = baseImg.Clone(
                new Rectangle(cur.X, cur.Y, cur.Width, cur.Height)
                , System.Drawing.Imaging.PixelFormat.DontCare);

            textBox1.Text = listView2.SelectedItems[0].Text;
            textBox2.Text = cur.X.ToString();
            textBox3.Text = cur.Y.ToString();
            textBox4.Text = cur.Width.ToString();
            textBox5.Text = cur.Height.ToString();
            lV2LastIndex = listView2.SelectedIndices[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int indexAt = lV2LastIndex;
            lV2LastIndex++;

            Animations[comboBox1.SelectedItem.ToString()].Insert(indexAt, listView1.SelectedItems[0].Text);
            listView2.Clear();
            foreach (string s in Animations[comboBox1.SelectedItem.ToString()])
            {
                listView2.Items.Add(s, s);
            }
        }

        int frameNo = 0;
        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            frameNo++;
            frameNo = frameNo % Animations[comboBox1.SelectedItem.ToString()].Count;

            FrameData cur = Frames[listView2.Items[frameNo].Text];
            pictureBox1.Image = baseImg.Clone(
                new Rectangle(cur.X, cur.Y, cur.Width, cur.Height)
                , System.Drawing.Imaging.PixelFormat.DontCare);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int indexAt = lV2LastIndex;
            lV2LastIndex--;
            Animations[comboBox1.SelectedItem.ToString()].RemoveAt(indexAt);
            listView2.Clear();
            foreach (string s in Animations[comboBox1.SelectedItem.ToString()])
            {
                listView2.Items.Add(s, s);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "AnimationMaker XML | *.xml";
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (XmlWriter writer = XmlWriter.Create(saveFileDialog1.FileName))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Animations");
                    writer.WriteAttributeString("src", atlasPath);

                    foreach (KeyValuePair<string, List<string>> frames in Animations)
                    {
                        writer.WriteStartElement("Animation");

                        writer.WriteAttributeString("Name", frames.Key);

                        foreach (string s in frames.Value)
                        {
                            writer.WriteStartElement("Frame");
                            writer.WriteAttributeString("Id", s);
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "AnimationMaker XML | *.xml";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Animations = new Dictionary<string, List<string>>();

                Frames = new Dictionary<string, FrameData>();
                string curName = "";
                using (XmlReader reader = XmlReader.Create(openFileDialog1.FileName))
                    while (reader.Read())
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "Animations":
                                    atlasPath = reader["src"];
                                    using (XmlReader doc = XmlReader.Create(atlasPath))
                                        while (doc.Read())
                                            if (doc.IsStartElement())
                                            {
                                                switch (doc.Name)
                                                {
                                                    case "TextureAtlas":
                                                        spriteImg = doc["imagePath"];
                                                        break;
                                                    case "sprite":
                                                        Frames[doc["n"]] = new FrameData()
                                                        {
                                                            X = Convert.ToInt32(doc["x"]),
                                                            Y = Convert.ToInt32(doc["y"]),
                                                            Width = Convert.ToInt32(doc["w"]),
                                                            Height = Convert.ToInt32(doc["h"])
                                                        };
                                                        break;
                                                }
                                            }

                                    if (!Path.IsPathRooted(spriteImg))
                                    {
                                        spriteImg = Path.Combine(Path.GetDirectoryName(openFileDialog1.FileName), spriteImg);
                                    }

                                    baseImg = new Bitmap(spriteImg);

                                    foreach (KeyValuePair<string, FrameData> entries in Frames)
                                    {
                                        imageList1.Images.Add(entries.Key, baseImg.Clone(new Rectangle(entries.Value.X, entries.Value.Y, entries.Value.Width, entries.Value.Height), System.Drawing.Imaging.PixelFormat.DontCare));
                                        listView1.Items.Add(entries.Key, entries.Key);
                                    }
                                    break;
                                case "Animation":
                                    Animations[reader["Name"]] = new List<string>();
                                    curName = reader["Name"];
                                    comboBox1.Items.Add(curName);
                                    break;
                                case "Frame":
                                    Animations[curName].Add(reader["Id"]);
                                    break;
                            }
                        }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Exit", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes) this.Close();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            int interval = 10;
            if (Int32.TryParse(textBox6.Text, out interval)) timer1.Interval = interval;
        }

    }
}
