
Shader "submarine/Planes/AdditiveCullBack"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
	}

	Category
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		Cull Back 
		ZWrite On 
		ColorMask RGBA
		Lighting Off
		Material { Diffuse [_Color] }
		ColorMaterial AmbientAndDiffuse

		SubShader
		{
			Pass
			{
				BindChannels
				{
					Bind "Vertex", vertex
					Bind "texcoord", texcoord
					Bind "Color", color
				}
				SetTexture [_MainTex]
				{
					constantColor [_Color]
					combine texture * constant
				}
				SetTexture [_MainTex]
				{
					combine previous * primary
				}
			}
		}
	}
}