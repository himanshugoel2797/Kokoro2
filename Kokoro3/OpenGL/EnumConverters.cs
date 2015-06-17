﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if PC      //Choose the right library to pick the enums from, since this only counts if using OpenGL, an OpenGL define check isn't necessary
using OpenTK.Graphics.OpenGL4;
#endif

namespace Kokoro3.OpenGL
{
    public class EnumConverters
    {
        public static PixelFormat EPixelFormat(Engine.PixelFormat pf)
        {
            if (pf == Engine.PixelFormat.BGRA) return PixelFormat.Bgra;
            else if (pf == Engine.PixelFormat.RGBA) return PixelFormat.Rgba;
            else if (pf == Engine.PixelFormat.Depth) return PixelFormat.DepthComponent;

            return PixelFormat.Rgba;    //This will never be called, it avoids compiler errors
        }

        public static PixelInternalFormat EPixelComponentType(Engine.PixelComponentType pct)
        {
            if (pct == Engine.PixelComponentType.D24S8) return PixelInternalFormat.Depth24Stencil8;
            else if (pct == Engine.PixelComponentType.D32) return PixelInternalFormat.DepthComponent32;
            else if (pct == Engine.PixelComponentType.D32S8) return PixelInternalFormat.Depth32fStencil8;
            else if (pct == Engine.PixelComponentType.RGBA16f) return PixelInternalFormat.Rgba16f;
            else if (pct == Engine.PixelComponentType.RGBA32f) return PixelInternalFormat.Rgba32f;
            else if (pct == Engine.PixelComponentType.RGBA8) return PixelInternalFormat.Rgba8;
            else if (pct == Engine.PixelComponentType.SRGBA8) return PixelInternalFormat.Srgb8Alpha8;

            return PixelInternalFormat.Rgba8;   //This will never be called, it avoids compiler errors
        }

        public static SizedInternalFormat ESizedInternalFormat(Engine.PixelComponentType pct)
        {
            if (pct == Engine.PixelComponentType.RGBA16f) return SizedInternalFormat.Rgba16f;
            else if (pct == Engine.PixelComponentType.RGBA32f) return SizedInternalFormat.Rgba32f;
            else if (pct == Engine.PixelComponentType.RGBA8) return SizedInternalFormat.Rgba8;

            return SizedInternalFormat.Rgba16f;
        }

        public static TextureTarget ETextureTarget(Engine.TextureType tType)
        {
            if (tType == Engine.TextureType.Texture1D) return TextureTarget.Texture1D;
            else if (tType == Engine.TextureType.Texture2D) return TextureTarget.Texture2D;
            else if (tType == Engine.TextureType.Texture2DArray) return TextureTarget.Texture2DArray;
            else if (tType == Engine.TextureType.Texture3D) return TextureTarget.Texture3D;

            return TextureTarget.Texture2D;
        }

        public static PixelType EPixelType(Engine.PixelType type)
        {
            if (type == Engine.PixelType.UByte332) return OpenTK.Graphics.OpenGL4.PixelType.UnsignedByte332;
            else if (type == Engine.PixelType.UInt1010102) return OpenTK.Graphics.OpenGL4.PixelType.UnsignedInt1010102;
            else if (type == Engine.PixelType.UInt248) return OpenTK.Graphics.OpenGL4.PixelType.UnsignedInt248;
            else if (type == Engine.PixelType.UInt8888) return OpenTK.Graphics.OpenGL4.PixelType.UnsignedInt8888;
            else if (type == Engine.PixelType.UShort4444) return OpenTK.Graphics.OpenGL4.PixelType.UnsignedShort4444;
            else if (type == Engine.PixelType.UShort5551) return OpenTK.Graphics.OpenGL4.PixelType.UnsignedShort5551;
            else if (type == Engine.PixelType.Float) return PixelType.Float;
            else if (type == Engine.PixelType.HalfFloat) return PixelType.HalfFloat;

            return OpenTK.Graphics.OpenGL4.PixelType.UnsignedInt8888;   //This will never be called, it avoids compiler errors
        }

