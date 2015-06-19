using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenGL
{
    public enum HardwareLimits
    {
        //Contains enums for the resource limits imposed by the harwdare
        MajorVersion = GetPName.MajorVersion,
        MinorVersion = GetPName.MinorVersion,
        Max3DTextureSize = GetPName.Max3DTextureSize,
        MaxArrayTextureLayers = GetPName.MaxArrayTextureLayers,
        MaxColorAttachments = GetPName.MaxColorAttachments,
        MaxIndices = GetPName.MaxElementsIndices,
        MaxVertices = GetPName.MaxElementsVertices,
        MaxTextureUnits = GetPName.MaxTextureUnits,
        MaxTextureSize = GetPName.MaxTextureSize,
        MaxViewportSize = GetPName.MaxViewportDims,
        MaxViewports = GetPName.MaxViewports
    }
}
