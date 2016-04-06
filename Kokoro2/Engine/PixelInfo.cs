using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine
{
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
        UByte332, UInt1010102, UInt248, UInt8888, UShort4444, UShort5551, Float, HalfFloat, UnsignedByte
    }

    public enum DepthFunc
    {
        Always, Equal, GEqual, LEqual, Never, NotEqual
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

    public enum TextureTarget
    {
        Texture2D = 1,
        TextureCubeMap,
        TextureBuffer,
        TextureCubeMapNegativeX,
        TextureCubeMapPositiveX,
        TextureCubeMapNegativeY,
        TextureCubeMapPositiveY,
        TextureCubeMapNegativeZ,
        TextureCubeMapPositiveZ,

    }
}
