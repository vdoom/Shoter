
Shader "submarine/WaterDrop"
{
	Properties
	{    
		_MainTex ("Texture", 2D) = "white" { }
	}

	Category
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

		Blend Off
		ColorMask RGBA
		Cull Back 
		Lighting Off 
		ZWrite Off
		ZTest Always

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
					constantColor (1, 1, 1, 1)
					combine constant - primary
				}
				SetTexture [_MainTex]
				{
					combine texture * previous
				}
			}
		}
	}
}