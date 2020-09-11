// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32817,y:32301,varname:node_3138,prsc:2|emission-9747-OUT,voffset-2125-OUT;n:type:ShaderForge.SFN_Time,id:394,x:34638,y:32590,varname:node_394,prsc:2;n:type:ShaderForge.SFN_Divide,id:6728,x:34638,y:32716,varname:node_6728,prsc:2|A-9921-OUT,B-555-OUT;n:type:ShaderForge.SFN_ValueProperty,id:555,x:34804,y:32801,ptovrint:False,ptlb:Size,ptin:_Size,varname:node_555,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:9921,x:34804,y:32716,ptovrint:False,ptlb:Flow Speed,ptin:_FlowSpeed,varname:node_9921,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.01;n:type:ShaderForge.SFN_Multiply,id:9292,x:34480,y:32716,varname:node_9292,prsc:2|A-394-T,B-6728-OUT;n:type:ShaderForge.SFN_TexCoord,id:4330,x:34480,y:32566,varname:node_4330,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:8592,x:34329,y:32665,varname:node_8592,prsc:2|A-4330-UVOUT,B-9292-OUT;n:type:ShaderForge.SFN_Tex2d,id:3191,x:34166,y:32584,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:node_3191,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a8860e2076ef84f40b7a77b7a84de899,ntxv:0,isnm:True|UVIN-8592-OUT;n:type:ShaderForge.SFN_Multiply,id:5274,x:33993,y:32574,varname:node_5274,prsc:2|A-3191-RGB,B-6653-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6653,x:33993,y:32723,ptovrint:False,ptlb:Flow Strength,ptin:_FlowStrength,varname:node_6653,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.01;n:type:ShaderForge.SFN_TexCoord,id:732,x:33946,y:32341,varname:node_732,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:6885,x:33753,y:32341,varname:node_6885,prsc:2|A-732-UVOUT,B-5274-OUT;n:type:ShaderForge.SFN_Multiply,id:7187,x:33573,y:32341,varname:node_7187,prsc:2|A-6885-OUT,B-555-OUT;n:type:ShaderForge.SFN_Tex2d,id:7653,x:33399,y:32341,ptovrint:False,ptlb:Main Texture,ptin:_MainTexture,varname:node_7653,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6f1fe35187b9fb8479f0fe82fa92aa9d,ntxv:0,isnm:False|UVIN-556-OUT;n:type:ShaderForge.SFN_Color,id:6032,x:33136,y:31997,ptovrint:False,ptlb:Water Color,ptin:_WaterColor,varname:node_6032,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2078432,c2:0.6431373,c3:1,c4:1;n:type:ShaderForge.SFN_Color,id:3604,x:33285,y:31997,ptovrint:False,ptlb:Dark Foam Color,ptin:_DarkFoamColor,varname:node_3604,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.5251782,c3:1,c4:1;n:type:ShaderForge.SFN_Lerp,id:1066,x:33155,y:32153,varname:node_1066,prsc:2|A-6032-RGB,B-3604-RGB,T-8516-RGB;n:type:ShaderForge.SFN_ComponentMask,id:556,x:33573,y:32486,varname:node_556,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-7187-OUT;n:type:ShaderForge.SFN_Add,id:464,x:33543,y:32083,varname:node_464,prsc:2|A-556-OUT,B-9028-OUT;n:type:ShaderForge.SFN_Vector3,id:9028,x:33719,y:32083,varname:node_9028,prsc:2,v1:0.075,v2:-0.075,v3:0;n:type:ShaderForge.SFN_Tex2d,id:8516,x:33399,y:32136,ptovrint:False,ptlb:Main Texture2,ptin:_MainTexture2,varname:_MainTexture_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6f1fe35187b9fb8479f0fe82fa92aa9d,ntxv:3,isnm:False|UVIN-7369-OUT;n:type:ShaderForge.SFN_ComponentMask,id:7369,x:33543,y:31938,varname:node_7369,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-464-OUT;n:type:ShaderForge.SFN_Lerp,id:9747,x:33252,y:32555,varname:node_9747,prsc:2|A-1066-OUT,B-6718-RGB,T-7653-RGB;n:type:ShaderForge.SFN_Color,id:6718,x:33155,y:32288,ptovrint:False,ptlb:Light Foam Color,ptin:_LightFoamColor,varname:node_6718,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_FragmentPosition,id:4663,x:32926,y:32023,varname:node_4663,prsc:2;n:type:ShaderForge.SFN_ComponentMask,id:9263,x:32737,y:32056,varname:node_9263,prsc:2,cc1:0,cc2:2,cc3:-1,cc4:-1|IN-4663-XYZ;n:type:ShaderForge.SFN_Add,id:1376,x:32548,y:32066,varname:node_1376,prsc:2|A-9263-R,B-9263-G;n:type:ShaderForge.SFN_Add,id:5069,x:32538,y:32202,varname:node_5069,prsc:2|A-1376-OUT,B-1843-T;n:type:ShaderForge.SFN_Time,id:1843,x:32701,y:32202,varname:node_1843,prsc:2;n:type:ShaderForge.SFN_Sin,id:5191,x:32385,y:32202,varname:node_5191,prsc:2|IN-5069-OUT;n:type:ShaderForge.SFN_Multiply,id:3750,x:32221,y:32202,varname:node_3750,prsc:2|A-5191-OUT,B-5243-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5243,x:32385,y:32367,ptovrint:False,ptlb:Choppiness,ptin:_Choppiness,varname:node_5243,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.05;n:type:ShaderForge.SFN_ObjectPosition,id:9943,x:32068,y:32050,varname:node_9943,prsc:2;n:type:ShaderForge.SFN_Add,id:2125,x:31917,y:32478,varname:node_2125,prsc:2|A-9943-XYZ,B-1849-OUT;n:type:ShaderForge.SFN_Vector1,id:7590,x:32221,y:32329,varname:node_7590,prsc:2,v1:0;n:type:ShaderForge.SFN_Append,id:7085,x:32068,y:32202,varname:node_7085,prsc:2|A-7590-OUT,B-3750-OUT;n:type:ShaderForge.SFN_Append,id:1849,x:31915,y:32202,varname:node_1849,prsc:2|A-7085-OUT,B-7590-OUT;proporder:6653-9921-555-7653-6032-3604-3191-8516-6718-5243;pass:END;sub:END;*/

