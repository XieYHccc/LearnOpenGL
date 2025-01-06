#version 330 core

struct Light {
    vec3 position;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    sampler2D emission;
    float     shininess;
}; 

in vec3 Normal;
in vec3 FragPos;
in vec3 LightPos;
in vec2 TexCoords;

out vec4 FragColor;
  
uniform Material material;
uniform Light light; 

void main()
{   // diffuse
    vec3 normal = normalize(Normal);
    vec3 lightDir = normalize(LightPos - FragPos);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords)); 

    // specular
    float specularStrength = 0.5;
    vec3 viewDir = normalize(-FragPos);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 64);
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords)); 

    // ambient
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));

    // emission
    vec3 emission = vec3(texture(material.emission, TexCoords));

    vec3 result = ambient + diffuse + specular + emission;
    FragColor = vec4(result, 1.0);
}