#version 440 core


// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D ColorMap;
uniform sampler2D LightingMap;
uniform sampler2D NormalMap;

uniform vec2 TexSize;
uniform vec2 TexPos;
uniform vec2 SpriteSize;

void main(){

	vec2 nTexPos = TexPos;
	vec2 nSpriteSize = SpriteSize;
	
	nTexPos.y = TexSize.y - nTexPos.y;
	nTexPos.y -= SpriteSize.y;
	nTexPos /= TexSize;

	nSpriteSize /= TexSize;

	color = texture2D(ColorMap, (nTexPos + UV * nSpriteSize) );
}