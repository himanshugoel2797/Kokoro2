using Kokoro2.Engine.Prefabs;
using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Kokoro2.Engine.HighLevel
{
    public class Font
    {
        static PrivateFontCollection fonts;
        static Model box;


        System.Drawing.Font fnt;
        Dictionary<string, Texture> fontCache;

        public Vector4 ForeColor;
        public Vector2 Scale;

        public Font(string familyName, float size, Vector4 textColor)
        {
            ForeColor = textColor;
            fontCache = new Dictionary<string, Texture>();
            fnt = new System.Drawing.Font(familyName, size);

            if (box == null)
            {
                box = new Quad(0, 0, 1, 1);
                //TODO Fix box.Materials[0].Shader = ShaderLib.GBufferShader.Create();
            }
        }

        public Font(string font, float size)
        {
            ForeColor = Vector4.One;
            fontCache = new Dictionary<string, Texture>();

            if (fonts == null) fonts = new PrivateFontCollection();
            fonts.AddFontFile(font);

            fnt = new System.Drawing.Font(fonts.Families[fonts.Families.Length - 1].Name, size);
            if (box == null)
            {
                box = new Quad(0, 0, 1, 1);
                //TODO Fix box.Materials[0].Shader = ShaderLib.UVOffsetShader.Create();
            }
        }

        private Image DrawText(String text, System.Drawing.Font font, Color textColor, Color backColor)
        {
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;

        }

        public void DrawString(GraphicsContext context, Vector3 pos, string text)
        {
            if (!fontCache.ContainsKey(text))
            {
                box.Materials[0].AlbedoMap = new Texture(DrawText(text, fnt, Color.FromArgb((int)(ForeColor.W * 255), (int)(ForeColor.X * 255), (int)(ForeColor.Y * 255), (int)(ForeColor.Z * 255)), Color.Transparent), false);
            }
            else
            {
                box.Materials[0].AlbedoMap = fontCache[text];
            }

            var tmp = context.FaceCulling;
            context.FaceCulling = CullMode.Off;
            var tmp2 = pos - context.Camera.Position;
            tmp2.Normalize();
            Vector3 rotation = Vector3.ToSpherical(tmp2);
            box.World = Matrix4.CreateRotationX(rotation.X + 3.14f / 2f) * Matrix4.CreateRotationY(rotation.Y + 3.14f / 2f) * Matrix4.CreateRotationZ(rotation.Z + 3.14f / 2f) * Matrix4.CreateTranslation(pos.X, pos.Y, pos.Z) * Matrix4.Scale(box.Materials[0].AlbedoMap.Size.X, box.Materials[0].AlbedoMap.Size.Y * (float)context.AspectRatio, 1);
            box.Draw(context);
            context.FaceCulling = tmp;
        }
    }
}
