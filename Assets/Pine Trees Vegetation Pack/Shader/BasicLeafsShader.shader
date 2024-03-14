//Simple foliage

Shader "Nature/Standard Foliage" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGBA)", 2D) = "white" {}
		_Occlusion ("Occlusion Map",2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Specular ("Specular (A)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Wetness", Range(0,1)) = 0
		_Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
		_NormalIntensity ("Normal Intensity", Range(0,2)) = 1
		_OcclusionIntensity ("Occlusion Intensity", Range(0,1)) = 1

		_Speed("WindSpeed",Range(0.1,10)) = 1
		_Amount("WindForce", Range(0.1,10)) = 3
		_Distance("WindScale", Range( 0, 0.5)) = 0.1
	}
	SubShader {
		Tags { "RenderType"="TransparentCutout" "Queue"="Geometry"}
		
		LOD 300
		cull off
		Blend Off
        ZWrite On
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alphatest:_Cutoff vertex:vert addshadow 

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Occlusion;
		sampler2D _Specular;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Specular;
			float2 uv_BumpMap;
			float2 uv_Occlusion;
		};

		half _Glossiness;
		half _NormalIntensity;
		half _OcclusionIntensity;
		half _Metallic;
		fixed4 _Color;

		float _Speed;
		float _Amount;
		float _Distance;
		
		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 s = tex2D (_Specular, IN.uv_Specular);	
			half ao = tex2D (_Occlusion,IN.uv_Occlusion);
			o.Albedo = c.rgb;
			
			fixed3 normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)); 
			normal = lerp(half3(0,0,1),normal,_NormalIntensity);
			o.Normal = normal;
			o.Occlusion = lerp(half3(1,1,1),ao,_OcclusionIntensity);
			o.Metallic = _Metallic;
			o.Smoothness = s.a * _Glossiness;
			o.Alpha = c.a;
		}
		
		float4 windPos(float4 vert)
		{
			float4 newVert = mul(unity_ObjectToWorld, vert);
			
			float _nxz = 0.1 * sin(_Time.y *(_Speed / 10 * 1) + newVert.y * _Amount * (1 / 10)) ;
			newVert.x += sin(_Time.y * _Speed + newVert.y * _Amount) * _Distance * (1 - _nxz);
			newVert.z += sin(_Time.y * _Speed + newVert.y * _Amount) * _Distance * (_nxz);
			newVert.y +=  sin(_Time.y * _Speed + newVert.y * _Amount) * _Distance ;
			vert = mul(unity_WorldToObject, newVert);
			return vert;
		}

		void vert(inout appdata_full v)
		{
			float4 newVert = windPos(v.vertex);
			v.vertex = newVert;
		}
		
		ENDCG
	}
	FallBack "Transparent"
}
