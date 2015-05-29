

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 vertexPosition_modelspace;
layout(location = 1) in vec2 vertexUV;
layout(location = 2) in vec3 normal;

// Output data ; will be interpolated for each fragment.
out vec2 UV;

// Values that stay constant for the whole mesh.
uniform mat4 Model;
uniform mat4 View;
uniform mat4 Proj;

uniform sampler2D heightMap;
uniform float zNear;
uniform float zFar;

smooth out vec3 vertexPos;
smooth out vec3 normalPos;
smooth out float depth;

void main(){

	// Output position of the vertex, in clip space : MVP * position
	mat4 MVP = Proj * View * Model;
	vec3 vertexMSP = vertexPosition_modelspace;

	vec4 heights = textureGather(heightMap, vertexUV, 0);
	float avg = (heights.x + heights.y + heights.z + heights.w)/4;

	//avg = texture2D(heightMap, vertexUV).r;

	vertexMSP += normal * -log((avg - 0.5) * 5);
	//vertexMSP.y = min(0.05f, vertexMSP.y);
	
	vec2 size = vec2(2.0,0.0);
	const ivec3 off = ivec3(-1,0,1);
	vec4 wave = texture(heightMap, vertexUV);
    float s11 = wave.x;
    float s01 = textureOffset(heightMap, vertexUV, off.xy).x;
    float s21 = textureOffset(heightMap, vertexUV, off.zy).x;
    float s10 = textureOffset(heightMap, vertexUV, off.yx).x;
    float s12 = textureOffset(heightMap, vertexUV, off.yz).x;
    vec3 va = normalize(vec3(size.xy,s21-s01));
    vec3 vb = normalize(vec3(size.yx,s12-s10));
    vec4 bump = vec4( cross(va,vb), s11 );

	normalPos = (Model * bump).xyz;
	//normalPos = (Model * vec4(normal, 0)).xyz;
	
	if(1.0 - normalize(normalPos).z < 0.9){
		vertexMSP.y += 0.25 * sin(vertexUV.x);
	}
	
	gl_Position =  MVP * vec4(vertexMSP,1);
	
	vertexPos = (MVP * vec4(vertexMSP, 1)).xyz;
	
	depth = (gl_Position.z * gl_Position.w - zNear)/(zFar - zNear);

	// UV of the vertex. No special space for this one.
	UV = vertexUV;
}