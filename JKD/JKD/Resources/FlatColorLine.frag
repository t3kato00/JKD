#version 330
#extension GL_ARB_explicit_uniform_location : enable
#extension GL_ARB_explicit_attrib_location : enable

uniform vec4 lineColor;

layout(location = 0) out vec4 color;

void main()
{
	color = lineColor;
}

