

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D diffuse;
uniform float blurSize;

void main(){

   vec4 clamp = vec4(0,0,0,0);
   vec4 sum = vec4(0.0);
 
   // blur in x (horizontal)
   // take nine samples, with the distance blurSize between them
   sum += (texture2D(diffuse, vec2(UV.x - 4.0*blurSize, UV.y)) - clamp) * 0.05;
   sum += (texture2D(diffuse, vec2(UV.x - 3.0*blurSize, UV.y)) - clamp) * 0.09;
   sum += (texture2D(diffuse, vec2(UV.x - 2.0*blurSize, UV.y)) - clamp) * 0.12;
   sum += (texture2D(diffuse, vec2(UV.x - blurSize, UV.y)) - clamp) * 0.15;
   sum += (texture2D(diffuse, vec2(UV.x, UV.y)) - clamp) * 0.16;
   sum += (texture2D(diffuse, vec2(UV.x + blurSize, UV.y)) - clamp) * 0.15;
   sum += (texture2D(diffuse, vec2(UV.x + 2.0*blurSize, UV.y)) - clamp) * 0.12;
   sum += (texture2D(diffuse, vec2(UV.x + 3.0*blurSize, UV.y)) - clamp) * 0.09;
   sum += (texture2D(diffuse, vec2(UV.x + 4.0*blurSize, UV.y)) - clamp) * 0.05;
 
	color = sum;
}