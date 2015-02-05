#version 330
#extension GL_ARB_explicit_uniform_location : enable
#extension GL_ARB_explicit_attrib_location : enable

uniform vec2 zoom;
uniform vec2 viewPosition;

layout(location = 0) in vec2 position;

out gl_PerVertex
{
	vec4 gl_Position;
};

void main()
{
	gl_Position = vec4(zoom * (position + viewPosition),0.0,1.0);
}

