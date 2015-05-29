

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

#define SAMPLES 16 // 10 is good
const float invSamples = 1/16.0;

//implement mixed frequemcu ao to get both high and low details, colored ao? SSGI

void main(){
const vec2 poisson16[] = vec2[](    // These are the Poisson Disk Samples
                                vec2( -0.94201624,  -0.39906216 ),
                                vec2(  0.94558609,  -0.76890725 ),
                                vec2( -0.094184101, -0.92938870 ),
                                vec2(  0.34495938,   0.29387760 ),
                                vec2( -0.91588581,   0.45771432 ),
                                vec2( -0.81544232,  -0.87912464 ),
                                vec2( -0.38277543,   0.27676845 ),
                                vec2(  0.97484398,   0.75648379 ),
                                vec2(  0.44323325,  -0.97511554 ),
                                vec2(  0.53742981,  -0.47373420 ),
                                vec2( -0.26496911,  -0.41893023 ),
                                vec2(  0.79197514,   0.19090188 ),
                                vec2( -0.24188840,   0.99706507 ),
                                vec2( -0.81409955,   0.91437590 ),
                                vec2(  0.19984126,   0.78641367 ),
                                vec2(  0.14383161,  -0.14100790 )
                               );


							   // reconstruct position from depth, USE YOUR CODE HERE
    vec3 viewPos = texture2D(positions, UV).rgb * 2.0 - 1.0;
 
    // get the view space normal, USE YOUR CODE HERE
    vec3 viewNormal = texture2D(normals, UV).rgb * 2.0 - 1.0;
 
    float ambientOcclusion = 0;
    // perform AO
    for (int i = 0; i < SAMPLES; ++i)
    {
        // sample at an offset specified by the current Poisson-Disk sample and scale it by a radius (has to be in Texture-Space)
        vec2 sampleTexCoord = UV + (poisson16[i] * (rad));
        vec3 samplePos = texture2D(positions, sampleTexCoord).rgb * 2.0 - 1.0;
        vec3 sampleDir = normalize(samplePos - viewPos);
 
        // angle between SURFACE-NORMAL and SAMPLE-DIRECTION (vector from SURFACE-POSITION to SAMPLE-POSITION)
        float NdotS = max(dot(viewNormal, sampleDir), 0);
        // distance between SURFACE-POSITION and SAMPLE-POSITION
        float VPdistSP = distance(viewPos, samplePos);
 
        // a = distance function
        float a = 1.0 - smoothstep(falloff, falloff * 2, VPdistSP);
        // b = dot-Product
        float b = NdotS;
 
        ambientOcclusion += (a * b);
    }
 
    color = vec4((ambientOcclusion * invSamples));
}

