

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D diffuse;
uniform sampler2D normals;
uniform sampler2D positions;
uniform sampler2D uvs;
uniform sampler2D matDataA;
uniform sampler2D matDataB;
uniform sampler2D lightMap;
uniform sampler2D ssaoMap;
uniform sampler2D shadowMap;
uniform vec3 cameraPos;
uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

uniform bool lighting;
uniform bool ssao;
uniform vec3 ambientLight;

const float c = 0.6f;
const float b = 0.0005f;

vec3 applyFog( in vec3  rgb,      // original color of the pixel
               in float distance, // camera to point distance
               in vec3  rayDir,   // camera to point vector
               in vec3  sunDir )  // sun light direction
{
    float fogAmount = 1.0 - exp( -distance*b );
    float sunAmount = max( dot( rayDir, sunDir ), 0.0 );
    vec3  fogColor  = mix( vec3(0.5,0.6,0.7), // bluish
                           vec3(1.0,0.9,0.7), // yellowish
                           pow(sunAmount,8.0) );
    return mix( rgb, fogColor, fogAmount );
}

vec3 applyFog1( in vec3  rgb,      // original color of the pixel
               in float distance, // camera to point distance
               in vec3  rayOri,   // camera position
               in vec3  rayDir )  // camera to point vector
{
    float fogAmount = c * exp(-rayOri.y*b) * (1.0-exp( -distance*rayDir.y*b ))/rayDir.y;
    vec3  fogColor  = vec3(0.5,0.6,0.7);
    return mix( rgb, fogColor, fogAmount );
}

void main(){
	// Output color = color of the texture at the specified UV

	vec4 light = texture2D(lightMap, UV);
	vec3 diffuseL = light.rgb;
	float specular = light.a;
	float lit = texture2D(matDataA, UV).r;
	float fog = texture2D(matDataA, UV).g;
	
	vec3 col = texture2D(diffuse, UV).rgb;
	float k = texture2D(matDataA, UV).b;
	
	vec3 occlussion = texture2D(ssaoMap, UV).aaa;
	//occlussion *=  clamp(vec3(0.0), vec3(1.0), log(ambientLight));
	color.rgb = col;

	//Output the lightmap + the original
	if(lighting && !ssao)color.rgb = (col * (vec3(ambientLight) + diffuseL) * k) + max(0, specular * (1.0 - k));
	if(lighting && ssao)color.rgb = (col * ( occlussion + diffuseL));// + max(0, specular * (1.0 - k)) + texture2D(ssaoMap, UV).rgb;// +  col * texture2D(shadowMap, UV).rgb;
	if(!lighting && ssao)color.rgb = (col * occlussion);
	//color.rgb = (texture2D(lightMap, UV).rgb);	//Apply reflections
	//color.rgb += vec3(0.5 * texture2D(shadowMap, UV).a);	//Apply shadows
	//color.rgb = col * texture2D(ssaoMap, UV).aaa;
	if(lit == 0)color.rgb = col;

	mat4 invWVP = inverse(View * Projection);

	vec3 pos = 2.0f * texture2D(positions, UV).rgb - 1.0f;
	//pos = (invWVP * vec4(pos, 1)).xyz;
	//pos.z = texture2D(uvs, UV).b;
	if(fog != 0)color.rgb = applyFog1(color.rgb, texture2D(uvs, UV).b, cameraPos, normalize(-cameraPos - pos) );
	//color.rgb = applyFog(color.rgb, texture2D(uvs, UV).b, normalize(cameraPos - pos), -vec3(1, -1, 0) );
	
	//color.rgb = texture2D(ssaoMap, UV).aaa;
	color.a = 1;
}

