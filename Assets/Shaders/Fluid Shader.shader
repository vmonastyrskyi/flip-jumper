Shader "Custom/Fluid Shader"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (1, 1, 1, 1)
        _DepthColor ("Depth Color", Color) = (1, 1, 1, 1)
        _DepthMaxDistance ("Depth Max Distance", Float) = 1.0
        _FlowStrength ("Flow Strength", Float ) = 0.01
        _FlowSpeed ("Flow Speed", Float ) = 0.01
        _Size ("Size", Float ) = 2
        _FluidColor ("Fluid Color", Color) = (1,1,1,1)
        [HDR]
        _LightFoamColor ("Light Foam Color", Color) = (1,1,1,1)
        _DarkFoamColor ("Dark Foam Color", Color) = (1,1,1,1)
        _MainTexture ("Main Texture", 2D) = "white" {}
        _Normal ("Normal", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
            };

            uniform sampler2D _CameraDepthTexture;
            half4 _TopColor;
            half4 _DepthColor;
            half _DepthMaxDistance;
            half _FlowStrength;
            half _FlowSpeed;
            half _Size;
            half4 _FluidColor;
            half4 _LightFoamColor;
            half4 _DarkFoamColor;
            sampler2D _MainTexture;
            half4 _MainTexture_ST;
            sampler2D _Normal;
            half4 _Normal_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTexture);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half2 flow = (i.uv + (_Time.g * (_FlowSpeed / _Size)));
                half3 normal = UnpackNormal(tex2D(_Normal, TRANSFORM_TEX(flow, _Normal)));
                half2 flowEffect = ((float3(i.uv, 0.0) + (normal.rgb * _FlowStrength)) * _Size).rg;
                half2 flowEffectWithOffset = (float3(flowEffect, 0.0) + half3(0.075, -0.075, 0)).rg;
                half4 flowTexture = tex2D(_MainTexture, TRANSFORM_TEX(flowEffect, _MainTexture));
                half4 _MainTexture2 = tex2D(_MainTexture, TRANSFORM_TEX(flowEffect, _MainTexture));
                half4 _MainTexture3 = tex2D(_MainTexture, TRANSFORM_TEX(flowEffectWithOffset, _MainTexture));
                half4 emissive = fixed4(lerp(lerp(_FluidColor.rgb, _DarkFoamColor.rgb, _MainTexture3.rgb), _LightFoamColor.rgb, _MainTexture2.rgb), 1);
            
                half depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                half depthDifference = depth - i.screenPos.w;
                half fluidDepthDifference = saturate(depthDifference / _DepthMaxDistance);
                half4 col = lerp(emissive + _TopColor, _DepthColor, fluidDepthDifference);
                return col;
            }
            ENDCG
        }
    }
}
