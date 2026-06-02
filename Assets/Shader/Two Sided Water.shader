Shader "Custom/URP_TwoSidedWater"
{
    Properties
    {
        _Color ("Water Color", Color) = (0.0, 0.4, 0.8, 0.7)
        
        [NoScaleOffset] _NormalMap ("Wave Normal Map", 2D) = "bump" {}
        _NormalStrength ("Wave Strength", Range(0.0, 2.0)) = 1.0
        _NormalScale ("Wave Scale", Float) = 1.0
        
        _WaveSpeed1 ("Wave Speed 1 (X,Y)", Vector) = (0.05, 0.05, 0, 0)
        _WaveSpeed2 ("Wave Speed 2 (X,Y)", Vector) = (-0.03, 0.07, 0, 0)
        
        _Smoothness ("Smoothness", Range(0.0, 1.0)) = 0.95
        _Metallic ("Metallic", Range(0.0, 1.0)) = 0.1
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            // Standard Alpha Blending
            Blend SrcAlpha OneMinusSrcAlpha
            // Turn off depth writing for proper transparency
            ZWrite Off
            // CRITICAL: Disables culling to render top and bottom sides
            Cull Off

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            // URP Lighting and Shadow Keywords (Required for proper rendering in URP)
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fog

            // URP Shader Libraries
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // Textures
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);

            // CBUFFER is required in URP for the SRP Batcher to work efficiently
            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _NormalStrength;
                float _NormalScale;
                float4 _WaveSpeed1;
                float4 _WaveSpeed2;
                float _Smoothness;
                float _Metallic;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionWS   : TEXCOORD0;
                
                // TBN Matrix variables for Normal Mapping
                float3 normalWS     : TEXCOORD1;
                float3 tangentWS    : TEXCOORD2;
                float3 bitangentWS  : TEXCOORD3;
                
                // Fog and Shadows
                float fogCoord      : TEXCOORD4;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                // Position transformations
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionCS = TransformWorldToHClip(output.positionWS);

                // Normal/Tangent transformations
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                output.normalWS = normalInput.normalWS;
                output.tangentWS = normalInput.tangentWS;
                output.bitangentWS = normalInput.bitangentWS;

                output.fogCoord = ComputeFogFactor(output.positionCS.z);

                return output;
            }

            // The SV_IsFrontFace semantic gives us a bool indicating top (true) or bottom (false)
            half4 frag(Varyings input, bool isFrontFace : SV_IsFrontFace) : SV_Target
            {
                // 1. Calculate World Space UVs
                float2 baseUV = input.positionWS.xz * _NormalScale;

                // 2. Pan UVs over time for two overlapping wave layers
                float2 uv1 = baseUV + (_Time.y * _WaveSpeed1.xy);
                float2 uv2 = baseUV + (_Time.y * _WaveSpeed2.xy);

                // 3. Sample and unpack normal maps
                half4 tex1 = SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, uv1);
                half4 tex2 = SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, uv2);
                float3 normal1 = UnpackNormalScale(tex1, _NormalStrength);
                float3 normal2 = UnpackNormalScale(tex2, _NormalStrength);

                // 4. Blend normal maps (Tangent Space)
                float3 blendedNormalTS = normalize(float3(normal1.xy + normal2.xy, normal1.z * normal2.z));

                // 5. Convert Tangent Space Normal to World Space Normal
                float3x3 tangentToWorld = float3x3(input.tangentWS, input.bitangentWS, input.normalWS);
                float3 finalNormalWS = TransformTangentToWorld(blendedNormalTS, tangentToWorld);
                finalNormalWS = normalize(finalNormalWS);

                // 6. TWO-SIDED LOGIC: Flip the normal if viewing from underneath
                if (!isFrontFace)
                {
                    finalNormalWS = -finalNormalWS;
                }

                // 7. Setup URP Surface Data (Albedo, Metallic, etc.)
                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = _Color.rgb;
                surfaceData.alpha = _Color.a;
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = _Smoothness;
                surfaceData.normalTS = blendedNormalTS; 
                surfaceData.emission = float3(0, 0, 0);
                surfaceData.occlusion = 1.0;

                // 8. Setup URP Input Data (Lighting, Shadows, View Direction)
                InputData inputData = (InputData)0;
                inputData.positionWS = input.positionWS;
                inputData.normalWS = finalNormalWS;
                inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                // Get shadow coordinates
                #if defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                    inputData.shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                #else
                    inputData.shadowCoord = float4(0, 0, 0, 0);
                #endif

                // 9. Calculate final physically based lighting (PBR)
                half4 color = UniversalFragmentPBR(inputData, surfaceData);
                
                // Add URP Fog
                color.rgb = MixFog(color.rgb, input.fogCoord);
                
                // Preserve transparency
                color.a = surfaceData.alpha;

                return color;
            }
            ENDHLSL
        }
    }
    // Fallback allows it to cast shadows if needed, though water usually doesn't need to.
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}