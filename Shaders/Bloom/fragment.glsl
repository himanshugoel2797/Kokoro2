

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D diffuse;
uniform vec4 col;
uniform float mode; 


void main(){

	// Output color = color of the texture at the specified UV
	if(mode == 1)color = col;
	else color = texture2D( diffuse, UV );
	color.a = 0.4;
	if( all(equal(color, vec4(0,0,0,color.a))) )color.a = 0;
}