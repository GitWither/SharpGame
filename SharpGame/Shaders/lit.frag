#version 430
#define MAX_LIGHTS 4

layout(location = 0) in vec2 inTexCoord;
layout(location = 1) in vec3 inNormals;
layout(location = 2) in vec3 inLightVector[MAX_LIGHTS];
layout(location = 7) in vec3 inCameraVector;

layout(location = 0) out vec4 outColor;

layout(binding = 0) uniform sampler2D uTexture;
layout(binding = 1) uniform sampler2D uNormalMap;
layout(binding = 2) uniform sampler2D uEmmissionMap;
 
uniform float u_Specularity;
uniform float u_HasNormalMap;
uniform float u_HasEmmissionMap;
uniform vec3 lightColor[4];
uniform float lightDistance[4];

void main() {
    vec3 normalMapValue = 2.0 * texture(uNormalMap, inTexCoord).rgb - 1.0;

    vec3 unitNormal = vec3(1);
    if (u_HasNormalMap == 0.5) {
        unitNormal = normalize(normalMapValue);
    }
    else {
        unitNormal = normalize(inNormals);
    }
    vec3 unitVectorToCamera = normalize(inCameraVector);


    vec3 totalDiffuse = vec3(0.0);
    vec3 totalSpecular = vec3(0.0);

    for (int i = 0; i < MAX_LIGHTS; i++) {
        float lightDistanceChecked = 0;
        if (lightDistance[i] <= 0.0) {
            lightDistanceChecked = 1;
        }
        else {
            lightDistanceChecked =  (lightDistance[i]);
        }

        vec3 unitLightVector = normalize(inLightVector[i]);

        float nDotl = dot(unitNormal, unitLightVector);
        float brightness = max(nDotl, 0.0);

        vec3 lightDirection = -unitLightVector;
        vec3 reflectedLightDirection = reflect(lightDirection, unitNormal);
        float specularFactor = dot(reflectedLightDirection, unitVectorToCamera);
        specularFactor = max(specularFactor, 0.0);
        float dampedFactor = pow(specularFactor, 10);

        totalDiffuse = totalDiffuse + (brightness * lightColor[i]) / lightDistanceChecked;
        totalSpecular = totalSpecular + (dampedFactor * u_Specularity * lightColor[i]) / lightDistanceChecked;
    }

    totalDiffuse = max(totalDiffuse, 0.2);

    vec4 fragColor = vec4(1.0);
    if (u_HasEmmissionMap == 0.5) {
        fragColor = texture(uEmmissionMap, inTexCoord) + vec4(totalDiffuse, 1.0) * texture(uTexture, inTexCoord) + vec4(totalSpecular, 1.0);
    }
    else {
        fragColor = vec4(totalDiffuse, 1.0) * texture(uTexture, inTexCoord) + vec4(totalSpecular, 1.0);
    }

    outColor = fragColor;
}
