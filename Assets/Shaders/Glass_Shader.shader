Shader "Custom/Glass_Shader"
{
    Properties
    {
        [MainTexture] _HighlightTex("Highlight Texture", 2D) = "white" {}
        _Strength("Highlight Strength" , Float) = 1.0
        _TintColor("Tint Color", Color) = (1,1,1,1)
        _TintStrength("Tint Strength" ,Float) = 0.2
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent"}

        Pass
        {
            Name "Forward"
            Tags { "LightMode" = "UniversalForward"}
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_HighlightTex);
            SAMPLER(sampler_HighlightTex);

            CBUFFER_START(UnityPerMaterial)
                float _Strength;
                float4 _TintColor;
                float _TintStrength;
                float4 _HighlightTex_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float highlight = SAMPLE_TEXTURE2D(_HighlightTex, sampler_HighlightTex, IN.uv).r;

                float3 baseTint = _TintColor.rgb * _TintStrength;

                float3 baseCol = baseTint * (1.0 -highlight);

                float3 highlightCol = float3(1,1,1) * highlight * _Strength;

                float3 col = baseCol + highlightCol;

                float alpha = (1.0 - highlight) * (_TintStrength * 0.3) + highlight * _Strength;

                return float4(col,alpha);
            }
            ENDHLSL
        }
    }
}
