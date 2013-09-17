Shader "submarine/Planes/Multiply" {
Properties {    
    _Color ("Main Color", Color) = (1,1,1,1)
    _Shininess ("Shininess", Range (1, 2)) = 1.0
    _MainTex ("Texture", 2D) = "white" { }
}

Category {
Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

SubShader {
	Blend DstColor Zero
	Alphatest Greater .05
	ColorMask RGBA
        Cull Off 
	Lighting Off 
	ZWrite On
	Fog { Color (0,0,0,0) }
    Pass {

CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

float4 _Color;
float _Shininess;
sampler2D _MainTex;

struct v2f {
    float4  pos : SV_POSITION;
    float2  uv : TEXCOORD0;
};

float4 _MainTex_ST;

v2f vert (appdata_base v)
{
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
    return o;
}

half4 frag (v2f i) : COLOR
{
    float4 texcol = tex2D (_MainTex, i.uv);
    texcol.x *= _Shininess;
    texcol.y *= _Shininess;
    texcol.z *= _Shininess;
    return texcol * _Color;
}
ENDCG

    }
}
Fallback "VertexLit"
} 
}