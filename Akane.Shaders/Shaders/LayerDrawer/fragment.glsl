#version 440 core


// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
layout(location = 0)out vec4 color;
layout(location = 1)out vec4 height;

// Values that stay constant for the whole mesh.
uniform sampler2D ColorMap;
uniform sampler2D TileIDs;
uniform sampler2D HeightMap;

uniform float maxGid;
uniform float firstGid;
uniform vec2 textureSize;
uniform vec2 mapSize;
uniform vec2 tileSize;
uniform vec2 viewportTileRes;
uniform vec2 viewportOffset;
uniform float layerNum;
uniform float maxLayer;

void main(){
	
	vec2 tileCounts = vec2(textureSize/tileSize);
	
	vec2 uvStep = vec2(1)/viewportTileRes;	//The value at which a new tile should occur
	vec2 curPos = mod(UV, uvStep);
	vec2 tileBaseCoords = clamp(UV - curPos, vec2(0.0), vec2(1));	//the UV coordinates of the base of the current tile

	vec2 nextTilePos = tileBaseCoords + uvStep;
	vec2 mapInfo = vec2(1)/viewportTileRes;
	vec2 curPixelPos = mod(UV, mapInfo);


	color = vec4(0);
	height = vec4(0);

	//The id of the tile
	float tileId = round(float(texelFetch(TileIDs, ivec2( ceil((UV - curPixelPos) * viewportTileRes - vec2(0.3, 0.3) + viewportOffset )), 0).r * maxGid)) - firstGid;
	
	//if(tileId >= 0 && tileId <= maxGid)
	{
		//The appropriate tile position
		vec2 offset = vec2(0);
		offset.x = mod(tileId, tileCounts.x);
		offset.y = (tileId - offset.x)/tileCounts.x + 1; 
		offset.y = tileCounts.y - offset.y;

		//offset = round(offset);
		offset = (offset * tileSize)/textureSize;

		vec2 UVCoords =  offset + ((curPos/uvStep) * tileSize)/textureSize;

		// Output color = color of the texture at the specified UV
		color = texture2D(ColorMap, UVCoords );
		height.rgb = vec3(layerNum/maxLayer * color.a);
		height.a = 1;
	}
}