#version 430 core

// Interpolated values from the vertex shaders
in vec2 UV;


// Ouput data
layout(location = 0) out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D LitMap;
uniform sampler2D BloomMap;
uniform sampler2D ShadowMap;
uniform sampler2D DiffuseMap;

void main(){
	color = texture2D(LitMap, UV) + texture2D(BloomMap, UV) + texture2D(DiffuseMap, UV) * texture2D(ShadowMap, UV).r;
	//color.rgb = (texture(DiffuseMap, UV).rgb + texture(LitMap, UV).rgb);
	color.a = 1;
}