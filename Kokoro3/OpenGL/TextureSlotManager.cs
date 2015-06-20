#if OPENGL
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public static class TextureSlotManager
    {
        internal static Dictionary<int, int> RealSlots = new Dictionary<int, int>();
        internal static Dictionary<int, Engine.TextureType> TextureTypes = new Dictionary<int, Engine.TextureType>();
        internal static Dictionary<int, int> PrevBoundSlots = new Dictionary<int, int>();

        static TextureSlotManager()
        {

        }

        public static void PlaceTextures(params Texture[] textures)
        {
            int[] IDs = new int[textures.Length];
            bool[] IDBound = new bool[textures.Length];
            int[] bindPoints = new int[textures.Length];

            for (int i = 0; i < textures.Length; i++)
            {
                IDs[i] = textures[i].ID;
                IDBound[i] = RealSlots.ContainsValue(IDs[i]);
                if (IDBound[i])
                {
                    bindPoints[i] = RealSlots.Single((a) => a.Value == IDs[i]).Key;
                }
            }

            for (int i = 0; i < textures.Length; i++)
            {
                //Place texture into empty slot
                if (!IDBound[i])
                {
                    int j = 0;
                    while (j < GraphicsDevice.SystemLimits[HardwareLimits.MaxTextureUnits])
                    {
                        if (!bindPoints.Contains(j))
                        {
                            textures[i].attachedSlot = j;   //NOTE possible source of bugs, what happens when two textures end up with the same bind point stored? This may lead to annoying texture management later
                            TextureTypes[j] = textures[i].TexType;
                            RealSlots[j] = IDs[i];  //Store the binding information
                            bindPoints[i] = j;  //Update the binding point data since the unit is now in use
                            break;
                        }
                        j++;
                    }
                    if (j >= GraphicsDevice.SystemLimits[HardwareLimits.MaxTextureUnits]) throw new Exception("Too many textures to be bound, too few available texture units");
                }
            }
        }

        public static void BindAll()
        {
            for (int i = 0; i < GraphicsDevice.SystemLimits[HardwareLimits.MaxTextureUnits]; i++)
            {
                if (PrevBoundSlots[i] != RealSlots[i])   //If there has been a change, apply the change to OpenGL
                {
                    //First update the binding info
                    PrevBoundSlots[i] = RealSlots[i];
                    GL.ActiveTexture(TextureUnit.Texture0 + i);
                    GL.BindTexture(EnumConverters.ETextureTarget(TextureTypes[i]), RealSlots[i]);   //The binding has been updated
                }
            }

        }
    }
}
#endif