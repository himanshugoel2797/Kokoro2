﻿#version 430 core

in vec2 UV;

layout(location = 0) out vec4 reflection;

uniform sampler2D colorMap;
uniform sampler2D normData;
uniform sampler2D specularData;
uniform sampler2D worldData;
uniform sampler2D envMap;
uniform sampler2D depthMap;
uniform sampler2D bloomMap;
uniform vec3 EyePos;
uniform mat4 View;
uniform mat4 Projection;


vec3 decode (vec2 enc)
{
	vec2 ang = enc*2-1;
    vec2 scth;
    scth.x = sin(ang.x * 3.1415926536f);
	scth.y = cos(ang.x * 3.1415926536f);
    vec2 scphi = vec2(sqrt(1.0 - ang.y*ang.y), ang.y);
    return vec3(scth.y*scphi.x, scth.x*scphi.x, scphi.y);
}

float rand(vec4 seed4)
{
    float dot_product = dot(seed4, vec4(12.9898,78.233,45.164,94.673));
    return fract(sin(dot_product) * 43758.5453);
}

void main(){
    vec3 n = decode(texture2D(normData, UV).xy);
    n = normalize(n);
    vec3 worldCoord = texture2D(worldData, UV).rgb;
    vec3 v = EyePos - worldCoord;
    v = normalize(-v);
    float depth = texture2D(depthMap, UV).r;
    
	vec4 vis = vec4(0);
    vec3 refNorm = (Projection * View * vec4(reflect(v, n), 0)).rgb;
    refNorm = 0.5 * refNorm + 0.5;
	vec2 uv_c = refNorm.xy;
    vis = texture2D(colorMap, uv_c);// + texture2D(bloomMap, uv_c) * 0.25f;
    reflection = vis;
}




