Shader "Custom/Toon"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Intensity ("Intensity", Range(0, 1)) = 1
        _Ramp("Ramp", 2D)  = "white"{}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
    
        #pragma surface surf Ramp nofog noforwardadd nolightmap nodynlightmap
    
        half _Intensity;
        sampler2D _Ramp;
    
        half4 LightingRamp (SurfaceOutput o, half3 lightDir, half atten) 
        {
            half NdotL = dot(o.Normal, lightDir);
            half diff = NdotL * 0.5 + 0.5;
            half3 ramp = tex2D(_Ramp, half2(diff, 1)).rgb;
            return half4(o.Albedo * ramp * atten * _Intensity, 1);
        }
    
        struct Input 
        {
            float2 uv_MainTex;
        };
        
        sampler2D _MainTex;
        
        void surf (Input IN, inout SurfaceOutput o) 
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}
