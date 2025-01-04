#version 330 core

struct Light {
    vec3 position;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
}; 
  
in vec3 Normal;
in vec3 FragPos;
in vec3 LightPos;

out vec4 FragColor;
  
uniform Material material;
uniform Light light; 

void main()
{   // diffuse
    vec3 normal = normalize(Normal);
    vec3 lightDir = normalize(LightPos - FragPos);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * material.diffuse;

    // specular
    float specularStrength = 0.5;
    vec3 viewDir = normalize(-FragPos);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 64);
    vec3 specular = light.specular * spec * material.specular; 

    // ambient
    vec3 ambient = light.ambient * material.ambient;

    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);
}