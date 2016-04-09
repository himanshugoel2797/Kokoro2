#version 430 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 position;
layout(location = 1) in vec2 vertexUV;

uniform float ZNear;
uniform float ZFar;
uniform sampler2D PosData;
uniform vec3 Source;

uniform mat4 View;
uniform mat4 Projection;

// Output data ; will be interpolated for each fragment.
out vec2 UV;

void main(){
	gl_Position = Projection * View * vec4(Source + texelFetch(PosData, ivec2(position.xy), 0).xyz * 50, 1);
	gl_PointSize = 1;
	// UV of the vertex. No special space for this one
	UV = (position.xy+vec2(1,1))/2.0;
}