using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL_GPULimitProber
{
    class Program
    {
        static Dictionary<string, string> results = new Dictionary<string,string>();

        static void Main(string[] args)
        {
            results = new Dictionary<string, string>();
            GameWindow win = new GameWindow(100, 100, OpenTK.Graphics.GraphicsMode.Default, "Title", GameWindowFlags.Default, DisplayDevice.Default, 4, 5, OpenTK.Graphics.GraphicsContextFlags.Debug);
            win.UpdateFrame += win_UpdateFrame;
            win.Run();
            foreach(KeyValuePair<string,string> s in results)
            {
                Console.WriteLine(s.Key + " : " + s.Value);
            }
            Console.ReadLine();
        }

        static void win_UpdateFrame(object sender, FrameEventArgs e)
        {
            //Read all the OpenGL status variables, write them to results and exit the window
            FetchString(StringName.Vendor);
            FetchString(StringName.Renderer);
            FetchString(StringName.ShadingLanguageVersion);
            FetchString(StringName.Version);
                Fetch(GetPName.MajorVersion);
                Fetch(GetPName.Max3DTextureSize);
                Fetch(GetPName.MaxArrayTextureLayers);
                Fetch(GetPName.MaxClipDistances);
                Fetch(GetPName.MaxColorAttachments);
                Fetch(GetPName.MaxColorTextureSamples);
                Fetch(GetPName.MaxCombinedFragmentUniformComponents);
                Fetch(GetPName.MaxCombinedGeometryUniformComponents);
                Fetch(GetPName.MaxCombinedImageUniforms);
                Fetch(GetPName.MaxCombinedTessControlUniformComponents);
                Fetch(GetPName.MaxCombinedTessEvaluationUniformComponents);
                Fetch(GetPName.MaxCombinedTextureImageUnits);
                Fetch(GetPName.MaxCombinedUniformBlocks);
                Fetch(GetPName.MaxCombinedVertexUniformComponents);
                Fetch(GetPName.MaxComputeImageUniforms);
                Fetch(GetPName.MaxCubeMapTextureSize);
                Fetch(GetPName.MaxDepthTextureSamples);
                Fetch(GetPName.MaxDrawBuffers);
                Fetch(GetPName.MaxDualSourceDrawBuffers);
                Fetch(GetPName.MaxElementsIndices);
                Fetch(GetPName.MaxElementsVertices);
                Fetch(GetPName.MaxFragmentImageUniforms);
                Fetch(GetPName.MaxFragmentInputComponents);
                Fetch(GetPName.MaxFragmentInterpolationOffset);
                Fetch(GetPName.MaxFragmentUniformBlocks);
                Fetch(GetPName.MaxFragmentUniformComponents);
                Fetch(GetPName.MaxFragmentUniformVectors);
                Fetch(GetPName.MaxGeometryImageUniforms);
                Fetch(GetPName.MaxGeometryInputComponents);
                Fetch(GetPName.MaxGeometryOutputComponents);
                Fetch(GetPName.MaxGeometryOutputVertices);
                Fetch(GetPName.MaxGeometryShaderInvocations);
                Fetch(GetPName.MaxGeometryTextureImageUnits);
                Fetch(GetPName.MaxGeometryTotalOutputComponents);
                Fetch(GetPName.MaxGeometryUniformBlocks);
                Fetch(GetPName.MaxGeometryUniformComponents);
                Fetch(GetPName.MaxGeometryVaryingComponents);
                Fetch(GetPName.MaxIntegerSamples);
                Fetch(GetPName.MaxPatchVertices);
                Fetch(GetPName.MaxProgramTexelOffset);
                Fetch(GetPName.MaxProgramTextureGatherOffset);
                Fetch(GetPName.MaxRectangleTextureSize);
                Fetch(GetPName.MaxRenderbufferSize);
                Fetch(GetPName.MaxSamples);
                Fetch(GetPName.MaxSubroutines);
                Fetch(GetPName.MaxSubroutineUniformLocations);
                Fetch(GetPName.MaxTessControlImageUniforms);
                Fetch(GetPName.MaxTessControlInputComponents);
                Fetch(GetPName.MaxTessControlOutputComponents);
                Fetch(GetPName.MaxTessControlTextureImageUnits);
                Fetch(GetPName.MaxTessControlTotalOutputComponents);
                Fetch(GetPName.MaxTessControlUniformBlocks);
                Fetch(GetPName.MaxTessControlUniformComponents);
                Fetch(GetPName.MaxTessEvaluationImageUniforms);
                Fetch(GetPName.MaxTessEvaluationInputComponents);
                Fetch(GetPName.MaxTessEvaluationOutputComponents);
                Fetch(GetPName.MaxTessEvaluationTextureImageUnits);
                Fetch(GetPName.MaxTessEvaluationUniformBlocks);
                Fetch(GetPName.MaxTessEvaluationUniformComponents);
                Fetch(GetPName.MaxTessGenLevel);
                Fetch(GetPName.MaxTessPatchComponents);
                Fetch(GetPName.MaxTextureBufferSize);
                Fetch(GetPName.MaxTextureCoords);
                Fetch(GetPName.MaxTextureImageUnits);
                Fetch(GetPName.MaxTextureLodBias);
                Fetch(GetPName.MaxTextureSize);
                Fetch(GetPName.MaxTextureUnits);
                Fetch(GetPName.MaxTransformFeedbackBuffers);
                Fetch(GetPName.MaxTransformFeedbackInterleavedComponents);
                Fetch(GetPName.MaxTransformFeedbackSeparateAttribs);
                Fetch(GetPName.MaxTransformFeedbackSeparateComponents);
                Fetch(GetPName.MaxUniformBlockSize);
                Fetch(GetPName.MaxUniformBufferBindings);
                Fetch(GetPName.MaxVaryingComponents);
                Fetch(GetPName.MaxVaryingFloats);
                Fetch(GetPName.MaxVaryingVectors);
                Fetch(GetPName.MaxVertexAttribs);
                Fetch(GetPName.MaxVertexImageUniforms);
                Fetch(GetPName.MaxVertexOutputComponents);
                Fetch(GetPName.MaxVertexStreams);
                Fetch(GetPName.MaxVertexTextureImageUnits);
                Fetch(GetPName.MaxVertexUniformBlocks);
                Fetch(GetPName.MaxVertexUniformComponents);
                Fetch(GetPName.MaxVertexUniformVectors);
                Fetch(GetPName.MaxVertexVaryingComponents);
                //Fetch(GetPName.MaxViewportDims);
                Fetch(GetPName.MaxViewports);

                ((GameWindow)sender).Exit();
        }

        static void Fetch(GetPName p)
        {
            if (results.ContainsKey(p.ToString())) results[p.ToString()] = GL.GetInteger(p).ToString();
            else results.Add(p.ToString(), GL.GetInteger(p).ToString() );
        }

        static void FetchString(StringName p)
        {
            results[p.ToString()] = GL.GetString(p);
        }
    }
}
