#version 130
#extension GL_ARB_explicit_uniform_location : enable
#extension GL_ARB_explicit_attrib_location : enable

uniform vec3 lineColor;

layout(location = 0) out vec3 color;

void main()
{
	color = lineColor;
}

