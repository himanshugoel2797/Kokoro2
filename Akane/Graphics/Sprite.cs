﻿using Kokoro2.Engine;
using Kokoro2.Engine.Prefabs;
using Kokoro2.Engine.Shaders;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Akane.Graphics
{
    public struct FrameData
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
    }

    public class Sprite
    {
        public bool Visible { get; set; }
        public Vector2 Position { get; set; }
        Texture spriteImg;
        public Dictionary<string, FrameData> Frames { get; set; }
        public Dictionary<string, string[]> Animations { get; set; }
        public Vector2 Size { get; private set; }
        public Vector2 Scale { get; set; }
        string curAnimation;
        int interval;
        int curFrame, curInterval;
        ShaderProgram spriteShader;
        Model spriteQuad;
        FrameBuffer buffer;

        public Sprite(string animations, AkaneManager a)
        {
            Visible = true;
            LoadAnimations(animations, a);

            spriteShader = new ShaderProgram(a.context, new VertexShader(Akane.Shaders.ShaderLibrary.LoadFile("Shaders/Sprite"), a.context), new FragmentShader(Akane.Shaders.ShaderLibrary.LoadFile("Shaders/Sprite"), a.context));
            //spriteQuad = new Quad(0, 0, 10, 10);
            spriteQuad = new FullScreenQuad(a.context);
            Scale = new Vector2(1, 1);
        }

        public void SetAnimation(string name, int interval, int defaultFrame)
        {
            curAnimation = name;
            curFrame = defaultFrame;
            curInterval = 0;
            this.interval = interval;
        }

        public void Draw(AkaneManager manager)
        {
            if (curInterval == interval)
            {
                curInterval = 0;
                curFrame++;
            }
            curFrame %= Animations[curAnimation].Length;
            curInterval++;

            FrameData img = Frames[Animations[curAnimation][curFrame]];
            Size = new Vector2(img.Width, img.Height);

            if (buffer == null)
            {
                buffer = new FrameBuffer(512, 512, manager.context);
                buffer.Add("DepthBuffer", DepthTextureSource.Create(512, 512, PixelComponentType.D32, manager.context), FrameBufferAttachments.DepthAttachment, manager.context);
                buffer.Add("Color", FramebufferTextureSource.Create(512, 512, 0, PixelComponentType.RGBA8, PixelType.Float, manager.context), FrameBufferAttachments.ColorAttachment0, manager.context);
            }
            //buffer.Bind(manager.context);

            //Draw sprite
            spriteQuad.RenderInfo.World = Matrix4.Scale(manager.AspectRatio * 2 * Scale.X, Scale.Y, 1) * Matrix4.CreateTranslation(Position.X * manager.AspectRatio * 2 * 0.25f, -Position.Y * 0.25f, 0);

            spriteShader["TexSize"] = new Vector2(spriteImg.Width, spriteImg.Height);
            spriteShader["TexPos"] = new Vector2(img.X, img.Y);
            spriteShader["SpriteSize"] = Size;
            spriteShader["AlbedoMap"] = spriteImg;
            manager.context.Draw(spriteQuad);

            //EngineCore.SetTexture(Textures.GetTexture(spriteImg), 0);
            //EngineCore.DrawRect(Position.X, Position.Y, img.X, img.Y, img.Width, img.Height);
            //buffer.UnBind();
        }

        private void LoadAnimations(string animationPath, AkaneManager a)
        {
            string curName = "";
            List<string> tmpList = new List<string>();
            Animations = new Dictionary<string, string[]>();
            using (XmlReader doc = XmlReader.Create(animationPath))
                while (doc.Read())
                    if (doc.IsStartElement())
                    {
                        switch (doc.Name)
                        {
                            case "Animations":
                                LoadSpriteSheet(Path.Combine(Path.GetDirectoryName(animationPath), doc["src"]), a);
                                break;
                            case "Animation":
                                Animations[doc["Name"]] = null;
                                if (curName != "") Animations[curName] = tmpList.ToArray();
                                curName = doc["Name"];
                                tmpList.Clear();
                                break;
                            case "Frame":
                                tmpList.Add(doc["Id"]);
                                break;
                        }
                    }

            Animations[curName] = tmpList.ToArray();
        }

        private void LoadSpriteSheet(string spritePath, AkaneManager a)
        {
            Frames = new Dictionary<string, FrameData>();
            using (XmlReader doc = XmlReader.Create(spritePath))
                while (doc.Read())
                    if (doc.IsStartElement())
                    {
                        switch (doc.Name)
                        {
                            case "TextureAtlas":
                                string tmpImg = doc["imagePath"];
                                if (!Path.IsPathRooted(tmpImg)) tmpImg = Path.Combine(Path.GetDirectoryName(spritePath), tmpImg);
                                spriteImg = ImageTextureSource.Create(tmpImg, 0, true, a.context);
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



        }

        public Sprite()
        {
            Visible = false;
        }

    }
}