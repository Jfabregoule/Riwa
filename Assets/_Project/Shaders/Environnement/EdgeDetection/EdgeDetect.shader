Shader "Custom/EdgeDetect"
{
    Properties
    {
        _EdgeColor ("Edge Color", Color) = (0, 0, 0, 1)
        _Threshold ("Edge Threshold", Range(0.001, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline" }
        Pass
        {
            Name "EdgeDetect"
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _EdgeColor;
            float _Threshold;

            TEXTURE2D(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);
            TEXTURE2D(_CameraNormalsTexture);
            SAMPLER(sampler_CameraNormalsTexture);
            TEXTURE2D(_CameraColorTexture);
            SAMPLER(sampler_CameraColorTexture);

         

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // base image
                float4 original = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uv);

                // edge detection
                float3 n = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, uv).rgb;
                float3 nX = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, uv + float2(_ScreenParams.z, 0)).rgb;
                float3 nY = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, uv + float2(0, _ScreenParams.w)).rgb;

                float diffX = distance(n, nX);
                float diffY = distance(n, nY);
                float edge = step(_Threshold, diffX + diffY);

                float4 edgeColor = edge * _EdgeColor;

                return original + edgeColor; // combine image + outline
            }
            ENDHLSL
        }
    }
}
