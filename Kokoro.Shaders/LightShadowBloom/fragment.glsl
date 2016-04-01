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
uniform sampler2D AvgColor;


vec3 Uncharted2Tonemap(vec3 x)
{
            float A = 0.15;
float B = 0.50;
float C = 0.10;
float D = 0.20;
float E = 0.02;
float F = 0.30;

            return ((x*(A*x+C*B)+D*E)/(x*(A*x+B)+D*F))-E/F;
}


void main(){
	color = texture2D(LitMap, UV) + texture2D(BloomMap, UV) + texture2D(DiffuseMap, UV) * texture2D(ShadowMap, UV).r;
	//color.rgb = (texture(DiffuseMap, UV).rgb + texture(LitMap, UV).rgb);
	color.a = 1;

	const float gamma = 2.2;
  
	vec3 col = texture2D(AvgColor, vec2(0)).rgb;
	float lum = (0.299*col.r + 0.587*col.g + 0.114*col.b);

	lum = max(0.4, lum);

    // Exposure tone mapping
    vec3 mapped = Uncharted2Tonemap(color.rgb * lum)/Uncharted2Tonemap(vec3(0.9));
    // Gamma correction 
    mapped = pow(mapped, vec3(1.0 / gamma));
  
    color = vec4(mapped, 1.0);
}