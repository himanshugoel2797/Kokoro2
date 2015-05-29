// Assumes an sRGB target (i.e., the output is already encoded to gamma 2.1)
#version 330


layout(location = 0) out vec4 color;
in vec2 UV;

uniform sampler2D diffuse;
uniform float temperature;

varying float noise;

float random( vec3 scale, float seed ){
    return fract( sin( dot( gl_FragCoord.xyz + seed, scale ) ) * 43758.5453 + seed ) ;
}


vec3 rgb2hsv(vec3 c)
{
    vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
    vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main(void) {    

	float temp = temperature * 0.01;

	float red = temp - 60;
	red = 329.698727446 * pow(red, -0.1332047592);
	red = clamp(red, 0, 255);
	color.r = red/255;

	float green = temp - 60;
    green = 288.1221695283 * pow(green, -0.0755148492);
    green = clamp(green, 0, 255);
	color.g = green/255;

	color.b = 1;
	color.a = 1;

	vec3 hsvCol = hsv2rgb(color.rgb);
	
	float r = .01 * random( vec3( 12.9898, 78.233, 151.7182 ), 0.0 );
    vec2 tPos = vec2( 0, 1.0 - 1.3 * noise + r );
    hsvCol.x *= texture2D( diffuse, tPos ).r;
	
	color.rgb = hsv2rgb(hsvCol);
}