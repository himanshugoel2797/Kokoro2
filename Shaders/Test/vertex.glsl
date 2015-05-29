



varying vec3 varposition;

varying vec3 varnormal;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Proj;
uniform mat3 Normal;

layout(location = 0) in vec3 vertexPosition_modelspace;
layout(location = 1) in vec2 vertexUV;
layout(location = 2) in vec3 normal;


#ifdef CLIP_WORKAROUND

varying float gl_ClipDistance[6];

#endif



void main()

{

	vec4 co = View * Model * vec4(vertexPosition_modelspace, 1);



	varposition = co.xyz;

	varnormal = normalize(Normal * normal);

	gl_Position = Proj * co;



#ifdef CLIP_WORKAROUND

	int i;

	for(i = 0; i < 6; i++)

		gl_ClipDistance[i] = dot(co, gl_ClipPlane[i]);

#elif !defined(GPU_ATI)

	// Setting gl_ClipVertex is necessary to get glClipPlane working on NVIDIA

	// graphic cards, while on ATI it can cause a software fallback.

	gl_ClipVertex = co; 

#endif 

}

