#version 430 core

in vec2 UV;	
in vec3 color; 
in vec3 secondaryColor;	

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

uniform vec3 EyeDir;

vec2 encode (vec3 n)
{
    float p = sqrt(n.z*8+8);
    return vec2(n.xy/p + 0.5);
}

void main()
{
		vec3 finalNormal = normalize(normPos);

		Normal0.rg = encode(finalNormal);	//Compress the normal data so we can eliminate one texture
		Normal0.b = 0;	//AO Map
		Normal0.a = 0;	//Cavity Map
		
		RGBA0 = vec4(0.3, 0.5, 0.8, 0.6) * (1.0 - dot(normalize(normPos), -normalize(EyeDir)));

		Material0 = vec4(0);

		Depth0.rgb = worldXY.xyz;
		Depth0.a = 0;	//Microsurface Map

		//We might have to write to gl_FragDepth here, but for now I think it's fine
		gl_FragDepth = log2(logBufDat.y) * logBufDat.x;
}