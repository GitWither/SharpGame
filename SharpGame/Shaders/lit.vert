#version 430
#define MAX_LIGHTS 4

layout(location = 0) in vec3 inPosition;
layout(location = 1) in vec2 inTexCoord;
layout(location = 2) in vec3 inNormals;

layout(location = 0) out vec2 outTexCoord;
layout(location = 1) out vec3 outNormals;
layout(location = 2) out vec3 outLightVector[MAX_LIGHTS];
layout(location = 7) out vec3 outCameraVector;

uniform mat4 u_Translation;
uniform mat4 u_Projection;
uniform mat4 u_View;

uniform vec3 lightPosition[MAX_LIGHTS];


void main() {

    vec4 worldPosition = u_Translation * vec4(inPosition, 1);

    mat4 mvp = u_Projection * u_View * u_Translation;

    gl_Position = mvp * worldPosition;

    outNormals = (u_Translation * vec4(inNormals, 0.0)).xyz;
    outCameraVector = (inverse(mvp) * vec4(0.0, 0.0, 0.0, 1.0)).xyz - worldPosition.xyz;


    for (int i = 0; i < MAX_LIGHTS; i++) {
        outLightVector[i] = lightPosition[i] - worldPosition.xyz;
    }

    outTexCoord = inTexCoord;
}