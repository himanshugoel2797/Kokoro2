#version 430 core

// Interpolated values from the vertex shaders
in vec2 UV;
// Ouput data
layout(location = 0) out vec4 lit;
layout(location = 1) out vec4 bloom;
// Values that stay constant for the whole mesh.
uniform sampler2D colorMap;
uniform sampler2D normData;
uniform sampler2D worldData;
uniform sampler2D ssrMap;
uniform sampler2D preCalc;

uniform vec3 lDir;
uniform vec3 lColor;
uniform vec3 EyePos;


float rand(vec4 seed4)
{
    float dot_product = dot(seed4, vec4(12.9898,78.233,45.164,94.673));
    return fract(sin(dot_product) * 43758.5453);
}

vec3 decode (vec2 enc)
{
	vec2 ang = enc*2-1;
    vec2 scth;
    scth.x = sin(ang.x * 3.1415926536f);
	scth.y = cos(ang.x * 3.1415926536f);
    vec2 scphi = vec2(sqrt(1.0 - ang.y*ang.y), ang.y);
    return vec3(scth.y*scphi.x, scth.x*scphi.x, scphi.y);
}

//n = normal
//m = half
//a = roughness factor
float ggx(vec3 n, vec3 m, float a)
{
    float top = a * a;
    float pA = max(0.0, dot(n, m));
    pA = pA * pA * (top - 1) + 1;
    pA = pA * pA * 3.14159;
    return top/pA;
}


float beckmann_NDF(vec3 n, vec3 m, float a)
{
    float nDotH2 = max(0, dot(n, m));
	nDotH2 *= nDotH2;
    float exponent = (nDotH2 - 1)/(nDotH2 * a * a);
    return exp(exponent)/(3.14159 * a * a * nDotH2 * nDotH2);
}



//f0 = fresnel factor
//h = half vector
//v = view vector
float fresnel_schlick(float f0,	vec3 h, vec3 v, float vDotH)
{
    return f0 + (1 - f0) * pow((1 - vDotH), 5);
}



float kelemen(vec3 l, vec3 v, vec3 h, vec3 n, float r, float nDotL, float nDotV, float vDotH)
{
    return (nDotL * nDotV)/(vDotH * vDotH);
}

float geometric_schlick(vec3 n, vec3 v, float k, float nDotV)
{
	return nDotV / (nDotV * (1 - k) + k);
}

float geometric_smith_schlick(vec3 l, vec3 v, vec3 h, vec3 n, float r, float nDotL, float nDotV, float vDotH)
{
	float k = r * 0.797884;
	return geometric_schlick(n, l, k, nDotL) * geometric_schlick(n, v, k, nDotV);
}


//Precalculated fetches
float p_ggx(vec3 n, vec3 m, float a)
{
    return texture2D(preCalc, vec2((dot(n, m) + 1) * 0.5, a)).r;
}

float p_fresnel_schlick(float f0,	vec3 h, vec3 v, float vDotH)
{
	return texture2D(preCalc, vec2((vDotH + 1) * 0.5, f0)).g;
}

float p_geometric_schlick(vec3 n, vec3 v, float k, float nDotV)
{
	return texture2D(preCalc, vec2((nDotV + 1) * 0.5, k)).b;
}

float p_geometric_smith_schlick(vec3 l, vec3 v, vec3 h, vec3 n, float r, float nDotL, float nDotV, float vDotH)
{
	float k = r * 0.797884;
	return p_geometric_schlick(n, l, k, nDotL) * p_geometric_schlick(n, v, k, nDotV);
}


vec4 cooktorr(vec3 n, vec3 v, vec3 l, float f0, float r, vec4 spec, vec4 dif)
{
    vec3 h = normalize((l + v));
    float nDotV = dot(n, v);
    float nDotL = dot(n, l);
    float vDotH = dot(v, h);
    
	float roughness = ggx(n, h, r * r);
    float fresnel = fresnel_schlick(f0, h, v, vDotH);
    float geometric = geometric_smith_schlick(l, v, h, n, r * r, nDotL, nDotV, vDotH);

    float rs = fresnel * roughness * geometric/(3.14159 * nDotV * nDotL);
    //rs = max(0.0, rs);
	//return vec4(vec3(rs), 1.0);
    return vec4(spec * rs + dif * fresnel);
}



void main(){
    vec4 dif = texture2D(colorMap, UV);
    vec3 worldCoord = texture2D(worldData, UV).rgb;
    vec3 l = normalize(-lDir);
    vec3 v = worldCoord - EyePos;
    v = normalize(v);

	vec4 tmp = texture2D(normData, UV);	//rg = normals, b = specular factor, a = glossiness
    vec3 n = normalize(decode(tmp.rg));
    /*vec3 r = reflect( v, n );
    float m = 2. * sqrt( 
        pow( r.x, 2. ) + 
        pow( r.y, 2. ) + 
        pow( r.z + 1., 2. ) 
    );

    vec2 vN = r.xy / m + .5;*/
    lit = vec4(lColor.rgb, 1) * cooktorr(n, v, l, tmp.b, tmp.a, texture2D(ssrMap, UV), dif);

	const vec3 fac = vec3(0.299, 0.587, 0.114);
    float lum = dot(fac, lit.rgb);
    bloom = mix(0.0, 1.0, 1.0 - step(1.0, lum)) * lit;
    lit.a = 1;
    bloom.a = 1;
}



