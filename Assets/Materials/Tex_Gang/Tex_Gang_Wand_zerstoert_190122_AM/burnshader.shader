// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "burnshader"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 tex2DNode26 = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 color7 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float4 color8 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float lerpResult5 = lerp( color7.r , color8.g , i.uv_texcoord.y);
			float simplePerlin2D16 = snoise( ( i.uv_texcoord * 7.0 ) );
			float temp_output_17_0 = ( ( ( lerpResult5 - 0.5 ) * ( 4.0 / 0.5 ) ) + simplePerlin2D16 );
			float clampResult22 = clamp( temp_output_17_0 , 0.0 , 1.0 );
			float temp_output_47_0 = ( clampResult22 * 0.5 );
			float3 lerpResult88 = lerp( tex2DNode26 , BlendNormals( tex2DNode26 , UnpackNormal( tex2D( _TextureSample0, uv_TextureSample0 ) ) ) , temp_output_47_0);
			o.Normal = lerpResult88;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode1 = tex2D( _Albedo, uv_Albedo );
			float4 lerpResult23 = lerp( tex2DNode1 , float4( 0.06603771,0,0,0 ) , clampResult22);
			float4 color31 = IsGammaSpace() ? float4(0.4352941,0,0,0) : float4(0.1589608,0,0,0);
			float lerpResult58 = lerp( color31.r , -0.8 , ( 1.0 - i.uv_texcoord.y ));
			float4 temp_output_61_0 = ( color31 * lerpResult58 );
			float4 temp_output_29_0 = ( temp_output_17_0 * ( temp_output_61_0 + ( temp_output_61_0 * 20.0 ) ) );
			float4 clampResult79 = clamp( temp_output_29_0 , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 lerpResult76 = lerp( lerpResult23 , ( lerpResult23 + clampResult79 ) , clampResult22);
			o.Albedo = lerpResult76.rgb;
			float4 temp_cast_1 = (20.0).xxxx;
			float4 clampResult91 = clamp( ( temp_output_29_0 * temp_output_47_0 ) , float4( 0,0,0,0 ) , temp_cast_1 );
			o.Emission = clampResult91.rgb;
			o.Smoothness = ( tex2DNode1.a - temp_output_47_0 );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
872;92;714;714;1178.118;799.3161;1.3;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;57;-2268.148,-500.1182;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-2415.26,-33.93702;Float;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-2993.319,783.6063;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;82;-2046.282,-307.2616;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;90;-1938.782,-534.681;Float;False;315;303;Comment;1;58;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;8;-2410.446,148.4224;Float;False;Constant;_Color1;Color 1;1;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;31;-2149.045,-691.1884;Float;False;Constant;_Color2;Color 2;3;0;Create;True;0;0;False;0;0.4352941,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-2322.54,522.7925;Float;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;58;-1888.782,-484.681;Float;True;3;0;FLOAT;0;False;1;FLOAT;-0.8;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2367.518,891.3495;Float;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;5;-2169.575,226.081;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1885.34,962.5983;Float;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;False;0;7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-1627.824,-675.2909;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1688.541,781.799;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;14;-2114.393,898.169;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;9;-1976.631,501.0631;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1635.375,-361.8163;Float;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;16;-1448.337,776.1248;Float;False;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1577.994,516.3466;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-1401.214,-395.8074;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-1234.138,517.0917;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;67;-1286.02,-663.9606;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;22;-819.7567,528.8252;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1251.557,-912.1458;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;95446a1afef1dfb4aa5e583aa4078b35;95446a1afef1dfb4aa5e583aa4078b35;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1007.745,-506.1703;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-611.2456,693.4789;Float;False;Constant;_Float3;Float 3;2;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;87;-172.3622,654.4714;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;38713203b4fc4154ca971884db8f73b0;38713203b4fc4154ca971884db8f73b0;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;23;-596.4684,-676.9501;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.06603771,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;26;-540.3848,-151.744;Float;True;Property;_Normal;Normal;1;0;Create;True;0;0;False;0;890c6aaac024a994aa2efdbe1dd67a9d;890c6aaac024a994aa2efdbe1dd67a9d;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-449.806,510.0187;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;79;-498.5175,-425.9576;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendNormalsNode;86;14.45581,367.1628;Float;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;75;-250.4445,-518.6285;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;82.15359,-249.3167;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;92;460.2661,122.8655;Float;False;Constant;_Float4;Float 4;3;0;Create;True;0;0;False;0;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;44;-289.6329,90.88084;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;83;-1208.535,908.3233;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;76;92.20157,-493.7991;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;28;-996.3328,443.6227;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;88;97.46352,-17.45788;Float;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;91;378.9519,-124.1461;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;737.8375,-150.3555;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;burnshader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;82;0;57;2
WireConnection;58;0;31;1
WireConnection;58;2;82;0
WireConnection;5;0;7;1
WireConnection;5;1;8;2
WireConnection;5;2;4;2
WireConnection;61;0;31;0
WireConnection;61;1;58;0
WireConnection;18;0;4;0
WireConnection;18;1;19;0
WireConnection;14;0;15;0
WireConnection;14;1;12;0
WireConnection;9;0;5;0
WireConnection;9;1;12;0
WireConnection;16;0;18;0
WireConnection;10;0;9;0
WireConnection;10;1;14;0
WireConnection;66;0;61;0
WireConnection;66;1;68;0
WireConnection;17;0;10;0
WireConnection;17;1;16;0
WireConnection;67;0;61;0
WireConnection;67;1;66;0
WireConnection;22;0;17;0
WireConnection;29;0;17;0
WireConnection;29;1;67;0
WireConnection;23;0;1;0
WireConnection;23;2;22;0
WireConnection;47;0;22;0
WireConnection;47;1;48;0
WireConnection;79;0;29;0
WireConnection;86;0;26;0
WireConnection;86;1;87;0
WireConnection;75;0;23;0
WireConnection;75;1;79;0
WireConnection;81;0;29;0
WireConnection;81;1;47;0
WireConnection;44;0;1;4
WireConnection;44;1;47;0
WireConnection;83;0;16;0
WireConnection;76;0;23;0
WireConnection;76;1;75;0
WireConnection;76;2;22;0
WireConnection;28;0;17;0
WireConnection;88;0;26;0
WireConnection;88;1;86;0
WireConnection;88;2;47;0
WireConnection;91;0;81;0
WireConnection;91;2;92;0
WireConnection;0;0;76;0
WireConnection;0;1;88;0
WireConnection;0;2;91;0
WireConnection;0;4;44;0
ASEEND*/
//CHKSM=E663E95029EF0BF828A6DD5A4334A56FD4B122EA