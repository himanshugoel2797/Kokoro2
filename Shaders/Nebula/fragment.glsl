// Assumes an sRGB target (i.e., the output is already encoded to gamma 2.1)
#version 330
 
// In the noise-function space. xy corresponds to screen-space XY
uniform vec3    origin;
uniform mat2    rotate;
uniform float	zoom;
uniform float	alpha;
uniform float	iterations;
uniform float	mode;
 
#define volsteps 8
 
#define sparsity 0.5  // .4 to .5 (sparse)
#define stepsize 0.2
#define frequencyVariation   1.3 // 0.5 to 2.0
 
#define brightness 0.0018
#define distfading 0.6800


layout(location = 0) out vec4 color;
 
void main(void) {    


	// viewport resolution (in pixels)
	vec2    resolution = vec2(192, 108);
	vec2    invResolution = 1/resolution;
 

    vec2 UVImg = gl_FragCoord.xy * invResolution - 0.5;
    UVImg.y *= resolution.y * invResolution.x;
 
    vec3 dir = vec3(UVImg * zoom, 1.0);
    dir.xy *= rotate;

    float s = 0.1, fade = 0.01;
    color.rgb = vec3(0.0);
     
    for (int r = 0; r < volsteps; ++r) {
 
        vec3 p = origin + dir * (s * 0.5);
        p = vec3(frequencyVariation) - mod(p, vec3(frequencyVariation * 2.0));
 
        float prevlen = 0.0, a = 0.0;
        for (int i = 0; i < iterations; ++i) {
            p = abs(p);
            p = p * (1.0 / dot(p, p)) + (-sparsity); // the magic formula            
            float len = length(p);
            a += abs(len - prevlen); // absolute sum of average change
            prevlen = len;
        }
         
        a *= a * a; // add contrast
         
        // coloring based on distance        
        color.rgb += (vec3(s, s*s, s*s*s) * a * brightness + 1.0) * fade;
        fade *= distfading; // distance fading
        s += stepsize;

    }
     
    color.rgb = min(color.rgb, vec3(1.2));
	color.a = max(mode, alpha * ((color.r + color.g + color.b)/3));
}