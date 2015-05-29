using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kokoro2.Math;

#if OPENGL
#if PC
using Kokoro2.OpenGL.PC;
#endif
#endif

namespace Kokoro2.Engine.Shaders
{
    public enum ShaderTypes
    {
        Vertex = 0, Fragment = 4, Geometry = 3, TessellationControl = 1, TessellationEval = 2, TessellationComb = 5, Compute = 6
    }

    /// <summary>
    /// A Shader Program Object
    /// </summary>
    public class Shader : ShaderLL
    {
        private static Dictionary<byte[], int> shaderDB = new Dictionary<byte[], int>();
        private static FNV1a fnv = new FNV1a();

        protected Shader(string shader, ShaderTypes type)
        {
            base.shaderType = type;
            if (type != ShaderTypes.TessellationComb)
            {
                byte[] hash = fnv.ComputeHash(Encoding.UTF8.GetBytes(shader));

                if (!shaderDB.ContainsKey(hash))
                {
                    base.aCreate(base.shaderType, shader);
                    base.CheckForErrors(shader, base.shaderType);

                    shaderDB.Add(hash, base.id);
                }
                else
                {
                    base.id = shaderDB[hash];
                }
                Kokoro2.Debug.ObjectAllocTracker.NewCreated(this, id, type.ToString() + " Shader");
            }
        }

#if DEBUG
        ~Shader()
        {
            Kokoro2.Debug.ObjectAllocTracker.ObjectDestroyed(this, id, base.shaderType.ToString() + " Shader");
        }
#endif

        internal ShaderTypes GetShaderType()
        {
            return pGetShaderType();
        }

        internal int GetID()
        {
            return pGetID();
        }
    }
}
