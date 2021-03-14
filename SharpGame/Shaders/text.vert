#version 430

layout(location = 0) in vec3 inPosition;
layout(location = 1) in vec2 inTexCoord;

layout(location = 0) out vec2 outTexCoord;

uniform mat4 u_Translation;


void main() {
    vec4 worldPosition = u_Translation * vec4(inPosition.x, inPosition.y, inPosition.z, 1);

    gl_Position = worldPosition;

    outTexCoord = inTexCoord;
}