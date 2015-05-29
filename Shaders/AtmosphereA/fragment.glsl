

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D diffuse;
uniform vec4 overallColor;
uniform sampler2D transparencyMap;
uniform vec2 screenSpaceCenter;
uniform float screenSpaceRadius;

void main(){

	// Output color = color of the texture at the specified UV
	color = (texture2D(diffuse, UV) * texture2D(transparencyMap, UV).r);
	color.a = texture2D(transparencyMap, UV).r;

	color += overallColor * (1-color.a);
	color.a += 0.3;

	color +=  (screenSpaceRadius - abs(length(UV - screenSpaceCenter)))/screenSpaceRadius * (vec4(1,1,1,1));
}