Shader "LNE/TextureVR" 
{
	Properties 
	{
		_LeftTex ("Left Texture (RGB)", 2D) = "white" {}
		_RightTex ("Right Texture (RGB)", 2D) = "white" {}
	}

	SubShader 
	{
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			sampler2D _LeftTex;
			sampler2D _RightTex;

			float4 frag(v2f_img i) : COLOR 
			{
				i.uv.x *= 2.0;

				if (i.uv.x < 1.0)
				{
					return tex2D(_LeftTex, i.uv);
				}
				else
				{
					return tex2D(_RightTex, i.uv + float2(-1.0, 0.0));
				}
			}
			ENDCG
		}
	}
}
