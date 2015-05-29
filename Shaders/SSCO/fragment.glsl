

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

float readDepth(in vec2 coord)  
   {  
		return texture2D(positions, coord).w;
   }   

   vec3 readColor(in vec2 coord)  
   {  
     return texture2D(diffuse, coord).xyz;  
   } 

   float compareDepths(in float depth1, in float depth2)  
   {  
     float gauss = 0.0; 
     float diff = (depth1 - depth2)*100.0; //depth difference (0-100)
     float gdisplace = 0.2; //gauss bell center
     float garea = 3.0; //gauss bell width

     //reduce left bell width to avoid self-shadowing
     if (diff<gdisplace) garea = 0.2; 

     gauss = pow(2.7182,-2*(diff-gdisplace)*(diff-gdisplace)/(garea*garea));

     return max(0.2,gauss);  
   }  

   vec3 calAO(float depth,float dw, float dh, inout float ao)  
   {  
     float temp = 0;
     vec3 bleed = vec3(0.0,0.0,0.0);
     float coordw = UV.x + dw/depth;
     float coordh = UV.y + dh/depth;

     if (coordw  < 1.0 && coordw  > 0.0 && coordh < 1.0 && coordh  > 0.0){

     	vec2 coord = vec2(coordw , coordh);
     	temp = compareDepths(depth, readDepth(coord)); 
        bleed = readColor(coord); 
 
     }
     ao += temp;
     return temp*bleed;  
   }

void main(){

//randomization texture:
     vec2 fres = vec2(64,64);
     vec3 random = texture2D(rnm, UV*fres.xy).xyz;
     random = random*2.0-vec3(1.0);

     //initialize stuff:
     float depth = readDepth(UV);
     vec3 gi = vec3(0.0,0.0,0.0);  
     float ao = 0.0;
	 float pw = rad.x;
	 float ph = rad.y;

     for(int i=0; i<8; ++i) 
     {  
       //calculate color bleeding and ao:
       gi += calAO(depth,  pw, ph,ao);  
       gi += calAO(depth,  pw, -ph,ao);  
       gi += calAO(depth,  -pw, ph,ao);  
       gi += calAO(depth,  -pw, -ph,ao); 
     
       //sample jittering:
       pw += random.x*0.0005;
       ph += random.y*0.0005;

       //increase sampling area:
       pw *= 1.4;  
       ph *= 1.4;    
     }         

     //final values, some adjusting:
     vec3 finalAO = vec3(1.0-(ao/32.0));
     vec3 finalGI = (gi/32)*0.6;

     color.a = finalAO.r;  
	 color.rgb = finalGI;
}

