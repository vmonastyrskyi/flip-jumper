Shader "Custom/Bubble Shader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.03
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_BaseColor("BaseColor", Color) = (0, 0, 0, 0)
		_InsideColor("InsideColor", Color) = (0, 0, 0, 0)
	}

	SubShader
	{
		Tags 
		{ 
            "RenderType" = "TransparentCutout"
            "Queue" = "AlphaTest+0"
            "IsEmissive" = "true"  
        }
        
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow nolightmap vertex:vertexDataFunc 
		
		struct Input
		{
			half3 worldPos;
			half3 worldNormal;
			half4 vertexColor : COLOR;
			half2 uv_texcoord;
		};

		uniform half4 _InsideColor;
		uniform half4 _BaseColor;
		uniform sampler2D _TextureSample0;
		uniform float _Cutoff = 0.03;

		void vertexDataFunc(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			half3 normalXYZ = v.normal.xyz;
			half3 appendResult = (half3(normalXYZ.x, 0.0, normalXYZ.z));
			half4 output = (((v.color * 2) + -1.0) + v.color.a);
			half4 lerpResult = lerp(half4(1, 1, 1, 0), half4(0, 0, 0, 1), saturate(output));
			half3 vertexXYZ = v.vertex.xyz;
			v.vertex.xyz = (((half4(appendResult, 0.0) * (1.0 - lerpResult)) * half4(0.1981132,0.1981132,0.1981132,0)) + half4(vertexXYZ, 0.0)).rgb;
		}

		void surf(Input i, inout SurfaceOutputStandard o)
		{
			float fresnelNdotV = dot(i.worldNormal, normalize(UnityWorldSpaceViewDir(i.worldPos)));
			float fresnel = (0.3 + 0.3 * pow(1.0 - fresnelNdotV, 1.0));
			o.Emission = ((fresnel * _InsideColor) + _BaseColor).rgb;
			o.Smoothness = 1.0;
			o.Alpha = 1;
			half4 output = (((i.vertexColor * 2) + -1.0) + i.vertexColor.a);
			half4 lerpResult = lerp(half4(1, 1, 1, 0) , half4(0, 0, 0, 1) , saturate(output));
			clip((lerpResult * ((1.0 - output) * tex2D(_TextureSample0, i.uv_texcoord))).r - _Cutoff);
		}

		ENDCG
	}
}