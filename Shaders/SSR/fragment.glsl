

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

//SSAO Uniforms
uniform sampler2D rnm;		//random normal data
uniform float totStrength;
uniform float strength;
uniform float offset;
uniform float falloff;
uniform vec2 rad;

uniform mat4 ProjectionMat;
in vec3 cameraPosition;
uniform vec3 lightColor0;
uniform vec3 lightDirection0;

#define SAMPLES 16 // 10 is good
const float invSamples = 1/16.0;


/*
 * Raytracing to get intersection point
 */
vec3 raytrace(in vec3 reflectionVector, in float startDepth)
{
	vec3 color = vec3(0.0f);
	float stepSize = 0.001; 
 
	float size = length(reflectionVector.xy);
	reflectionVector = normalize(reflectionVector/size);
	reflectionVector = reflectionVector * stepSize;
        
        // Current sampling position is at current fragment
	vec2 sampledPosition = UV;
        // Current depth at current fragment
	float currentDepth = startDepth;
        // The sampled depth at the current sampling position
	float sampledDepth = texture2D(positions, sampledPosition).w;
 
        // Raytrace as long as in texture space of depth buffer (between 0 and 1)
	while(sampledPosition.x <= 1.0 && sampledPosition.x >= 0.0 &&
	      sampledPosition.y <= 1.0 && sampledPosition.y >= 0.0)
	{
                // Update sampling position by adding reflection vector's xy and y components
		sampledPosition = sampledPosition + reflectionVector.xy;
                // Updating depth values
		currentDepth = currentDepth + reflectionVector.z * startDepth;
		float sampledDepth = texture2D(positions, sampledPosition).w;
                
                // If current depth is greater than sampled depth of depth buffer, intersection is found
		if(currentDepth > sampledDepth)
		{
                        // Delta is for stop the raytracing after the first intersection is found
                        // Not using delta will create "repeating artifacts"
			float delta = (currentDepth - sampledDepth);
			if(delta < 0.003f )
			{
				//color = vec3(0.3);
				color = texture2D(diffuse, sampledPosition).rgb;
				break;
			}
		}
	}
 
	return color;
}

void main(){

	vec3 reflectedColor = vec3(0.0f);
 float reflectivity = texture2D(matDataA, UV).a;
 if(reflectivity > 0){
	vec3 normal = (2.0 * texture2D(normals, UV).xyz) - 1.0;
 
	// Depth at current fragment
	float currDepth = texture2D(positions, UV).w;
 
	// Eye position, camera is at (0, 0, 0), we look along negative z, add near plane to correct parallax
	vec3 eyePosition = normalize( cameraPosition + vec3(0,0,0.01) );
	vec4 reflectionVector = ProjectionMat * reflect( vec4(-eyePosition, 0), vec4(normal, 0) ) ;
 
        // Call raytrace to get reflected color
	reflectedColor = raytrace(reflectionVector.xyz, currDepth);	
 }
 color.rgb = reflectivity * clamp(reflectedColor, vec3(0.0), vec3(2));
	//color = vec4(reflect( vec4(-eyePosition, 0), vec4(normal, 0) ));
	
	}