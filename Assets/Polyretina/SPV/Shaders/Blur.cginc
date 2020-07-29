#ifndef BLUR_FRAGS_CGINC
#define BLUR_FRAGS_CGINC

	#include "Functions.cginc"

    /*
     * Properties
     */

#ifdef FIRST_PASS
    #define TEX _MainTex
#else
    #ifndef GRAB_PASS
    #define GRAB_PASS
    #endif
    #define TEX _GrabTexture
#endif

    sampler2D TEX;

    /*
     * Functions
     */

	// 13 uses a sigma of ~2.01
	//  9 uses a sigma of ~1.75
	//  5 uses a sigma of ~1.45
	//  3 uses a sigma of ~1.00

	float4 horizontal_blur13(sampler2D tex, float2 uv)
	{
		float2 pixel_size = get_pixel_size();
		float2 offset1 = float2(1.41176, 0.0);
		float2 offset2 = float2(3.29412, 0.0);
		float2 offset3 = float2(5.17647, 0.0);

		float4 output = (float4)0.0;
		output += tex2D(tex, uv) * 0.19648;
		output += tex2D(tex, uv + offset1 * pixel_size) * 0.29691;
		output += tex2D(tex, uv - offset1 * pixel_size) * 0.29691;
		output += tex2D(tex, uv + offset2 * pixel_size) * 0.09447;
		output += tex2D(tex, uv - offset2 * pixel_size) * 0.09447;
		output += tex2D(tex, uv + offset3 * pixel_size) * 0.01038;
		output += tex2D(tex, uv - offset3 * pixel_size) * 0.01038;
		return output;
	}

	float4 vertical_blur13(sampler2D tex, float2 uv)
	{
		float2 pixel_size = get_pixel_size();
		float2 offset1 = float2(0.0, 1.41176);
		float2 offset2 = float2(0.0, 3.29412);
		float2 offset3 = float2(0.0, 5.17647);

		float4 output = (float4)0.0;
		output += tex2D(tex, uv) * 0.19648;
		output += tex2D(tex, uv + offset1 * pixel_size) * 0.29691;
		output += tex2D(tex, uv - offset1 * pixel_size) * 0.29691;
		output += tex2D(tex, uv + offset2 * pixel_size) * 0.09447;
		output += tex2D(tex, uv - offset2 * pixel_size) * 0.09447;
		output += tex2D(tex, uv + offset3 * pixel_size) * 0.01038;
		output += tex2D(tex, uv - offset3 * pixel_size) * 0.01038;
		return output;
	}

	float4 horizontal_blur9(sampler2D tex, float2 uv)
	{
		float2 pixel_size = get_pixel_size();
		float2 offset1 = float2(1.38462, 0.0);
		float2 offset2 = float2(3.23077, 0.0);

		float4 output = (float4)0.0;
		output += tex2D(tex, uv) * 0.22703;
		output += tex2D(tex, uv + offset1 * pixel_size) * 0.31622;
		output += tex2D(tex, uv - offset1 * pixel_size) * 0.31622;
		output += tex2D(tex, uv + offset2 * pixel_size) * 0.07027;
		output += tex2D(tex, uv - offset2 * pixel_size) * 0.07027;
		return output;
	}

	float4 vertical_blur9(sampler2D tex, float2 uv)
	{
		float2 pixel_size = get_pixel_size();
		float2 offset1 = float2(0.0, 1.38462);
		float2 offset2 = float2(0.0, 3.23077);

		float4 output = (float4)0.0;
		output += tex2D(tex, uv) * 0.22703;
		output += tex2D(tex, uv + offset1 * pixel_size) * 0.31622;
		output += tex2D(tex, uv - offset1 * pixel_size) * 0.31622;
		output += tex2D(tex, uv + offset2 * pixel_size) * 0.07027;
		output += tex2D(tex, uv - offset2 * pixel_size) * 0.07027;
		return output;
	}

	float4 horizontal_blur5(sampler2D tex, float2 uv)
	{
		float2 pixel_size = get_pixel_size();
		float2 offset = float2(1.33333, 0.0);

		float4 output = (float4)0.0;
		output += tex2D(tex, uv) * 0.29412;
		output += tex2D(tex, uv + offset * pixel_size) * 0.35294;
		output += tex2D(tex, uv - offset * pixel_size) * 0.35294;
		return output;
	}

	float4 vertical_blur5(sampler2D tex, float2 uv)
	{
		float2 pixel_size = get_pixel_size();
		float2 offset = float2(0.0, 1.33333);

		float4 output = (float4)0.0;
		output += tex2D(tex, uv) * 0.29412;
		output += tex2D(tex, uv + offset * pixel_size) * 0.35294;
		output += tex2D(tex, uv - offset * pixel_size) * 0.35294;
		return output;
	}

	float4 horizontal_blur3(sampler2D tex, float2 uv)
	{
		float2 pixel_size = float2(get_pixel_size().x, 0.0);

		float4 output = (float4)0.0;
		output += tex2D(tex, uv) * 0.44198;
		output += tex2D(tex, uv + pixel_size) * 0.27901;
		output += tex2D(tex, uv - pixel_size) * 0.27901;
		return output;
	}

	float4 vertical_blur3(sampler2D tex, float2 uv)
	{
		float2 pixel_size = float2(0.0, get_pixel_size().y);

		float4 output = (float4)0.0;
		output += tex2D(tex, uv) * 0.44198;
		output += tex2D(tex, uv + pixel_size) * 0.27901;
		output += tex2D(tex, uv - pixel_size) * 0.27901;
		return output;
	}

    /*
     * Frag
     */

	float4 horizontal_blur3(float2 uv : TEXCOORD0) : SV_TARGET
	{
		return horizontal_blur3(TEX, uv);
	}

	float4 horizontal_blur5(float2 uv : TEXCOORD0) : SV_TARGET
	{
		return horizontal_blur5(TEX, uv);
	}

    float4 horizontal_blur9(float2 uv : TEXCOORD0) : SV_TARGET
    {
        return horizontal_blur9(TEX, uv);
    }

    float4 horizontal_blur13(float2 uv : TEXCOORD0) : SV_TARGET
    {
        return horizontal_blur13(TEX, uv);
    }

	float4 vertical_blur3(float2 uv : TEXCOORD0) : SV_TARGET
	{
		return vertical_blur3(TEX, uv);
	}

	float4 vertical_blur5(float2 uv : TEXCOORD0) : SV_TARGET
	{
		return vertical_blur5(TEX, uv);
	}

    float4 vertical_blur9(float2 uv : TEXCOORD0) : SV_TARGET
    {
        return vertical_blur9(TEX, uv);
    }

    float4 vertical_blur13(float2 uv : TEXCOORD0) : SV_TARGET
    {
        return vertical_blur13(TEX, uv);
    }

    float4 horizontal_blur(float2 uv : TEXCOORD0) : SV_TARGET
    {
#if defined(TAP_3)
		return horizontal_blur3(TEX, uv);
#elif defined (TAP_5)
		return horizontal_blur5(TEX, uv);
#elif defined (TAP_9)
        return horizontal_blur9(TEX, uv);
#elif defined (TAP_13)
        return horizontal_blur13(TEX, uv);
#else
        return tex2D(TEX, uv);
#endif
    }

    float4 vertical_blur(float2 uv : TEXCOORD0) : SV_TARGET
    {
#if defined(TAP_3)
		return vertical_blur3(TEX, uv);
#elif defined (TAP_5)
		return vertical_blur5(TEX, uv);
#elif defined (TAP_9)
        return vertical_blur9(TEX, uv);
#elif defined (TAP_13)
        return vertical_blur13(TEX, uv);
#else
        return tex2D(TEX, uv);
#endif
    }
#endif
