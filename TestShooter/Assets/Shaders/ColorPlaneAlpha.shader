
Shader "submarine/ColorPlane/Alpha"
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
			Blend SrcAlpha OneMinusSrcAlpha 
			Cull Off 
			ZWrite On
			
			Pass
			{
				Color [_Color]
			}
		}
	}
}