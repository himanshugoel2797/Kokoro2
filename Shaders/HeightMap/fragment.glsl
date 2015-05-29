

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

void main(){

	// Output color = color of the texture at the specified UV
	color = texture2D( diffuse, UV );
	//if(color.a <= 0.5)discard;

	worldPos.xyz = 0.5 * (vertexPos) + 1.0f;
	worldPos.w = 1;

	uv.xy = UV;
	uv.z = depth;
	uv.w = 1;

	//normal.xyz = 0.5f * normalize(normalPos) + 1.0f;
	normal.a = 1;
	normal.xyz = 0.5f * normalize(normalPos) + 1.0;

	//if(1.0 - normalize(normalPos).z > 0.9f)color.rgb = vec3(1, 0, 0);
	//else color.rgb = vec3(0, 1, 0);
	//color.rgb = vec3(1, 0, 0);

	if(1.0 - normalize(normalPos).z < 0.9f)color.rgb = vec3(0, 0, 1);

	materialData.r = roughness;
	materialData.g = fresnel;
	materialData.b = k;
	materialData.a = reflectiveness;

}