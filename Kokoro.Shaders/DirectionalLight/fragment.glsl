﻿#version 430 core

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 lit;
layout(location = 1) out vec4 bloom;

// Values that stay constant for the whole mesh.
uniform sampler2D colorMap;
uniform sampler2D normData;
uniform sampler2D specularData;
uniform sampler2D worldData;
uniform samplerCube envMap;

uniform vec3 lDir;
uniform vec4 lColor;
uniform vec3 eyePos;

float rand(vec4 seed4)
{
    float dot_product = dot(seed4, vec4(12.9898,78.233,45.164,94.673));
    return fract(sin(dot_product) * 43758.5453);
}

//n = normal
//m = half
//a = roughness factor
float ggx(vec3 n, vec3 m, float a)
{
	float top = a * a;

	float pA = dot(n, m);
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

float kelemen(vec3 l, vec3 v, vec3 h, vec3 n, float nDotL, float nDotV, float vDotH)
{
	return (nDotL * nDotV)/(vDotH * vDotH);
}

vec4 cooktorr(vec3 n, vec3 v, vec3 l, float f0, vec4 spec, vec4 dif)
{
	vec3 m = normalize(l + v);

	float nDotV = max(0.0, dot(n, v));
	float nDotL = max(0.0, dot(n, l));
	float vDotH = max(0.0, dot(v, m));

	float roughness = ggx(n, m, spec.a * spec.a);
	float fresnel = fresnel_schlick(f0, m, v, vDotH);
	float geometric = kelemen(l, v, m, n, nDotL, nDotV, vDotH);

	float rs = fresnel * roughness * geometric/(nDotV * nDotL);

	rs = max(0.0, rs);

	return nDotL * (spec * rs + dif);
}

void main(){
    
	vec3 n = 2.0 * texture2D(normData, UV).xyz - 1.0;
	n = normalize(n);

	vec4 dif = texture2D(colorMap, UV);
	vec4 spec = texture2D(specularData, UV);
	vec3 worldCoord = texture2D(worldData, UV).rgb;

	float f0 = 0.5f;

	vec3 l = normalize(-lDir);
	vec3 v = eyePos - worldCoord;
	v = normalize(v);

	lit = lColor * cooktorr(n, v, l, f0, spec, dif);
	//lit.rgb = v;
	const vec3 fac = vec3(0.299, 0.587, 0.114);
	float lum = dot(fac, lit.rgb);
	bloom = mix(0.0, 1.0, step(0.8, lum)) * lit;

	lit.a = 1;
	bloom.a = 1;
}
