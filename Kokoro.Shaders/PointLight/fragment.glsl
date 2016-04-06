﻿#version 430 core

// Interpolated values from the vertex shaders
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
uniform sampler2D envMap;
uniform vec3 lPos;
uniform vec4 lColor;
uniform vec3 EyePos;
uniform vec2 ScreenSize;

in float flogz;
uniform float Fcoef;

void main(){

	vec2 uv_coord = gl_FragCoord.xy/ScreenSize;
    vec3 worldCoord = texture2D(worldData, uv_coord).rgb;
    vec3 l = normalize(worldCoord - lPos.xyz);
    vec3 v = EyePos - worldCoord;
    v = normalize(-v);

	float dist = distance(worldCoord, lPos.xyz);
	//dist += 0.001f;

    
	
	vec3 n = 2.0 * texture2D(normData, uv_coord).xyz - 1.0;
    n = normalize(n);
    vec4 dif = texture2D(colorMap, uv_coord);
    vec4 spec = texture2D(specularData, uv_coord);
    const float f0 = 1;

	float attenuation = max(0, dot(n, l)) / (lColor.a * (dist + 1) * (dist + 1));
	lit = attenuation * vec4(lColor.rgb, 1);

	const vec3 fac = vec3(0.299, 0.587, 0.114);
    float lum = dot(fac, lit.rgb);
    bloom = mix(0.0, 1.0, 1.0 - step(0.8, lum)) * lit;
	bloom = vec4(0);
    gl_FragDepth = Fcoef * 0.5 * log2(flogz);
}

