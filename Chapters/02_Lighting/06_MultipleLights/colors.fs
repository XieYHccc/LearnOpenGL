#version 330 core

#define NR_POINT_LIGHTS 4 

struct DirLight {
    vec3 direction;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};  
uniform DirLight u_dirLight;
 
struct PointLight {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};
uniform PointLight u_pointLights[NR_POINT_LIGHTS];

struct SpotLight {
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
uniform SpotLight u_spotLight;

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float     shininess;
}; 
uniform Material u_material;

uniform mat4 u_model;
uniform mat4 u_view;

in vec3 v_Normal;
in vec3 FragPos;
in vec2 v_TexCoords;

out vec4 FragColor;

vec3 CalculateDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
    normal = normalize(normal);
    vec3 lightDir = normalize(mat3(u_view) * light.direction);
    lightDir = -lightDir;

    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), u_material.shininess);
    // combine results
    vec3 ambient = light.ambient * texture(u_material.diffuse, v_TexCoords).rgb;
    vec3 diffuse = light.diffuse * diff * texture(u_material.diffuse, v_TexCoords).rgb;
    vec3 specular = light.specular * spec * texture(u_material.specular, v_TexCoords).rgb;
    return (ambient + diffuse + specular);
}

vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    normal = normalize(normal);
    viewDir = normalize(viewDir);
    vec3 lightPos = vec3(u_view * vec4(light.position, 1.0));
    vec3 lightDir = normalize(lightPos - fragPos);

    // diffuse shading
    float diff = max(dot(normal,lightDir), 0);
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), u_material.shininess);
    // combine results
    vec3 ambient = light.ambient * texture(u_material.diffuse, v_TexCoords).rgb;
    vec3 diffuse = light.diffuse * diff * texture(u_material.diffuse, v_TexCoords).rgb;
    vec3 specular = light.specular * spec * texture(u_material.specular, v_TexCoords).rgb;
    // attenuation
    float distance = length(lightPos - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    return (ambient + diffuse + specular);
}

vec3 CalculateSpotLight(SpotLight spotLight, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    normal = normalize(normal);
    viewDir = normalize(viewDir);
    vec3 lightPos = vec3(u_view * vec4(spotLight.position, 1.0));
    vec3 lightDir = normalize(lightPos - fragPos);
    vec3 spotDir = normalize(mat3(u_view) * spotLight.direction);

    // diffuse shading
    float diff = max(dot(normal,lightDir), 0);
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), u_material.shininess);
    // combine results
    vec3 ambient = spotLight.ambient * texture(u_material.diffuse, v_TexCoords).rgb;
    vec3 diffuse = spotLight.diffuse * diff * texture(u_material.diffuse, v_TexCoords).rgb;
    vec3 specular = spotLight.specular * spec * texture(u_material.specular, v_TexCoords).rgb;
    // attenuation
    float distance = length(lightPos - fragPos);
    float attenuation = 1.0 / (spotLight.constant + spotLight.linear * distance + spotLight.quadratic * (distance * distance));
    diffuse *= attenuation;
    specular *= attenuation;
    ambient *= attenuation;

    // spotlight (soft edges)
    float theta = dot(lightDir, -spotDir);
    float epsilon = spotLight.cutOff - spotLight.outerCutOff;
    float intensity = clamp((theta - spotLight.outerCutOff) / epsilon, 0.0, 1.0);
    diffuse *= intensity;
    specular *= intensity;

    return (ambient + diffuse + specular);

}

void main()
{   
    // properties
    vec3 norm = normalize(v_Normal);
    vec3 viewDir = normalize(-FragPos);

    // phase 1: directional lighting
    vec3 result = CalculateDirLight(u_dirLight, norm, viewDir);
    // phase 2: point lights
    for(int i = 0; i < NR_POINT_LIGHTS; i++)
        result += CalculatePointLight(u_pointLights[i], norm, FragPos, viewDir);
    // phase 3: spot light
    result += CalculateSpotLight(u_spotLight, norm, FragPos, viewDir);

    FragColor = vec4(result, 1.0);
}