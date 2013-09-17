
Shader "submarine/Planes/NormalShiny"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_Shininess ("Shininess", Range (0, 1)) = 0
		_MainTex ("Texture", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range (0, 1)) = .01
	}

	Category
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
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
			 	Blend SrcAlpha OneMinusSrcAlpha				
				SetTexture [_MainTex]
				{
					constantColor ([_Shininess], [_Shininess], [_Shininess], [_Shininess])
					combine texture * constant
				}
				
				SetTexture [_MainTex]
				{
					combine texture + previous
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