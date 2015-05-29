

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 color;
layout(location = 1) out vec4 worldPos;
layout(location = 2) out vec4 uv;
layout(location = 3) out vec4 normal;
layout(location = 4) out vec4 materialData;
layout(location = 5) out vec4 materialData1;

//varyings
smooth in vec3 vertexPos;
smooth in vec3 normalPos;
smooth in float depth;

// Values that stay constant for the whole mesh.
uniform sampler2D diffuse;
uniform sampler2D normalMap;
uniform vec2 materialExtraInfo2;

uniform float roughness;
uniform float fresnel;
uniform float k;
uniform float reflectiveness;
uniform vec3 col;

void main(){

	// Output color = color of the texture at the specified UV
	color.rgb = vec3(0.4, 0.5, 0.6);
	//if(color.a <= 0.5)discard;

	materialData = uv = worldPos = vec4(0);
	worldPos.xyz = 0.5 * (vertexPos) + 1.0f;
	worldPos.w = 1;

	uv.xy = UV;
	uv.z = 1500;
	//uv.w = 1;

	normal.xyz = 0.5f * normalize(-normalPos) + 1.0f;
	normal.a = 1;
	//normal.xyz = 0.5f * normalize(normalPos * texture2D(normalMap, UV).rgb) + 1.0;

	materialData.r = 0;
	//materialData.g = fresnel;
	//materialData.b = k;
	//materialData.a = reflectiveness;

}