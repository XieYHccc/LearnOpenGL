#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 lightPos; 

out vec3 Normal;
out vec3 FragPos;
out vec3 LightPos;
out vec2 TexCoords;

void main()
{
	gl_Position = projection * view * model * vec4(aPos, 1.0);
	mat4 viewModel = view * model;
	FragPos = vec3(viewModel * vec4(aPos, 1.0));
	LightPos = vec3(view * vec4(lightPos, 1.0));
	Normal = mat3(transpose(inverse(viewModel))) * aNormal;  
	TexCoords = aTexCoords;
}