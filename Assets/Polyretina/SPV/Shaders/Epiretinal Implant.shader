Shader "LNE/Epiretinal Implant"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment phospherisation

			#pragma shader_feature_local OUTLINE

			#define FIRST_PASS

			#include "Vert.cginc"
			#include "Phospherisation.cginc"
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
