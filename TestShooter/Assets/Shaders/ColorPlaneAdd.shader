
Shader "submarine/ColorPlane/Add"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
	}

	Category
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		
		SubShader
		{
			Blend SrcAlpha One 
			Cull Off 
			ZWrite On
			
			Pass
			{
				Color [_Color]
			}
		}
	}
}