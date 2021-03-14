#version 430

layout(location = 0) in vec3 inPosition;
layout(location = 1) in vec2 inTexCoord;

layout(location = 0) out vec2 outTexCoord;

uniform mat4 u_Translation;
uniform mat4 u_Projection;
uniform mat4 u_View;


void main() {

    vec4 worldPosition = u_Translation * vec4(inPosition, 1);

    gl_Position = u_Projection * u_View * u_Translation * worldPosition;

    outTexCoord = inTexCoord;
}