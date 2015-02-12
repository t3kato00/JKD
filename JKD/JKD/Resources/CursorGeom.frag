#version 330
#extension GL_ARB_explicit_uniform_location : enable
#extension GL_ARB_explicit_attrib_location : enable

layout(points) in;
layout(triangle_strip, max_vertices = 4) out;

uniform float time;
uniform vec2 absoluteZoom;
uniform float cursorSize;
uniform float cursorBorder;

in gl_PerVertex
{
	vec4 gl_Position;
} gl_in[];

out gl_PerVertex
{
	vec4 gl_Position;
};

out Fragment
{
	vec2 textureCoordinate;
};

void main()
{
	float t2 = time;
	float k = cursorSize+cursorBorder;
	float n = k/cursorSize;
	//float n = 1.0;
	vec2 off = k*vec2( sin(t2), cos(t2) );
	vec2 offxy = absoluteZoom*off.xy;
	vec2 offyx = absoluteZoom*off.yx;

	gl_Position = vec4( gl_in[0].gl_Position.xy + offxy, 0.0, 1.0);
	textureCoordinate = vec2(n,n);
	EmitVertex();
	gl_Position = vec4( gl_in[0].gl_Position.xy + vec2(-1.0,1.0)*offyx, 0.0, 1.0);
	textureCoordinate = vec2(-n,n);
	EmitVertex();
	gl_Position = vec4( gl_in[0].gl_Position.xy + vec2(1.0,-1.0)*offyx, 0.0, 1.0);
	textureCoordinate = vec2(n,-n);
	EmitVertex();
	gl_Position = vec4( gl_in[0].gl_Position.xy + vec2(-1.0,-1.0)*offxy, 0.0, 1.0);
	textureCoordinate = vec2(-n,-n);
	EmitVertex();
	EndPrimitive();
}

