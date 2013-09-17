
Shader "submarine/Planes/NormalNoTexAlpha"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
	}

	Category
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Alphatest Greater .01
		Cull Off 
		ZWrite On 
		ColorMask RGBA
		Lighting Off
		Material { Diffuse [_Color] }
		ColorMaterial AmbientAndDiffuse

		SubShader
		{
			Pass
			{
				SetTexture [_MainTex]
				{
					constantColor (0, 0, 0, 1)
					combine texture + constant
				}
				SetTexture [_MainTex]
				{
					constantColor [_Color]
					combine previous * constant
				}
			}
		}
	}
}