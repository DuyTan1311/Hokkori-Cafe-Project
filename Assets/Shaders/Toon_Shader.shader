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
                Light mainLight = GetMainLight();
                float mainDot = dot(normal, mainLight.direction);

                float mainIntensity = saturate(mainDot) * mainLight.distanceAttenuation;

                // remap về 0-1 và chia bậc ánh sáng
                float steps = floor(mainIntensity * _StepCount);
                steps = min(steps, _StepCount - 1.0);

                // xử lý UV Pallete
                float2 finalUV;
                finalUV.y = IN.uv.y;
                float colWidth = 1.0/ _StepCount;
                finalUV.x = (steps * colWidth) + (colWidth * 0.5);

                // xử lý màu gốc
                half3 baseCol = SAMPLE_TEXTURE2D(_PaletteTex, sampler_PaletteTex, finalUV).rgb;

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

                half3 finalCol = baseCol + additionalLightSum;

                return half4(finalCol, 1.0);
                
            }
            ENDHLSL
        }
    }
}
