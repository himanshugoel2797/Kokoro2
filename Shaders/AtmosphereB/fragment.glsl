

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D diffuse;

void main(){

	// Output color = color of the texture at the specified UV
	color = texture2D( diffuse, UV );
	color.a = 1;
}