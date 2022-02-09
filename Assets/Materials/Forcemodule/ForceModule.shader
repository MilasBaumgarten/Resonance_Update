// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ForceModule"
{
	Properties
	{
		_BeamColor("BeamColor", Color) = (0,0.2410286,0.8313726,0)
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend One One
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		

		Pass
		{
			Name "Unlit"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			uniform float4 _BeamColor;
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
			
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 finalColor;
				float mulTime77 = _Time.y * 0.03;
				float2 appendResult53 = (float2(-100.0 , -100.0));
				float2 uv43 = i.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner76 = ( mulTime77 * appendResult53 + uv43);
				float2 fmodResult74 = frac(( panner76 + 0.0 )/float2( 1,1 ))*float2( 1,1 );
				float temp_output_65_0 = ( fmodResult74.x * 2.0 );
				float clampResult71 = clamp( ( temp_output_65_0 * ( 1.0 - temp_output_65_0 ) ) , 0.0 , 1.0 );
				float mulTime57 = _Time.y * 0.02;
				float2 panner7 = ( mulTime57 * appendResult53 + uv43);
				float simplePerlin2D62 = snoise( panner7 );
				float2 fmodResult60 = frac(( ( simplePerlin2D62 * 0.1 ) + uv43 )/float2( 1,1 ))*float2( 1,1 );
				float temp_output_18_0 = ( ( fmodResult60.y - 0.15 ) * 1.4 );
				float temp_output_10_0 = min( temp_output_18_0 , ( 1.0 - temp_output_18_0 ) );
				float lerpResult82 = lerp( temp_output_10_0 , ceil( temp_output_10_0 ) , 0.1);
				float temp_output_78_0 = ( clampResult71 * lerpResult82 );
				float simplePerlin2D85 = snoise( panner76 );
				float clampResult90 = clamp( ( temp_output_78_0 + simplePerlin2D85 ) , 0.0 , 0.1 );
				float temp_output_98_0 = ( ( fmodResult60.y - 0.4 ) * 5.0 );
				float clampResult103 = clamp( min( temp_output_98_0 , ( 1.0 - temp_output_98_0 ) ) , 0.0 , 1.0 );
				float temp_output_69_0 = ( ( ( temp_output_78_0 * 3.0 ) - clampResult90 ) + temp_output_10_0 + clampResult103 );
				float4 temp_output_117_0 = ( _BeamColor * temp_output_69_0 );
				float clampResult61 = clamp( temp_output_69_0 , 0.5 , 2.0 );
				float4 clampResult135 = clamp( ( temp_output_117_0 + ( ( clampResult61 - 0.5 ) / 2.0 ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
				
				
				finalColor = clampResult135;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16100
800;92;657;714;-379.9081;1349.902;2.555649;True;False
Node;AmplifyShaderEditor.RangedFloatNode;49;-2615.801,-413.941;Float;False;Constant;_Float2;Float 2;0;0;Create;True;0;0;False;0;100;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;50;-2410.3,-409.8309;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;57;-2083.921,-371.7213;Float;False;1;0;FLOAT;0.02;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;53;-2231.515,-436.546;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;43;-2485.07,-711.9515;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;7;-1830.345,-590.1527;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;62;-1594.284,-771.6203;Float;False;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-1350.463,-783.3104;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;63;-1171.771,-719.8508;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;77;-2088.244,-212.9187;Float;False;1;0;FLOAT;0.03;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-861.84,-669.949;Float;False;Constant;_Float4;Float 4;0;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;76;-1785.778,-314.3914;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimplifiedFModOpNode;60;-1167.921,-518.4925;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;72;-684.4957,-739.6206;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;45;-948.2314,-517.4337;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;22;-1236.604,-325.0448;Float;True;Constant;_Float1;Float 1;0;0;Create;True;0;0;False;0;0.15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;19;-994.6758,-327.8286;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1060.513,25.49544;Float;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;False;0;1.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimplifiedFModOpNode;74;-524.102,-773.9299;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-739.3928,-592.2286;Float;False;Constant;_Float3;Float 3;0;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;75;-304.4123,-772.871;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-740.5564,-277.0763;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-555.572,-640.3864;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;11;-800.6736,9.015305;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;10;-489.2899,-206.3135;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;68;-318.4578,-524.3263;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-162.5526,58.84052;Float;False;Constant;_Float6;Float 6;0;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;81;-198.5751,-67.2356;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;102;-1421.926,370.5061;Float;False;Constant;_Float8;Float 8;0;0;Create;True;0;0;False;0;0.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-136.9966,-645.3004;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-1119.264,709.8981;Float;False;Constant;_Float7;Float 7;0;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;82;67.09009,-31.21539;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;71;112.7738,-536.6238;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;101;-1126.718,309.1261;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;224.1658,-741.7482;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-799.3079,407.3264;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;85;485.6911,-101.8564;Float;False;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;99;-859.425,693.418;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;460.2855,-628.5673;Float;False;Constant;_Float5;Float 5;0;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;86;698.7505,-116.9712;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;90;964.9625,-100.106;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;100;-548.0414,478.0892;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;584.869,-811.7818;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;88;797.7321,-662.2197;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;103;-241.0931,461.1146;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;69;1002.719,-458.7725;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;130;1327.694,6.638514;Float;False;Constant;_Float9;Float 9;1;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;61;1258.96,-345.2234;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;41;1072.215,-892.326;Float;False;Property;_BeamColor;BeamColor;0;0;Create;True;0;0;False;0;0,0.2410286,0.8313726,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;129;1493.499,-81.14059;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;1501.838,177.3765;Float;False;Constant;_Float10;Float 10;1;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;1371.28,-631.738;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;133;1782.959,-34.74268;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;123;1567.623,-373.738;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;1645.9,-810.6828;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;135;1836.628,-304.642;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;120;1887.979,-515.2522;Float;False;True;2;Float;ASEMaterialInspector;0;1;ForceModule;0770190933193b94aaa3065e307002fa;0;0;Unlit;2;True;4;1;False;-1;1;False;-1;0;1;False;-1;1;False;-1;True;0;False;-1;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;50;0;49;0
WireConnection;53;0;50;0
WireConnection;53;1;50;0
WireConnection;7;0;43;0
WireConnection;7;2;53;0
WireConnection;7;1;57;0
WireConnection;62;0;7;0
WireConnection;64;0;62;0
WireConnection;63;0;64;0
WireConnection;63;1;43;0
WireConnection;76;0;43;0
WireConnection;76;2;53;0
WireConnection;76;1;77;0
WireConnection;60;0;63;0
WireConnection;72;0;76;0
WireConnection;72;1;73;0
WireConnection;45;0;60;0
WireConnection;19;0;45;1
WireConnection;19;1;22;0
WireConnection;74;0;72;0
WireConnection;75;0;74;0
WireConnection;18;0;19;0
WireConnection;18;1;9;0
WireConnection;65;0;75;0
WireConnection;65;1;66;0
WireConnection;11;0;18;0
WireConnection;10;0;18;0
WireConnection;10;1;11;0
WireConnection;68;0;65;0
WireConnection;81;0;10;0
WireConnection;67;0;65;0
WireConnection;67;1;68;0
WireConnection;82;0;10;0
WireConnection;82;1;81;0
WireConnection;82;2;83;0
WireConnection;71;0;67;0
WireConnection;101;0;45;1
WireConnection;101;1;102;0
WireConnection;78;0;71;0
WireConnection;78;1;82;0
WireConnection;98;0;101;0
WireConnection;98;1;97;0
WireConnection;85;0;76;0
WireConnection;99;0;98;0
WireConnection;86;0;78;0
WireConnection;86;1;85;0
WireConnection;90;0;86;0
WireConnection;100;0;98;0
WireConnection;100;1;99;0
WireConnection;79;0;78;0
WireConnection;79;1;80;0
WireConnection;88;0;79;0
WireConnection;88;1;90;0
WireConnection;103;0;100;0
WireConnection;69;0;88;0
WireConnection;69;1;10;0
WireConnection;69;2;103;0
WireConnection;61;0;69;0
WireConnection;129;0;61;0
WireConnection;129;1;130;0
WireConnection;117;0;41;0
WireConnection;117;1;69;0
WireConnection;133;0;129;0
WireConnection;133;1;134;0
WireConnection;123;0;117;0
WireConnection;123;1;133;0
WireConnection;121;0;117;0
WireConnection;121;1;117;0
WireConnection;135;0;123;0
WireConnection;120;0;135;0
ASEEND*/
//CHKSM=728958D4D09152446425BB15643372DF64EE6BCB