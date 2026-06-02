Shader "Custom/UnderwaterDistortion"
{
    Properties
    {
        _NormalMap("Normal Map", 2D) = "bump" {}
        _DistortionStrength("Distortion Strength", Range(0,0.1)) = 0.02
        _ScrollSpeedX("Scroll Speed X", Float) = 0.1
        _ScrollSpeedY("Scroll Speed Y", Float) = 0.05
        _NormalTiling("Normal Tiling", Float) = 1
        _TintColor("Tint Color", Color) = (0,0.4,0.6,1)
        _TintStrength("Tint Strength", Range(0,1)) = 0.15
    }

    SubShader
    {
        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

        TEXTURE2D(_NormalMap);
        SAMPLER(sampler_NormalMap);

        float _DistortionStrength;
        float _ScrollSpeedX;
        float _ScrollSpeedY;
        float _NormalTiling;

        float4 _TintColor;
        float _TintStrength;

        ENDHLSL

        Pass
        {
            Name "UnderwaterDistortion"

            ZWrite Off
            Cull Off

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            float4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;

                float2 normalUV =
                    uv * _NormalTiling +
                    float2(
                        _Time.y * _ScrollSpeedX,
                        _Time.y * _ScrollSpeedY
                    );

                float3 normal =
                    UnpackNormal(
                        SAMPLE_TEXTURE2D(
                            _NormalMap,
                            sampler_NormalMap,
                            normalUV
                        )
                    );

                float2 distortion =
                    normal.xy * _DistortionStrength;

                float2 distortedUV =
                    uv + distortion;

                float4 color =
                    SAMPLE_TEXTURE2D(
                        _BlitTexture,
                        sampler_LinearClamp,
                        distortedUV
                    );

                color.rgb =
                    lerp(
                        color.rgb,
                        _TintColor.rgb,
                        _TintStrength
                    );

                return color;
            }

            ENDHLSL
        }
    }
}