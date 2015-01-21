#version 430

layout(location = 0) uniform vec2 zoom;
layout(location = 1) uniform vec2 viewPosition;

layout(location = 0) in vec2 position;

void main()
{
	gl_Position = vec4(zoom * (position + viewPosition),0.0,0.0);
}

