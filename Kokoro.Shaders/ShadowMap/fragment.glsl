#version 430 core

in vec3 norm;
in vec3 pos;
in vec2 UV;

layout(location = 0) out vec4 colMap;
layout(location = 1) out vec4 posMap;

uniform sampler2D AlbedoMap;

void main()
{
	colMap = texture2D(AlbedoMap, UV);
	posMap = vec4(pos, gl_FragCoord.z * gl_FragCoord.z);
}