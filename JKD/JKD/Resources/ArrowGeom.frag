#version 330
#extension GL_ARB_explicit_attrib_location : enable

layout(lines) in;
layout(triangle_strip, max_vertices = 7) out;

uniform vec2 absoluteZoom;
uniform vec3 constants;

in gl_PerVertex
{
	vec4 gl_Position;
} gl_in[];

out gl_PerVertex
{
	vec4 gl_Position;
};


void main()
{
	vec2 c = gl_in[1].gl_Position.xy - gl_in[0].gl_Position.xy;
	vec2 q = constants.x * absoluteZoom * normalize(vec2(c.y,-c.x));
	float n = 1-length(absoluteZoom * c * constants.z);
	gl_Position = vec4(gl_in[0].gl_Position.xy-q, 0, 1);
	EmitVertex();
	gl_Position = vec4(gl_in[0].gl_Position.xy+q, 0, 1);
	EmitVertex();
	gl_Position = vec4(gl_in[0].gl_Position.xy - q + n * c, 0, 1);
	EmitVertex();
	gl_Position = vec4(gl_in[0].gl_Position.xy + q + n * c, 0, 1);
	EmitVertex();
	EndPrimitive();

	vec2 r = constants.y * absoluteZoom * normalize(vec2(c.y,-c.x));
	gl_Position = vec4(-r + n * c, 0, 1);
	EmitVertex();
	gl_Position = vec4(r + n * c, 0, 1);
	EmitVertex();
	gl_Position = vec4(gl_in[1].gl_Position.xy, 0, 1);
	EmitVertex();
	EndPrimitive();

}

