Shader "Custom/Toon_Shader"
{
    Properties
    {
        [MainTexture] _PaletteTex("Color Palette", 2D) = "white" {}
        _StepCount("Toon Steps", Float) = 5.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }

        Pass{
            Name "ShadowCaster"
            Tags{ "LightMode" = "ShadowCaster"}

            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "ForwardLit"
            Tags {"LightMode" = "UniversalForward"}
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION; // OS means object-space
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION; // CS means clip-space
                float3 normalWS : TEXCOORD0; // WS means world-space
                float3 positionWS : TEXCOORD1;
                float2 uv : TEXCOORD3;
            };

            TEXTURE2D(_PaletteTex);
            SAMPLER(sampler_PaletteTex);

            CBUFFER_START(UnityPerMaterial)
                float _StepCount;
                float4 _PaletteTex_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 normal = IN.normalWS;
                
                // Lấy main light
                float4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                Light mainLight = GetMainLight(shadowCoord);

                float mainDot = dot(normal, mainLight.direction);

                float mainIntensity = saturate(mainDot) * mainLight.distanceAttenuation;

                // remap về 0-1 và chia bậc ánh sáng
                float scaled = mainIntensity * _StepCount;

                float baseSteps = floor(scaled);
                float nextSteps = min(baseSteps + 1.0, _StepCount - 1.0);

                float t = frac(scaled);

                float w = fwidth(scaled);
                float smoothT = smoothstep(0.5 - w, 0.5 + w, t);

                float shadow = mainLight.shadowAttenuation;
                float shadowEffect = (1.0 - shadow) * 1.0;

                baseSteps -= shadowEffect;
                nextSteps -= shadowEffect;

                baseSteps = clamp(baseSteps, 0.0, _StepCount - 1.0);
                nextSteps = clamp(nextSteps, 0.0, _StepCount - 1.0);

                // xử lý UV Pallete
                float colWidth = 1.0 / _StepCount;

                float2 uvA = float2(baseSteps * colWidth + colWidth * 0.5, IN.uv.y);
                float2 uvB = float2(nextSteps * colWidth + colWidth * 0.5, IN.uv.y);

                half3 colA = SAMPLE_TEXTURE2D(_PaletteTex, sampler_PaletteTex, uvA).rgb;
                half3 colB = SAMPLE_TEXTURE2D(_PaletteTex, sampler_PaletteTex, uvB).rgb;

                half3 baseCol = lerp(colA, colB, smoothT);

                float3 ambient = SampleSH(normalize(normal)) * 0.5;

                baseCol += ambient *(1.0 - mainIntensity);

                // xử lý additional light
                float3 additionalLightSum = 0;

                #if defined(_ADDITIONAL_LIGHTS)
                uint pixelLightCount = GetAdditionalLightsCount();

                for(uint i = 0; i < pixelLightCount; ++i)
                {
                    Light addLight = GetAdditionalLight(i, IN.positionWS);

                    float3 lightDir = normalize(addLight.direction);
                    float addDot = saturate(dot(normal, lightDir));

                    float attenuation = addLight.distanceAttenuation;

                    float3 lightColor = addLight.color * addDot * attenuation;

                    additionalLightSum += lightColor;
                }
                #endif

                half3 finalCol = baseCol * (1.0 + additionalLightSum);

                return half4(finalCol, 1.0);
                
            }
            ENDHLSL
        }
    }
}
