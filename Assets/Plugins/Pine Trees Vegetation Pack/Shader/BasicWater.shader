Shader "Water/BasicWater" {
	Properties {
		_Color ("Water Color", Color) = (0,0.2,1,1)
		_BumpMap ("Waves Normal Map", 2D) = "bump" {}
		_BumpMap2 ("Water Normal Map", 2D) = "bump" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_ScrollXSpeed("X scroll speed", Range(-10, 10)) = 0
        _ScrollYSpeed("Y scroll speed", Range(-10, 10)) = -0.4 
	}
	SubShader {
		Tags { "RenderType"="Transparent"  "Queue"="Transparent"}
		LOD 200
		Cull off
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		
		sampler2D _BumpMap;
		sampler2D _BumpMap2;
		fixed _ScrollXSpeed;
        fixed _ScrollYSpeed;
		
		struct Input {
			float2 uv_BumpMap;
			float2 uv_BumpMap2;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed offsetX = _ScrollXSpeed * _Time;
            fixed offsetY = _ScrollYSpeed * _Time;
			fixed2 offsetUV = fixed2(offsetX, offsetY);
		
			// Albedo comes from a texture tinted by color
			fixed4 c = _Color;
			o.Albedo = c.rgb;
			fixed3 normal1 = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap+offsetUV)); 
			fixed3 normal2 = UnpackNormal(tex2D(_BumpMap2, IN.uv_BumpMap2-offsetUV)); 
			o.Normal = normal1+normal2;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
