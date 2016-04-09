#version 430 core

// Interpolated values from the vertex shaders
in vec2 UV;


// Ouput data
layout(location = 0) out vec4 c_posData;
layout(location = 1) out vec4 c_dVData;


// Values that stay constant for the whole mesh.
uniform vec2 MassRange;
uniform sampler2D PosData;
uniform sampler2D dVData;
uniform vec3 Impulse;
uniform float DeltaTime;

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main(){
	c_dVData = texture2D(dVData, UV);
	c_posData = texture2D(PosData, UV);

	c_posData.xyz = min(vec3(1), c_posData.xyz + c_dVData.xyz * DeltaTime);
	c_dVData.xyz = Impulse * c_dVData.w * DeltaTime * rand(c_posData.xy);
}