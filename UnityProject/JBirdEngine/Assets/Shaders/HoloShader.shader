Shader "Custom/HoloShader" {
	Properties {
		//_Color ("Color", Color) = (1,1,1,1)
		_Hue ("Hue", Range(0,360)) = 0
		_Alpha ("Alpha", Range(0,1)) = 1
		_HoloTex ("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness ("Smoothness", Range(0,1)) = 1
	}
	SubShader {
		Tags { "Queue"="Transparent+10" "RenderType" = "Transparent" }
		LOD 200
		GrabPass { "_HoloTex" }
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _HoloTex;

		struct Input {
			float2 uv_HoloTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Hue;
		float _Alpha;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float2 newUV = float2(IN.uv_HoloTex.x, IN.uv_HoloTex.y);
			fixed4 c = tex2D (_HoloTex, newUV); //* _Color;
			//convert tex color to HSV
			float cmax = max(max(c.r, max(c.g, c.b)), .004);
			float cmin = max(min(c.r, min(c.g, c.b)), .004);
			float s = (cmax - cmin) / cmax;
			//convert to RGB
			float R = saturate(abs((_Hue / 360) * 6 - 3) - 1);
			float G = saturate(2 - abs((_Hue / 360) * 6 - 2));
			float B = saturate(2 - abs((_Hue / 360) * 6 - 4));
			float3 hueRGB = float3(R,G,B);
			float4 endColor = float4(((hueRGB - 1) * s + 1) * cmax, _Alpha);
			o.Albedo = endColor.rgb;
			o.Smoothness = 1;
			o.Alpha = endColor.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
