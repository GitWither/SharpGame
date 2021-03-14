#version 430

layout(location = 0) in vec3 inPosition;

layout(location = 0) out vec3 outTexCoord;

uniform mat4 u_View;
uniform mat4 u_Projection;

void main() {

    outTexCoord = inPosition;
    vec4 pos = u_Projection * u_View * vec4(inPosition, 1);
    gl_Position = pos.xyww;
}