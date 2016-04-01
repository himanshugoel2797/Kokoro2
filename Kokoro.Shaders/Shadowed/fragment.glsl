#version 430 core

// Interpolated values from the vertex shaders
in vec2 UV;
in vec4 shadowCoord;
in vec3 worldCoord;
in vec3 norm;
// Ouput data
layout(location = 0) out vec4 shadow;
layout(location = 1) out vec4 worldPos;
layout(location = 2) out vec4 normDat;
layout(location = 3) out vec4 color;
layout(location = 4) out vec4 specular;
// Values that stay constant for the whole mesh.
uniform sampler2D AlbedoMap;
uniform sampler2D SpecularMap;
uniform sampler2D GlossinessMap;
uniform sampler2DShadow ShadowMap;
uniform sampler2D ReflectiveNormMap;
uniform sampler2D ReflectivePosMap;
in float flogz;
uniform float Fcoef;
vec2 poissonDisk[4] = vec2[](
  vec2( -0.94201624, -0.39906216 ),
  vec2( 0.94558609, -0.76890725 ),
  vec2( -0.094184101, -0.92938870 ),
  vec2( 0.34495938, 0.29387760 )
);
float rand(vec4 seed4)
{
    float dot_product = dot(seed4, vec4(12.9898,78.233,45.164,94.673));
    return fract(sin(dot_product) * 43758.5453);
}


void main(){
    float vis = 1.0f;
    vec3 shad = shadowCoord.xyz / shadowCoord.w;
    vec2 tUV;
    tUV.x = 0.5 * shad.x + 0.5;
    tUV.y = 0.5 * shad.y + 0.5;
    float z = 0.5 * shad.z + 0.5;
    for(int i = 0; i < 4; i++)
	{
        int index = int(rand(16 * vec4(gl_FragCoord.xyy, i))) % 4;
        vis -= 0.2 * (1.0 - texture(ShadowMap, vec3(0.5 * shad.xy + 0.5 + poissonDisk[i]/700, 0.5 * shad.z + 0.5), 0.0001));
    }

	if(vis > 0.4) vis = 1.0;

	vec3 sWorldPos = texture2D(ReflectivePosMap, tUV).rgb;
    vec3 sNormVal = 2.0 * texture2D(ReflectiveNormMap, tUV).rgb - 1.0;
    sNormVal = normalize(sNormVal);
    
	normDat.rgb = 0.5 * normalize(norm) + 0.5;
    normDat.a = 1;
    worldPos.rgb = worldCoord;
    worldPos.a = 1;

    color = texture2D(AlbedoMap, UV);
	specular = texture2D(SpecularMap, UV);
	specular.a = texture2D(GlossinessMap, UV).r;
	shadow = vec4(vis, vis, vis, 1.0);


    gl_FragDepth = Fcoef * 0.5 * log2(flogz);
}

