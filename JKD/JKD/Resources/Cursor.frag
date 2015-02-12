#version 330
#extension GL_ARB_explicit_uniform_location : enable
#extension GL_ARB_explicit_attrib_location : enable

uniform float cursorBorder;
uniform float cursorSize;
uniform vec4 cursorColor;

in Fragment
{
	vec2 textureCoordinate;
};

layout(location = 0) out vec4 color;

void main()
{
	float n = cursorSize/cursorBorder;
	vec2 atx = abs(textureCoordinate) - vec2(1.0,1.0);
	float nearness;
	
	if(atx.x >= 0 && atx.y >= 0){
		nearness = max(atx.x, atx.y);
	}
	else if(atx.x < 0 && atx.y >= 0){
		nearness = atx.y;
	}
	else if(atx.x >=0 && atx.y < 0){
		nearness = atx.x;
	}
	else{
		nearness = min(-atx.x, -atx.y);	
	}
	float k = max(0.0, 1.0 - nearness*n);
	color = vec4(k*cursorColor.xyz, cursorColor.w);
	//color = vec4( textureCoordinate, 0.0, 1.0 );
	//color = vec4( 255.0, 255.0, 255.0, 1.0 );
}

