#version 130
#extension GL_ARB_explicit_uniform_location : enable
#extension GL_ARB_explicit_attrib_location : enable

layout(location = 0) uniform vec2 zoom;
layout(location = 2) uniform vec2 viewPosition;

layout(location = 0) in vec2 position;

void main()
{
	gl_Position = vec4(zoom * (position + viewPosition),0.0,0.0);
}

