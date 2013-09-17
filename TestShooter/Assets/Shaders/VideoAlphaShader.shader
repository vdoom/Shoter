
Shader "submarine/Planes/VideoAlpha"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_Fafa ("Fafa", Range (0, 1)) = 0.5
		_MainTex ("Texture", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range (0, 1)) = .01
	}

	Category
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Alphatest Greater [_Cutoff]
		Cull Off 
		ZWrite On 
		ColorMask RGBA
		Lighting Off
		Material { Diffuse [_Color] }
		ColorMaterial AmbientAndDiffuse

		Blend SrcAlpha OneMinusSrcAlpha

		SubShader
		{
			Pass
			{
				CGPROGRAM 
					#pragma vertex vert
					#pragma fragment frag 
					#include "UnityCG.cginc"  

					float4 _Color;
					float _Fafa;

					sampler2D _MainTex; 
					sampler2D _AlphaTex; 

					float4 _MainTex_ST;

					struct appdata {
						float4 vertex : POSITION;
						float2 texcoord : TEXCOORD0;
					};

					struct v2f 
					{ 
						float4 pos : POSITION; 
						float2 uv : TEXCOORD0;
						float2 uv2 : TEXCOORD1;
					}; 

					v2f vert (appdata v)
					{
						v2f o;
						o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
						o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
						o.uv2 = TRANSFORM_TEX((v.texcoord + float2(0.5, 0)), _MainTex);
						return o;
					}

					half4 frag (v2f i) : COLOR 
					{ 
						float4 color = tex2D(_MainTex, i.uv); 
						float4 color2 = tex2D(_MainTex, i.uv2);

						return float4(color.r, color.g, color.b, color2.r); 
					} 
				ENDCG
			}
		}
	}

	Fallback "vexillum/Sprites/Normal"
}