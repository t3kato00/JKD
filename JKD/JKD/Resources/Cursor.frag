#version 330
#extension GL_ARB_explicit_uniform_location : enable
#extension GL_ARB_explicit_attrib_location : enable

uniform float cursorBorder;
uniform float cursorSize;

in Fragment
{
	vec2 textureCoordinate;
};

layout(location = 0) out vec4 color;

void main()
{
	float n = cursorSize/cursorBorder;
	vec2 atx = abs(abs(textureCoordinate) - vec2(1.0,1.0));
	float nearness = max(atx.x,atx.y);
	float k = 255.0*max(0.0, 1.0 - nearness*nearness);
	//color = vec4( k, k, k, 1.0 );
	color = vec4( textureCoordinate, 0.0, 1.0 );
	//color = vec4( 255.0, 255.0, 255.0, 1.0 );
}

