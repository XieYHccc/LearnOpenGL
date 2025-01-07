#version 330 core

struct Light {
    vec3 position;  
    vec3 direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
}; 

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float     shininess;
}; 

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

out vec4 FragColor;

uniform mat4 model;
uniform mat4 view;
uniform Material material;
uniform Light light; 

void main()
{   
    vec3 lightPos = vec3(view * vec4(light.position, 1.0));
    vec3 lightDir = normalize(lightPos - FragPos);
    vec3 spotLightDir = normalize(mat3(view) * light.direction);
    vec3 normal = normalize(Normal);

    // diffuse
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoords).rgb; 

    // specular
    float specularStrength = 0.5;
    vec3 viewDir = normalize(-FragPos);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 64);
    vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb; 

    // ambient
    vec3 ambient = light.ambient * texture(material.diffuse, TexCoords).rgb;

    // spotlight intensity 
    float theta = dot(normalize(spotLightDir), -lightDir);
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
    diffuse *= intensity;
    specular *= intensity;

    // attenuation
    float distance = length(lightPos - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
    diffuse *= attenuation;
    specular *= attenuation;

    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);


}