Shader "Custom/Fluid Shader" {
    Properties {
        _FlowStrength ("Flow Strength", Float ) = 0.01
        _FlowSpeed ("Flow Speed", Float ) = 0.01
        _Size ("Size", Float ) = 2
        _MainTexture ("Main Texture", 2D) = "white" {}
        _WaterColor ("Water Color", Color) = (0.2078432,0.6431373,1,1)
        _DarkFoamColor ("Dark Foam Color", Color) = (0,0.5251782,1,1)
        _Normal ("Normal", 2D) = "white" {}
        _MainTexture2 ("Main Texture2", 2D) = "bump" {}
        _LightFoamColor ("Light Foam Color", Color) = (1,1,1,1)
        _Choppiness ("Choppiness", Float ) = 0.05
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma target 3.0
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform sampler2D _MainTexture2; uniform float4 _MainTexture2_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _Size)
                UNITY_DEFINE_INSTANCED_PROP( float, _FlowSpeed)
                UNITY_DEFINE_INSTANCED_PROP( float, _FlowStrength)
                UNITY_DEFINE_INSTANCED_PROP( float4, _WaterColor)
                UNITY_DEFINE_INSTANCED_PROP( float4, _DarkFoamColor)
                UNITY_DEFINE_INSTANCED_PROP( float4, _LightFoamColor)
                UNITY_DEFINE_INSTANCED_PROP( float, _Choppiness)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float node_7590 = 0.0;
                float2 node_9263 = mul(unity_ObjectToWorld, v.vertex).rgb.rb;
                float4 node_1843 = _Time;
                float _Choppiness_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Choppiness );
                v.vertex.xyz += (objPos.rgb+float3(float2(node_7590,(sin(((node_9263.r+node_9263.g)+node_1843.g))*_Choppiness_var)),node_7590));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
////// Lighting:
////// Emissive:
                float4 _WaterColor_var = UNITY_ACCESS_INSTANCED_PROP( Props, _WaterColor );
                float4 _DarkFoamColor_var = UNITY_ACCESS_INSTANCED_PROP( Props, _DarkFoamColor );
                float4 node_394 = _Time;
                float _FlowSpeed_var = UNITY_ACCESS_INSTANCED_PROP( Props, _FlowSpeed );
                float _Size_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Size );
                float2 node_8592 = (i.uv0+(node_394.g*(_FlowSpeed_var/_Size_var)));
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(node_8592, _Normal)));
                float _FlowStrength_var = UNITY_ACCESS_INSTANCED_PROP( Props, _FlowStrength );
                float3 node_5274 = (_Normal_var.rgb*_FlowStrength_var);
                float2 node_556 = ((float3(i.uv0,0.0)+node_5274)*_Size_var).rg;
                float2 node_7369 = (float3(node_556,0.0)+float3(0.075,-0.075,0)).rg;
                float4 _MainTexture2_var = tex2D(_MainTexture2,TRANSFORM_TEX(node_7369, _MainTexture2));
                float4 _LightFoamColor_var = UNITY_ACCESS_INSTANCED_PROP( Props, _LightFoamColor );
                float4 _MainTexture_var = tex2D(_MainTexture,TRANSFORM_TEX(node_556, _MainTexture));
                float3 node_9747 = lerp(lerp(_WaterColor_var.rgb,_DarkFoamColor_var.rgb,_MainTexture2_var.rgb),_LightFoamColor_var.rgb,_MainTexture_var.rgb);
                float3 emissive = node_9747;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma target 3.0
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _Choppiness)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float node_7590 = 0.0;
                float2 node_9263 = mul(unity_ObjectToWorld, v.vertex).rgb.rb;
                float4 node_1843 = _Time;
                float _Choppiness_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Choppiness );
                v.vertex.xyz += (objPos.rgb+float3(float2(node_7590,(sin(((node_9263.r+node_9263.g)+node_1843.g))*_Choppiness_var)),node_7590));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
