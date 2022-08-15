#version 430

layout(location = 0) in vec3 inPosition;
layout(location = 1) in vec2 inTexCoord;

layout(location = 0) out vec2 outTexCoord;

uniform mat4 u_Translation;
uniform mat4 u_ViewProjection;


void main() {
    gl_Position = u_ViewProjection * u_Translation * vec4(inPosition, 1);

    outTexCoord = inTexCoord;
}