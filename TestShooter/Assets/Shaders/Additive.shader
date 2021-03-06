
Shader "submarine/Planes/Additive"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range (0, 1)) = .01
	}

	Category
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		Alphatest Greater [_Cutoff]
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
					constantColor [_Color]
					combine texture * constant
				}
			}
		}
	}
}