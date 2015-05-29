#version 440 core


// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D ColorMap;

uniform float Height;
uniform float tileID;
uniform float maxGID;

void main(){

	// Output color = color of the texture at the specified UV
	//color = texture2D( ColorMap, UV );
	color.r = tileID/maxGID;
	color.g = Height;
	color.b = 0;
	color.a = 1;

	//if(tileID == 0)color.a = 0;
}