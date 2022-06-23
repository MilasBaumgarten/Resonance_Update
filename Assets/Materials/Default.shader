Shader "Custom/Default" {
	Properties {
		_ColorRough ("Basecolor and Roughness", 2D) = "white" {}
		_ColorTint("Tint", Color) = (1.0, 1.0, 1.0, 1.0)
		_Normal ("Normal", 2D) = "bump" {}
		_Metallic ("Metallic", 2D) = "black" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _ColorRough;
		fixed4 _ColorTint;
		sampler2D _Normal;
		sampler2D _Metallic;

		struct Input {
			float2 uv_ColorRough;
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 colorRough = tex2D(_ColorRough, IN.uv_ColorRough);
			fixed3 normal = UnpackNormal(tex2D(_Normal, IN.uv_ColorRough));	// = -0.5) * 2 -> [-1; 1]
			fixed3 metallic = tex2D(_Metallic, IN.uv_ColorRough);

			o.Albedo = colorRough.rgb * _ColorTint;
			o.Smoothness = 1 - colorRough.a;

			o.Normal = normal;

			o.Metallic = metallic.g;
		}
		ENDCG
	}
	FallBack "Diffuse"
}