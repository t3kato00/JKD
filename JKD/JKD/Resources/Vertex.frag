
layout(location = 0) uniform float zoom;
layout(location = 1) uniform vec2 viewPosition;

layout(location = 0) in vec2 position;

void main()
{
	gl_Position = zoom * vec4(position + viewPosition,0.0,0.0);
}

