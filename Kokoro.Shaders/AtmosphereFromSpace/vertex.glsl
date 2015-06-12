#version 430 core

layout(location = 0) in vec3 vertex;
layout(location = 1) in vec3 normal;

uniform mat4 World;
uniform mat4 View;
uniform mat4 Projection;

uniform float ZFar;
uniform float ZNear;

uniform vec3 EyePos;		// The camera's current position
uniform vec3 EyeDir;	
uniform vec3 SunPos;		// The direction vector to the light source	

out vec2 UV;	
out vec3 color; 
out vec3 secondaryColor;	

out float depth;
out vec3 worldXY;
smooth out vec3 normPos;
out vec2 logBufDat;	

void main(void) 
{
	// Get the ray from the camera to the vertex and its length (which is the far point of the ray passing through the atmosphere) 
	mat4 MVP = Projection * View * World;

	vec3 halfVector = normalize(normalize(EyeDir) + normalize(SunPos));

	//color = 1.333 + (1 - 1.333) * pow((1 + dot(halfVector, normalize(EyeDir))), 5) * vec3(0.3, 0.5, 0.8);
	color = vec3(0.3, 0.5, 0.8) * (1.0 - dot(normalize(normal), normalize(EyePos - vertex)));

	gl_Position = MVP * vec4(vertex,1.0);

	float FCOEF = 2.0 / log2(ZFar + 1.0);
	logBufDat.x = 0.5f * FCOEF;

	depth = (gl_Position.z * gl_Position.w - ZNear)/(ZFar - ZNear);

	gl_Position.z = log2(max(ZNear, 1.0 + gl_Position.w)) * FCOEF - 1.0;
	logBufDat.y = 1.0 + gl_Position.w;

	normPos = (World * vec4(normal, 0)).xyz;
	worldXY = (World * vec4(vertex, 1)).xyz;
}