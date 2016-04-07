﻿#version 430 core

// Interpolated values from the vertex shaders
in vec2 UV;
in vec4 shadowCoord;
in vec3 worldCoord;
in vec3 norm;
// Ouput data
layout(location = 0) out vec4 worldPos;
layout(location = 1) out vec4 normDat;
layout(location = 2) out vec4 color;
// Values that stay constant for the whole mesh.
uniform sampler2D AlbedoMap;
uniform sampler2D SpecularMap;
uniform sampler2D GlossinessMap;
uniform sampler2D ShadowMap;
uniform sampler2D ReflectivePosMap;
uniform vec3 EyePos;

uniform mat4 View;

in float flogz;
uniform float Fcoef;
vec2 encode (vec3 n)
{
    return (vec2(atan(n.y, n.x)/3.1415926536, n.z)+1.0)*0.5;
}


void main(){
    vec3 shad = shadowCoord.xyz / shadowCoord.w;
    shad = 0.5 * shad + 0.5;
    float s_moment = texture2D(ReflectivePosMap, shad.xy).a;
    float f_moment = texture2D(ShadowMap, shad.xy).r;
    float vis = s_moment / (s_moment + (gl_FragCoord.z - f_moment) * (gl_FragCoord.z - f_moment));
    vis = pow(vis, 3);
    vec3 v = normalize(worldCoord - EyePos);
    normDat.rg = encode(normalize(norm));
    normDat.b = texture2D(SpecularMap, UV).r;
	normDat.a = texture2D(GlossinessMap, UV).g;
    worldPos.rgb = worldCoord;
    worldPos.a = vis;
    color = texture2D(AlbedoMap, UV);
    gl_FragDepth = Fcoef * 0.5 * log2(flogz);
}



