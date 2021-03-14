#version 430

layout(location = 0) in vec3 inPosition;
layout(location = 1) in vec2 inTexCoord;
layout(location = 2) in vec3 inOffset;

layout(location = 0) out vec2 outTexCoord;

uniform mat4 u_Translation;
uniform mat4 u_Projection;
uniform mat4 u_View;

void main() {
    vec3 cameraRight = vec3(u_View[0][0], u_View[1][0], u_View[2][0]);
    vec3 cameraUp = vec3(u_View[0][1], u_View[1][1], u_View[2][1]);

    vec3 pos = cameraRight * inPosition.x + cameraUp * inPosition.y;

    gl_Position = u_Projection * u_View * u_Translation * vec4(pos + inOffset, 1);

    outTexCoord = inTexCoord;
}