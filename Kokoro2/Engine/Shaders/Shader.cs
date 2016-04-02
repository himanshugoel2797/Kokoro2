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
        private static Dictionary<byte[], ulong> shaderDB = new Dictionary<byte[], ulong>();
        private static FNV1a fnv = new FNV1a();

        protected Shader(string shader, ShaderTypes type, GraphicsContext c) : base(c)
        {
            base.shaderType = type;
            if (type != ShaderTypes.TessellationComb)
            {
                byte[] hash = fnv.ComputeHash(Encoding.UTF8.GetBytes(shader));

                if (!shaderDB.ContainsKey(hash))
                {
                    base.aCreate(base.shaderType, shader);
                    base.CheckForErrors(shader, base.shaderType);

                    shaderDB.Add(hash, ID);
                    Kokoro2.Engine.ObjectAllocTracker.NewCreated(this);
                }
                else
                {
                    ID = shaderDB[hash];
                }
            }
        }

        #region Shader library loader
        static Shader()
        {
            LoadCalls = new List<Func<string, string>>();
            StringToLoader = new Dictionary<string, int>();

            //Load the default shader library at initialization
            LoadLibrary(Kokoro2.Shaders.ShaderLibrary.LoadFile, Kokoro2.Shaders.ShaderLibrary.LoadShaders());
        }
        static List<Func<string, string>> LoadCalls;
        static Dictionary<string, int> StringToLoader;

        public static void LoadLibrary(Func<string, string> loadCall, string[] options)
        {
            LoadCalls.Add(loadCall);
            int index = LoadCalls.Count - 1;

            for (int i = 0; i < options.Length; i++) StringToLoader.Add(options[i], index);
        }

        protected static string GetFile(string file)
        {
            if (StringToLoader.ContainsKey(file))
            {
                return LoadCalls[StringToLoader[file]](file);
            }
            else
            {
                throw new System.IO.FileNotFoundException($"The shader '{file}' could not be found");
            }
        }
        #endregion
#if DEBUG
        ~Shader()
        {
            Kokoro2.Engine.ObjectAllocTracker.ObjectDestroyed(this);
        }
#endif

        internal ShaderTypes GetShaderType()
        {
            return pGetShaderType();
        }
    }
}
