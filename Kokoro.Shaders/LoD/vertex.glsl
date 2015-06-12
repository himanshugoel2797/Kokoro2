#version 430 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 position;
layout(location = 2) in vec2 vertexUV;
layout(location = 1) in vec3 normal;

// Output data
out vec2 UV;
out vec3 worldXY;
smooth out vec3 normPos;

void main()
{
	normPos = normal;
	worldXY = position;
	UV = vertexUV;
}