#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

uniform mat4 u_model;
uniform mat4 u_view;
uniform mat4 u_projection;

out vec3 v_Normal;
out vec3 FragPos;
out vec2 v_TexCoords;

void main()
{
	mat4 viewModel = u_view * u_model;
	gl_Position = u_projection * viewModel * vec4(aPos, 1.0);
	FragPos = vec3(viewModel * vec4(aPos, 1.0));
	v_Normal = mat3(transpose(inverse(viewModel))) * aNormal;  
	v_TexCoords = aTexCoords;
}