

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D diffuse;
uniform sampler2D normals;
uniform sampler2D positions;
uniform sampler2D uvs;
uniform sampler2D matDataA;
uniform sampler2D matDataB;

in vec3 cameraPosition;

//Directional Light Params
uniform vec3 lightColor0;
uniform vec3 lightDirection0;

uniform vec3 lightColor1;
uniform vec3 lightDirection1;

uniform vec3 lightColor2;
uniform vec3 lightDirection2;



//Mode selections
uniform float mode;		//1 = directional light, 2 = point light

void main(){

	vec4 normalData = texture2D(normals, UV);
    //tranform normal back into [-1,1] range
    //vec3 normal = 2.0f * normalData.xyz - 1.0f;
	vec3 normal = normalize(2.0f * normalData.xyz - 1.0f);
	//vec3 normal = normalData.xyz;
	vec3 position = 2.0f * texture2D(positions, UV).rgb - 1.0f;	//Get the world space position of the point
	
	float roughnessValue = texture2D(matDataA, UV).r; // 0 : smooth, 1: rough
    float F0 = texture2D(matDataA, UV).g; // fresnel reflectance at normal incidence
    float k = texture2D(matDataA, UV).b; 

	if(roughnessValue != 0){
	//Optimize for 3 directional lights
	vec3 diffuseLight = lightColor0;
	float specularLight = 0;

	//Directional Light 1
	//Lambertain diffuse
	vec3 lightVector = -normalize(lightDirection0);
	float NdL = max(0.0, dot(normal,lightVector));
    diffuseLight = NdL * lightColor0;

	
	//Directional Light 2
	//Lambertain diffuse
	lightVector = -normalize(lightDirection1);
	NdL = max(0.0, dot(normal,lightVector));
    diffuseLight += NdL * lightColor1;


	//Directional Light 3
	//Lambertain diffuse
	lightVector = -normalize(lightDirection2);
	NdL = max(0.0, dot(normal,lightVector));
    diffuseLight += NdL * lightColor2;

	//output the two lights
	color.rgb = max(vec3(0), diffuseLight);

	}else{
	
		color.rgb = vec3(0);
	}
		color.a = 1;
}

