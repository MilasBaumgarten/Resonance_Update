// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "screen_fade"
{
	Properties
	{
		_Alphatexture("Alpha texture", 2D) = "white" {}
		_ScreenRender("ScreenRender", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _ScreenRender;
		uniform float4 _ScreenRender_ST;
		uniform sampler2D _Alphatexture;
		uniform float4 _Alphatexture_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_ScreenRender = i.uv_texcoord * _ScreenRender_ST.xy + _ScreenRender_ST.zw;
			float4 tex2DNode4 = tex2D( _ScreenRender, uv_ScreenRender );
			float4 appendResult9 = (float4(tex2DNode4.r , tex2DNode4.g , tex2DNode4.b , 0.0));
			float2 uv_Alphatexture = i.uv_texcoord * _Alphatexture_ST.xy + _Alphatexture_ST.zw;
			float4 tex2DNode5 = tex2D( _Alphatexture, uv_Alphatexture );
			o.Emission = ( appendResult9 + ( tex2DNode5.r * 0.15 ) ).xyz;
			o.Alpha = tex2DNode5.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16200
563;92;914;695;1202.637;564.7022;1.974996;True;True
Node;AmplifyShaderEditor.RangedFloatNode;13;-581.1343,66.53991;Float;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;0.15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-710.1348,-148.9848;Float;True;Property;_ScreenRender;ScreenRender;1;0;Create;True;0;0;False;0;None;4c7d4ec7de8056944ab0502195fb1250;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-649.5,158.4501;Float;True;Property;_Alphatexture;Alpha texture;0;0;Create;True;0;0;False;0;None;190d5bd102b1ece4486c1b7a28643776;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-330.1585,77.96987;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-350.5401,-120.7148;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-162.6781,46.55994;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;screen_fade;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;5;1
WireConnection;11;1;13;0
WireConnection;9;0;4;1
WireConnection;9;1;4;2
WireConnection;9;2;4;3
WireConnection;10;0;9;0
WireConnection;10;1;11;0
WireConnection;2;2;10;0
WireConnection;2;9;5;1
ASEEND*/
//CHKSM=C9AE5B56162F66F9F5AC3579C64E2D273B0434B4