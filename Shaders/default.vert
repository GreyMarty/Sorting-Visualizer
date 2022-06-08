#version 430 core

layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 aSize;
layout (location = 2) in vec3 aColor;

out VS_OUT 
{
	vec2 size;
	vec3 color;
} vs_out;

void main()
{
	gl_Position = vec4(aPosition.x, aPosition.y, 0, 1);

	vs_out.size = aSize;
	vs_out.color = aColor;
}