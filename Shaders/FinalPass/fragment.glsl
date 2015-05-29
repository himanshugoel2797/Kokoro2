

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D diffuse;

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

	float exposure = 22;

	// Output color = color of the texture at the specified UV
	color = texture2D( diffuse, UV );
	color.rgb = Uncharted2Tonemap(color.rgb * exposure);
	color.a = 1;
}