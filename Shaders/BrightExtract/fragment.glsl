

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D diffuse;
uniform vec4 col;
uniform float mode; 


vec4 threshold(float midPoint)
{
  vec4 texColor = texture2D(diffuse, UV);
  float brightness = (0.2126*texColor.r) + (0.7152*texColor.g) + (0.0722*texColor.b);
  
  if (brightness > midPoint) {
    return vec4(1.0, 1.0, 1.0, 1.0);
  } else {
    return vec4(0.0, 0.0, 0.0, 1.0);
  }

}


void main(){
	color = texture2D( diffuse, UV );
	
	color *= threshold(0.75f);

	//color *= float((all(greaterThan(color, vec4(0.7) ))));

	color.a = 1;
}