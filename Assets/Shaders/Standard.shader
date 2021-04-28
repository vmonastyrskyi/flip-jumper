Shader "Custom/Standard"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Intensity ("Intensity", Range(0, 1)) = 1
        
        [MaterialToggle] _Rim("Rim", Float) = 0
        _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _RimPower ("Rim Power", Range(0, 8)) = 4.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf WrapLambert nofog noforwardadd nolightmap nodynlightmap
    
        half _Intensity;
    
        half4 LightingWrapLambert(SurfaceOutput o, half3 lightDir, half atten)
        {
            half NdotL = dot(o.Normal, lightDir);
            half diff = NdotL * 0.5 + 0.5;
            return half4(o.Albedo * (diff * atten) * _Intensity, 1);
        }
    
        struct Input 
        {
            half2 uv_MainTex;
            float3 viewDir;
        };
        
        sampler2D _MainTex;
        
        half _Rim;
        half4 _RimColor;
        half _RimPower;
        
        void surf(Input IN, inout SurfaceOutput o) 
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            
            if (_Rim)
            {
                half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
                o.Emission = _RimColor.rgb * pow (rim, _RimPower);
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
