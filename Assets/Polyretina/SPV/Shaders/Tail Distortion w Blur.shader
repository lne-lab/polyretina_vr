Shader "LNE/Tail Distortion (w/ Blur)"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		/*
			This first pass is a complete passthrough, but is necessary for clipping 
			to work in the tail_distortion shader which is essential for performance.
		*/

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#define FIRST_PASS
			
			#include "Vert.cginc"
			#include "Frag.cginc"
			ENDCG
		}
		
		GrabPass { }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment tail_distortion

			#pragma shader_feature_local RT_TARGET
			#pragma shader_feature_local OUTLINE
			#pragma shader_feature_local LOW_QUALITY MEDIUM_QUALITY HIGH_QUALITY
			
			#include "Vert.cginc"
			#include "Tail Distortion.cginc"
			ENDCG
		}
		
		GrabPass { }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment horizontal_blur3

			#include "Vert.cginc"
			#include "Blur.cginc"
			ENDCG
		}

		GrabPass { }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment vertical_blur3

			#include "Vert.cginc"
			#include "Blur.cginc"
			ENDCG
		}
	}
}
