using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
    public class EngineObjectManager
    {
        #region Graphics Context Management
        private static ulong ctxID = 0;
        internal static ulong RegisterContext()
        {
            return ctxID++;
        }

        #endregion

        private ulong curID = 0;    //Used to obtain a new unique ID each time
        private Dictionary<ulong, int> LLMap;   //Used to map game engine IDs to low level IDs
        private int Default2DTexID;  //Default 2D Texture, represents an in-engine ID of 0
        private int DefaultFrameBufferID;     //Default Framebuffer, represents an in-engine ID of 0

        public EngineObjectManager()
        {
            LLMap = new Dictionary<ulong, int>();
        }

        public ulong RegisterObject(int id)
        {
            curID++;
            LLMap[curID] = id;
            return curID;
        }

        public void UnregisterObject(ulong id)
        {
            LLMap.Remove(id);
        }

        public int GetDefaultID<T>()
        {
            var typeT = typeof(T);

            if (typeT == typeof(Texture)) return Default2DTexID;
            else if (typeT == typeof(FrameBuffer)) return DefaultFrameBufferID;

            return 0;
        }

        public int this[ulong id, Type t]
        {
            get
            {
                return LLMap[id];
            }
        }
    }
}
