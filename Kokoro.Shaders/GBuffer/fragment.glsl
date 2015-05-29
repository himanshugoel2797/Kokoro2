in vec2 UV;
in float depth;
in vec3 worldXY;
smooth in vec3 normPos;
in vec3 tangent;
in vec3 bitangent;

layout(location = 0) out vec4 RGBA0;
layout(location = 1) out vec4 Depth0;
layout(location = 2) out vec4 Normal0;


uniform sampler2D ColorMap;
uniform sampler2D LightingMap;
uniform sampler2D NormalMap;
uniform float Emissivity;

void main()
{
		vec3 finalNormal = normalize(normPos);// + cross( normalize(bitangent), normalize(2.0f * texture2D(NormalMap, UV).rgb - 1.0f));
		Normal0 = vec4(normalize(0.5f * finalNormal + 0.5f), 1);
		RGBA0 = texture2D(ColorMap, UV);
		Depth0.r = depth/50;
		Depth0.gb = worldXY.xy;
		Depth0.a = 1;
}