#version 430

layout(location = 2) uniform vec3 lineColor;

layout(location = 0) out vec3 color;

void main()
{
	color = lineColor;
}

