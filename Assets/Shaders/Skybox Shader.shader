Shader "Custom/Skybox Shader"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (1, 1, 1, 0)
        _HorizonColor ("Horizon Color", Color) = (1, 1, 1, 0)
        _BottomColor ("Bottom Color", Color) = (1, 1, 1, 0)
        _TopExponent ("Exponent Factor for Top Half", Float) = 1.0
        _BottomExponent ("Exponent Factor for Bottom Half", Float) = 1.0
        _Intensity ("Intensity Amplifier", Float) = 1.0
        _Angle ("Angle", Float) = 0.0
    }
    
    CGINCLUDE

    #include "UnityCG.cginc"

    struct appdata
    {
        float4 position : POSITION;
        float3 texcoord : TEXCOORD0;
    };

    struct v2f
    {
        float4 position : SV_POSITION;
        float3 texcoord : TEXCOORD0;
    };

    half4 _TopColor;
    half4 _HorizonColor;
    half4 _BottomColor;
    half _TopExponent;
    half _BottomExponent;
    half _Intensity;
    half _Angle;

    v2f vert (appdata v)
    {
        v2f o;
        float sinX = sin (_Angle);
        float cosX = cos (_Angle);
        float sinY = sin (_Angle);
        float2x2 rotationMatrix = float2x2(cosX, -sinX, sinY, cosX);
        o.position = UnityObjectToClipPos (v.position);
        o.texcoord.xz = mul(v.texcoord.xz, rotationMatrix);
        o.texcoord.y = v.texcoord.y;
        return o;
    }

    half4 frag (v2f i) : COLOR
    {
        float p = normalize (i.texcoord).y;
        float p1 = 1.0f - pow (min(1.0f, 1.0f - p), _TopExponent);
        float p3 = 1.0f - pow (min(1.0f, 1.0f + p), _BottomExponent);
        float p2 = 1.0f - p1 - p3;
        return (_TopColor * p1 + _HorizonColor * p2 + _BottomColor * p3) * _Intensity;
    }

    ENDCG
    
    SubShader
    {
        Tags { "RenderType"="Background" "Queue"="Background" }
        Pass
        {
            ZWrite Off
            Cull Off
            Fog { Mode Off }
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
}