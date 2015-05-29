

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 vertexPosition_modelspace;
layout(location = 1) in vec2 vertexUV;
layout(location = 2) in vec3 normal;

// Output data ; will be interpolated for each fragment.
out vec2 UV;

// Values that stay constant for the whole mesh.
uniform mat4 Model;
uniform mat4 View;
uniform mat4 Proj;

uniform sampler2D heightMap;
uniform float height;

void main(){

	// Output position of the vertex, in clip space : MVP * position
	mat4 MVP = Proj * View * Model;
	gl_Position =  MVP * vec4(vertexPosition_modelspace + (normal * texture2DLod(heightMap, vertexUV, 0).r * height),1);
	
	// UV of the vertex. No special space for this one.
	UV = vertexUV;
}