﻿#version 430 core

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0) out vec4 color;

// Values that stay constant for the whole mesh.
uniform sampler2D AlbedoMap;
uniform float blurSize;

void main(){
   vec4 sum = vec4(0.0);
 
   // blur in x (horizontal)
   // take nine samples, with the distance blurSize between them
   sum += (texture2D(AlbedoMap, vec2(UV.x - 4.0*blurSize, UV.y))) * 0.05;
   sum += (texture2D(AlbedoMap, vec2(UV.x - 3.0*blurSize, UV.y))) * 0.09;
   sum += (texture2D(AlbedoMap, vec2(UV.x - 2.0*blurSize, UV.y))) * 0.12;
   sum += (texture2D(AlbedoMap, vec2(UV.x - blurSize, UV.y))) * 0.15;
   sum += (texture2D(AlbedoMap, vec2(UV.x, UV.y))) * 0.16;
   sum += (texture2D(AlbedoMap, vec2(UV.x + blurSize, UV.y))) * 0.15;
   sum += (texture2D(AlbedoMap, vec2(UV.x + 2.0*blurSize, UV.y))) * 0.12;
   sum += (texture2D(AlbedoMap, vec2(UV.x + 3.0*blurSize, UV.y))) * 0.09;
   sum += (texture2D(AlbedoMap, vec2(UV.x + 4.0*blurSize, UV.y))) * 0.05;
 
	color = sum;
}