#if OPENGL
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public class ShaderProgram : IDisposable
    {
        internal int ID;
        List<Shader> shaders;

        public ShaderProgram()
        {
            ID = GL.CreateProgram();
            shaders = new List<Shader>();

            ResourceCounts = new Dictionary<ProgramInterface, int>();
            Mappings = new Dictionary<string, int>();
        }

        #region Shader Compilation
        public void Attach(Shader s)
        {
            GL.AttachShader(ID, s.ID);
            shaders.Add(s);
        }

        public void Link()
        {
            GL.LinkProgram(ID);

            int result = 0;
            GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out result);
            if (result != 1)
            {
                throw new Exception("Program Link Failed!", new Exception(GL.GetProgramInfoLog(ID)));  //If compile failed, throw an exception
            }
        }

        public void Detach(Shader s)
        {
            GL.DetachShader(ID, s.ID);
        }

        public void DetachAll()
        {
            for (int i = 0; i < shaders.Count; i++)
            {
                Detach(shaders[i]);
            }
        }
        #endregion

        #region Program Binary
        public void CacheBinary(string file)
        {
            int result = 0;
            GL.GetProgram(ID, GetProgramParameterName.ProgramBinaryRetrievableHint, out result);

            if (result != 0)
            {
                //TODO submit bug fix in OpenTK and update binaries used so this can actually be used
                GL.GetProgram(ID, (GetProgramParameterName)All.ProgramBinaryLength, out result);

                int outLength = 0;
                BinaryFormat bFormat;
                byte[] outData = new byte[result];

                //remove NuGet package dependency and replace it with a custom OpenTK reference 

                GL.GetProgramBinary(ID, result, out outLength, out bFormat, outData);

                var bFormatINT = BitConverter.GetBytes((int)bFormat);
                var outLengthINT = BitConverter.GetBytes(outLength);

                if (outLength > 0)
                {
                    using (System.IO.FileStream str = System.IO.File.Open(file, System.IO.FileMode.Create))
                    {
                        str.Write(outData, 0, outData.Length);
                        str.Write(bFormatINT, 0, 4);
                        str.Write(outLengthINT, 0, 4);
                    }
                }
                else throw new Exception("Failed to retrieve program binary");
            }
            else throw new Exception("Cannot retrieve binary for this shader");
        }

        public void LoadFromBinary(string binaryPath)
        {
            byte[] binary = System.IO.File.ReadAllBytes(binaryPath);

            int bFormat = BitConverter.ToInt32(binary, binary.Length - 8);
            int len = BitConverter.ToInt32(binary, binary.Length - 4);

            GL.ProgramBinary(ID, (BinaryFormat)bFormat, binary, len);
        }
        #endregion

        #region Introspection API
        Dictionary<ProgramInterface, int> ResourceCounts;
        Dictionary<string, int> Mappings;
        public void CacheUniforms()
        {
            GetInfo(ProgramInterface.AtomicCounterBuffer);
            GetInfo(ProgramInterface.BufferVariable);
            GetInfo(ProgramInterface.ComputeSubroutine);
            GetInfo(ProgramInterface.ComputeSubroutineUniform);
            GetInfo(ProgramInterface.FragmentSubroutine);
            GetInfo(ProgramInterface.FragmentSubroutineUniform);
            GetInfo(ProgramInterface.GeometrySubroutine);
            GetInfo(ProgramInterface.GeometrySubroutineUniform);
            GetInfo(ProgramInterface.ProgramInput);
            GetInfo(ProgramInterface.ProgramOutput);
            GetInfo(ProgramInterface.ShaderStorageBlock);
            GetInfo(ProgramInterface.TessControlSubroutine);
            GetInfo(ProgramInterface.TessControlSubroutineUniform);
            GetInfo(ProgramInterface.TessEvaluationSubroutine);
            GetInfo(ProgramInterface.TessEvaluationSubroutineUniform);
            GetInfo(ProgramInterface.TransformFeedbackBuffer);
            GetInfo(ProgramInterface.TransformFeedbackVarying);
            GetInfo(ProgramInterface.Uniform);
            GetInfo(ProgramInterface.UniformBlock);
            GetInfo(ProgramInterface.VertexSubroutine);
            GetInfo(ProgramInterface.VertexSubroutineUniform);
        }

        private void GetInfo(ProgramInterface i)
        {
            int val = 0;
            GL.GetProgramInterface(ID, i, ProgramInterfaceParameter.ActiveResources, out val);
            ResourceCounts[i] = val;

            ProgramProperty[] properties = new ProgramProperty[] { ProgramProperty.BlockIndex, ProgramProperty.NameLength, ProgramProperty.Type, ProgramProperty.Location };

            //NOTE We might want to use the Type information in some way?

            int dummy = 0;

            for (int index = 0; index < val; index++)
            {
                int[] results = new int[properties.Length];
                GL.GetProgramResource(ID, i, index, properties.Length, properties, results.Length, out dummy, results);

                StringBuilder name = new StringBuilder();
                GL.GetProgramResourceName(ID, i, index, results[1], out dummy, name);

                if (i != ProgramInterface.UniformBlock) Mappings[name.ToString()] = results[3]; //Add the mapping data to the Mappings collection
                else Mappings[name.ToString()] = results[0];    //If dealing with uniform blocks, we care about the blockindex, not the location
            }
        }

        #endregion

        #region Binding Uniforms
        public void BindUniormBufferObject(string name, UniformBuffer buffer, int bindingPoint)
        {
            GL.UniformBlockBinding(ID, Mappings[name], bindingPoint);
        }
        #endregion

        #region Memory Management
        public void Dispose()
        {
            GL.DeleteProgram(ID);
            ID = 0;
            GC.SuppressFinalize(this);
        }

        ~ShaderProgram()
        {
            System.Diagnostics.Debug.WriteLine($"[WARN] The ShaderProgram object {ID} was automatically disposed");
            Dispose();
        }
        #endregion
    }
}
#endif