        public static FramebufferAttachment EFrameBufferAttachment(Engine.FrameBufferAttachments attachment)
        {
            return (FramebufferAttachment)Enum.Parse(typeof(FramebufferAttachment), attachment.ToString());
        }

        public static DrawBuffersEnum EDrawBufferAttachment(Engine.FrameBufferAttachments attachment)
        {
            return (DrawBuffersEnum)Enum.Parse(typeof(DrawBuffersEnum), attachment.ToString());
        }

        public static CullFaceMode ECullMode(Engine.CullMode cullMode)
        {
            if (cullMode == Engine.CullMode.Back) return CullFaceMode.Back;
            else if (cullMode == Engine.CullMode.Front) return CullFaceMode.Front;

            return 0;
        }

        public static ShaderType EShaderTypes(Engine.ShaderTypes type)
        {
            if (type == Engine.ShaderTypes.Fragment) return ShaderType.FragmentShader;
            else if (type == Engine.ShaderTypes.Geometry) return ShaderType.GeometryShader;
            else if (type == Engine.ShaderTypes.TessellationControl) return ShaderType.TessControlShader;
            else if (type == Engine.ShaderTypes.TessellationEval) return ShaderType.TessEvaluationShader;
            else if (type == Engine.ShaderTypes.Vertex) return ShaderType.VertexShader;
            else if (type == Engine.ShaderTypes.Compute) return ShaderType.ComputeShader;

            return ShaderType.ComputeShader;
        }

        public static BlendingFactorDest EBlendFuncDST(Engine.BlendingFactor factor)
        {
            return (BlendingFactorDest)Enum.Parse(typeof(BlendingFactorDest), factor.ToString());
        }

        public static BlendingFactorSrc EBlendFuncSRC(Engine.BlendingFactor factor)
        {
            return (BlendingFactorSrc)Enum.Parse(typeof(BlendingFactorSrc), factor.ToString());
        }
        /*
        public static Engine.Input.Key OKey(OpenTK.Input.Key k)
        {
            return (Engine.Input.Key)Enum.Parse(typeof(Engine.Input.Key), k.ToString());
        }

        public static OpenTK.Input.Key EKey(Engine.Input.Key k)
        {
            return (OpenTK.Input.Key)Enum.Parse(typeof(OpenTK.Input.Key), k.ToString());
        }
        */

        public static OpenTK.Graphics.OpenGL4.All EDrawMode(Engine.DrawMode mode)
        {
            return (OpenTK.Graphics.OpenGL4.All)Enum.Parse(typeof(OpenTK.Graphics.OpenGL4.All), mode.ToString());
        }

        public static BufferTarget EBufferTarget(Engine.BufferUse use)
        {
            if (use == Engine.BufferUse.Array) return BufferTarget.ArrayBuffer;
            else if (use == Engine.BufferUse.Index) return BufferTarget.ElementArrayBuffer;
            else if (use == Engine.BufferUse.ShaderStorage) return BufferTarget.ShaderStorageBuffer;
            else if (use == Engine.BufferUse.Uniform) return BufferTarget.UniformBuffer;
            else if (use == Engine.BufferUse.Indirect) return BufferTarget.DrawIndirectBuffer;
            else if (use == Engine.BufferUse.Pixel) return BufferTarget.PixelUnpackBuffer;

            return BufferTarget.ArrayBuffer;    //Flow should never reach this
        }

        public static int ETextureFilter(Engine.TextureFilter filter)
        {
            if (filter == Engine.TextureFilter.Linear) return (int)All.Linear;
            else if (filter == Engine.TextureFilter.Nearest) return (int)All.Nearest;

            return (int)All.Linear;
        }

        public static int ETextureWrapMode(Engine.TextureWrapMode wrap)
        {
            if (wrap == Engine.TextureWrapMode.Clamp) return (int)All.ClampToBorder;
            else if (wrap == Engine.TextureWrapMode.Repeat) return (int)All.Repeat;

            return (int)All.Repeat;
        }

    }
}
