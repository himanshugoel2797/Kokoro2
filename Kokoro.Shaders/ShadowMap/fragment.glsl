#version 430 core

in vec3 norm;
in vec3 pos;

layout(location = 0) out vec4 normMap;
layout(location = 1) out vec4 posMap;

void main()
{
	normMap.rgb = 0.5 * normalize(norm) + 0.5;
	posMap.rgb = pos;

	normMap.a = posMap.a = 1;
}