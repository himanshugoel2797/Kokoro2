#version 430 core

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 color;

in float flogz;
uniform float Fcoef;

// Values that stay constant for the whole mesh.
uniform sampler2D AlbedoMap;

void main(){
	color.rg = UV;
	color.ba = vec2(1);
	gl_FragDepth = Fcoef * 0.5 * log2(flogz);
}