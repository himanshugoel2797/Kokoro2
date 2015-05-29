#version 440 core


// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D ColorMap;
uniform sampler2D LightingMap;
uniform sampler2D NormalMap;


void main(){
	color = texture2D(ColorMap, UV);
}