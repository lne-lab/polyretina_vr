Shader "LNE/Edge Detection"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment horizontal_blur

			#pragma shader_feature_local TAP_3 TAP_5 TAP_9 TAP_13

			#define FIRST_PASS

			#include "Vert.cginc"
			#include "Blur.cginc"
			ENDCG
		}

		GrabPass
		{

		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment vertical_blur

			#pragma shader_feature_local TAP_3 TAP_5 TAP_9 TAP_13

			#include "Vert.cginc"
			#include "Blur.cginc"
			ENDCG
		}
		
		GrabPass
		{

		}

		// 
		// Colour/Luminance processing
		//

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Vert.cginc"

			sampler2D _GrabTexture;

			float _contrast;
			float _brightness;
			float _saturation;

			float4 frag(float2 uv : TEXCOORD0) : SV_TARGET
			{
				float3 result = tex2D(_GrabTexture, uv).rgb;

				// valid brightness values are (-.5 to .5 * contrast)
				_brightness *= _contrast;

				result = (result - 0.5) * _contrast + 0.5;					// contrast
				result = result + _brightness;								// brightness
				result = lerp(Luminance(result).xxx, result, _saturation);	// saturation

				return float4(result, 1.0);
			}
			ENDCG
		}

		GrabPass
		{

		}

		//
		// Edge detection
		//

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Vert.cginc"
			#include "Functions.cginc"

			sampler2D _GrabTexture;

			float max4(float a, float b, float c, float d)
			{
				return max(max(max(a, b), c), d);
			}

			float4 frag(float2 uv : TEXCOORD0) : SV_TARGET
			{
				float2 pixel_size = get_pixel_size();

				float3 pixel1 = tex2D(_GrabTexture, float2(uv.x + pixel_size.x, uv.y			   )).rgb;
				float3 pixel2 = tex2D(_GrabTexture, float2(uv.x - pixel_size.x, uv.y			   )).rgb;
				float3 pixel3 = tex2D(_GrabTexture, float2(uv.x				  , uv.y + pixel_size.y)).rgb;
				float3 pixel4 = tex2D(_GrabTexture, float2(uv.x				  , uv.y - pixel_size.y)).rgb;
				float3 pixel5 = tex2D(_GrabTexture, float2(uv.x + pixel_size.x, uv.y + pixel_size.y)).rgb;
				float3 pixel6 = tex2D(_GrabTexture, float2(uv.x + pixel_size.x, uv.y - pixel_size.y)).rgb;
				float3 pixel7 = tex2D(_GrabTexture, float2(uv.x - pixel_size.x, uv.y + pixel_size.y)).rgb;
				float3 pixel8 = tex2D(_GrabTexture, float2(uv.x - pixel_size.x, uv.y - pixel_size.y)).rgb;

				float3 gx	= pixel8
							+ pixel2 * 2.0
							+ pixel7
							- pixel6
							- pixel1 * 2.0
							- pixel5;

				float3 gy	= pixel8
							+ pixel4 * 2.0
							+ pixel6
							- pixel7
							- pixel3 * 2.0
							- pixel5;

				float gvx = max4(gx.r, gx.g, gx.b, Luminance(gx));
				float gvy = max4(gy.r, gy.g, gy.b, Luminance(gy));

				float output = length(float2(gvx, gvy));
				return float4(output.xxx, 1.0);
			}
			ENDCG
		}
		
		GrabPass
		{

		}

		//
		// Edge thickening
		//

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma shader_feature_local THICKNESS_0 THICKNESS_1 THICKNESS_2 THICKNESS_3 THICKNESS_4 THICKNESS_5 THICKNESS_6

			#include "Vert.cginc"
			#include "Functions.cginc"

			sampler2D _GrabTexture;
			float _threshold;

#if defined(THICKNESS_6)
			static const int N = 6;
#elif defined(THICKNESS_5)
			static const int N = 5;
#elif defined(THICKNESS_4)
			static const int N = 4;
#elif defined(THICKNESS_3)
			static const int N = 3;
#elif defined(THICKNESS_2)
			static const int N = 2;
#elif defined(THICKNESS_1)
			static const int N = 1;
#else
			static const int N = 0;
	#ifndef THICKNESS_0
	#define THICKNESS_0
	#endif
#endif

			float4 frag(float2 uv : TEXCOORD0) : SV_TARGET
			{
#ifdef THICKNESS_0
				float luminance = step(_threshold, tex2D(_GrabTexture, uv).r);
#else
				float luminance = 0.0;
				float2 pixel_size = get_pixel_size();

				for (int i = -N; i <= N; ++i)
				{
					for (int j = -N; j <= N; ++j)
					{
						bool not_a_corner = (i > 0 || j > 0) ||
											(i > 0 || j < N) ||
											(i < N || j > 0) ||
											(i < N || j < N);

						if (not_a_corner)
						{
							luminance = max(luminance, tex2D(_GrabTexture, uv + float2(i, j) * pixel_size).r);
						}
					}
				}

				luminance = step(_threshold, luminance);
#endif
				return float4(luminance.xxx, 1.0);
			}
			ENDCG
		}
	}
}
