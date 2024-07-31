Shader "Custom/ToonTriplannar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _RampTex ("Ramp Texture", 2D) = "white" {}
        _RampOffset ("Ramp Offset", Range(0, 1)) = 0
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _Metallic ("Metallic", Range(0, 1)) = 0.5
    }
 
    SubShader
    {
        Tags {"RenderType"="Opaque" }
        LOD 100
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float3 worldRefl : TEXCOORD3;
                float3 worldNormal2 : TEXCOORD4;
                float3 worldPos2 : TEXCOORD5;
                float3 worldRefl2 : TEXCOORD6;
                float3 worldNormal3 : TEXCOORD7;
                float3 worldPos3 : TEXCOORD8;
                float3 worldRefl3 : TEXCOORD9;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            sampler2D _RampTex;
            float _RampOffset;
            float _Smoothness;
            float _Metallic;
 
            float4x4 unity_ObjectToWorld;
            float4x4 unity_ObjectToWorld2;
            float4x4 unity_ObjectToWorld3;
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
 
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = worldPos;
                o.worldRefl = reflect(UnityWorldSpaceViewDir(worldPos), o.worldNormal);
 
                float3 worldPos2 = mul(unity_ObjectToWorld2, v.vertex).xyz;
                o.worldNormal2 = UnityObjectToWorldNormal(v.normal);
                o.worldPos2 = worldPos2;
                o.worldRefl2 = reflect(UnityWorldSpaceViewDir(worldPos2), o.worldNormal2);
 
                float3 worldPos3 = mul(unity_ObjectToWorld3, v.vertex).xyz;
                o.worldNormal3 = UnityObjectToWorldNormal(v.normal);
                o.worldPos3 = worldPos3;
                o.worldRefl3 = reflect(UnityWorldSpaceViewDir(worldPos3), o.worldNormal3);
 
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                float3 worldNormal = normalize(i.worldNormal);
                float3 worldPos = i.worldPos;
                float3 worldRefl = normalize(i.worldRefl);
                float3 worldNormal2 = normalize(i.worldNormal2);
                float3 worldPos2 = i.worldPos2;
            }
        }
    }
}