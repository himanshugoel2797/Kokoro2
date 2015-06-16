using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.Engine
{
    /// <summary>
    /// The Face Culling Modes
    /// </summary>
    public enum CullMode { Off, Back, Front }

    /// <summary>
    /// The Blending Factors
    /// </summary>
    public enum BlendingFactor
    {
        Zero = 0,
        One,
        SrcAlpha,
        OneMinusSrcAlpha,
        DstAlpha,
        OneMinusDstAlpha,
        DstColor,
        OneMinusDstColor,
        SrcAlphaSaturate,
        ConstantColorExt,
        ConstantColor,
        OneMinusConstantColorExt,
        OneMinusConstantColor,
        ConstantAlphaExt,
        ConstantAlpha,
        OneMinusConstantAlpha,
        OneMinusConstantAlphaExt,
        Src1Alpha,
        Src1Color,
        OneMinusSrc1Color,
        OneMinusSrc1Alpha
    }

    /// <summary>
    /// The Blend Function
    /// </summary>
    public struct BlendFunc
    {
        public BlendingFactor Src;
        public BlendingFactor Dst;
    }

    public enum DrawMode
    {
        Triangles,
        TriangleStrip,
        Lines,
        LineStrip,
        Points,
        Patches
    }

    public enum BufferUse
    {
        Array, Index, Uniform, ShaderStorage, Indirect, Pixel
    }

    public enum PixelFormat
    {
        RGBA, BGRA, Depth
    }

    public enum PixelComponentType
    {
        RGBA8, RGBA16f, RGBA32f, D24S8, D32, D32S8, SRGBA8
    }

    public enum PixelType
    {
        UByte332, UInt1010102, UInt248, UInt8888, UShort4444, UShort5551, Float, HalfFloat
    }

    public enum ShaderTypes
    {
        Vertex = 0, Fragment = 4, Geometry = 3, TessellationControl = 1, TessellationEval = 2, TessellationComb = 5, Compute = 6
    }

    public enum TextureWrapMode
    {
        Repeat, Clamp
    }

    public enum TextureType
    {
        Texture1D, Texture2D, Texture2DArray, Texture3D
    }

    public enum TextureFilter
    {
        Linear, Nearest
    }

    /// <summary>
    /// The available FrameBuffer attachments
    /// </summary>
    public enum FrameBufferAttachments
    {
        ColorAttachment0,
        ColorAttachment1,
        ColorAttachment2,
        ColorAttachment3,
        ColorAttachment4,
        ColorAttachment5,
        ColorAttachment6,
        ColorAttachment7,
        ColorAttachment8,
        ColorAttachment9,
        ColorAttachment10,
        ColorAttachment11,
        ColorAttachment12,
        ColorAttachment13,
        ColorAttachment14,
        ColorAttachment15,
        DepthAttachment,
        DepthStencilAttachment,
        StencilAttachment
    }



    /// <summary>
    /// Describes how often the object will be updated
    /// </summary>
    public enum UpdateMode
    {
        /// <summary>
        /// Never
        /// </summary>
        Static,
        /// <summary>
        /// Every Frame
        /// </summary>
        Dynamic
    }

    /// <summary>
    /// Describes all possible objects
    /// </summary>
    public enum ObjectType
    {
        /// <summary>
        /// A Particle System
        /// </summary>
        ParticleSystem,
        /// <summary>
        /// A non-renderable Physics object
        /// </summary>
        PhysicsObject,
        /// <summary>
        /// A renderable mesh, optionally with collision data
        /// </summary>
        Mesh,
        /// <summary>
        /// A light source
        /// </summary>
        LightSource,
        /// <summary>
        /// A sound source
        /// </summary>
        SoundSource
    }
}
