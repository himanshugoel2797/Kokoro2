#version 430 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 position;
layout(location = 1) in vec2 vertexUV;

uniform float ZNear;
uniform float ZFar;

// Output data ; will be interpolated for each fragment.
out vec2 UV;
out vec2 logBufDat;

void main(){

	float FCOEF = 2.0 / log2(ZFar + 1.0);
	logBufDat.x = 0.5f * FCOEF;

	gl_Position = vec4(position, 1);
	gl_Position.z = log2(max(ZNear, 1.0 + gl_Position.w)) * FCOEF - 1.0;
	logBufDat.y = 1.0 + gl_Position.w;
	
	// UV of the vertex. No special space for this one
	UV = (position.xy+vec2(1,1))/2.0;
}