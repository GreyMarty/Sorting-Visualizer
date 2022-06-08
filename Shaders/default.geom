#version 430 core

layout (points) in;
layout (triangle_strip, max_vertices = 4) out;

in VS_OUT 
{
	vec2 size;
	vec3 color;
} gs_in[];

out vec3 fColor;

void rect(vec4 position)
{
	gl_Position = position;
	EmitVertex();

	gl_Position = position + vec4(gs_in[0].size.x, 0, 0, 0);
	EmitVertex();

	gl_Position = position + vec4(0, gs_in[0].size.y, 0, 0);
	EmitVertex();

	gl_Position = position + vec4(gs_in[0].size.x, gs_in[0].size.y, 0, 0);
	EmitVertex();

	EndPrimitive();
}

void main()
{
	fColor = gs_in[0].color;
	rect(gl_in[0].gl_Position);
}