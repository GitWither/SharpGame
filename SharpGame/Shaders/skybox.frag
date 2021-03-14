#version 430

layout(location = 0) in vec3 inTexCoord;

layout(location = 0) out vec4 outColor;

layout(binding = 0) uniform samplerCube uTexture;

void main() {

    outColor = texture(uTexture, inTexCoord);   
}
