﻿#version 430 core

in vec2 UV;
in float depth;
in vec3 worldXY;
smooth in vec3 normPos;
in vec2 logBufDat;

layout(location = 0) out vec4 RGBA0;
layout(location = 1) out vec4 Depth0;
layout(location = 2) out vec4 Normal0;
layout(location = 3) out vec4 Material0;

uniform sampler2D AlbedoMap;
uniform sampler2D AOMap;
uniform sampler2D CavityMap;
uniform sampler2D ReflectivityMap;
uniform sampler2D DerivativeAOCavityMicrosurfaceMap;

vec2 encode (vec3 n)
{
    float p = sqrt(n.z*8+8);
    return vec2(n.xy/p + 0.5);
}

void main()
{
		vec4 data = texture2D(DerivativeAOCavityMicrosurfaceMap, UV);

		vec3 dpdx = dFdx(worldXY);
		vec3 dpdy = dFdy(worldXY);

		float dhdx = dFdx(data.r);
		float dhdy = dFdy(data.r);

		vec3 r1 = cross(dpdy, normPos);
		vec3 r2 = cross(normPos, dpdx);

		vec3 finalNormal = normalize(normPos - (r1 * dhdx + r2 * dhdy)/dot(dpdx, r1) );

		Normal0.rg = encode(finalNormal);	//Compress the normal data so we can eliminate one texture
		Normal0.b = data.g;	//AO Map
		Normal0.a = data.b;	//Cavity Map


		RGBA0 = texture2D(AlbedoMap, UV);

		Material0 = texture2D(ReflectivityMap, UV);

		Depth0.rgb = worldXY.xyz;
		Depth0.a = data.a;	//Microsurface Map

		//We might have to write to gl_FragDepth here, but for now I think it's fine
}