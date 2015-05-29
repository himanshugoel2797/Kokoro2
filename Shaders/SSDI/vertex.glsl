

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 vertexPosition_modelspace;
layout(location = 1) in vec2 vertexUV;
layout(location = 2) in vec3 normal;

// Output data ; will be interpolated for each fragment.
out vec2 UV;
out vec3 cameraPosition;

// Values that stay constant for the whole mesh.
uniform mat4 Model;
uniform mat4 View;
uniform mat4 Proj;

uniform vec2 actualSize;
uniform vec2 viewSize;

void main(){


	cameraPosition = -transpose(mat3(View)) * View[3].xyz;

	// Output position of the vertex, in clip space : MVP * position
	gl_Position = vec4(vertexPosition_modelspace,1);
	
	// UV of the vertex. No special space for this one.
	UV = vertexUV;	//Scale the UV to the texture size
}