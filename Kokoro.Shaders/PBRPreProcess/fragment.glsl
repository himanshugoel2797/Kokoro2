#version 430 core

// Interpolated values from the vertex shaders
in vec2 UV;
// Ouput data
layout(location = 0) out vec4 map;

//n = normal
//m = half
//a = roughness factor
float ggx(float nDotm, float a)
{
    float top = a * a;
    float pA = nDotm;
    pA = pA * pA * (top - 1) + 1;
    pA = pA * pA * 3.14159;
    return top/pA;
}

//f0 = fresnel factor
//h = half vector
//v = view vector
float fresnel_schlick(float f0,	vec3 h, vec3 v, float vDotH)
{
    return f0 + (1 - f0) * pow((1 - vDotH), 5);
}

float geometric_schlick(vec3 n, vec3 v, float k, float nDotV)
{
	return nDotV / (nDotV * (1 - k) + k);
}

void main(){
    //ggx: x = [0, 1], y = [0, 1]
	map.r = ggx(UV.x, UV.y);
	map.g = fresnel_schlick(UV.x, vec3(0), vec3(0), UV.y);
	map.b = geometric_schlick(vec3(0), vec3(0), UV.x, UV.y);
	map.a = 1;
}



