Shader "LNE/TextureR" 
{
	Properties 
	{
		_SubTex ("Texture (RGB)", 2D) = "white" {}
	}

	SubShader 
	{
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			sampler2D _SubTex;

			float4 frag(v2f_img i) : COLOR 
			{
				return float4(tex2D(_SubTex, i.uv).rrr, 1);
			}
			ENDCG
		}
	}
}
