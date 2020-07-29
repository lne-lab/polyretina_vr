#ifndef FUNCTIONS_CGINC
#define FUNCTIONS_CGINC

	#include "Coordinates.cginc"

	float distance_to_center(float2 p)
	{
		return distance(float2(0.5, 0.5), p);
	}
	
	float4 outline_polyretina(float2 pixel, float2 view_diameter, float2 offset, float polyretina_radius)
	{
		float2 ret = pixel_to_retina(pixel, view_diameter);
		float dst = distance(ret, offset);

		return float4(step(polyretina_radius, dst) * step(dst, polyretina_radius + 50), 0.0, 0.0, 1.0);
	}

	float4 outline_polyretina(float2 pixel, float2 view_diameter, float polyretina_radius)
	{
		return outline_polyretina(pixel, view_diameter, (float2)0.0, polyretina_radius);
	}

	float4 outline_optic_disc(float rho)
	{
		return float4(step(rho, 4.0) * step(3.9, rho), 0.0, 0.0, 1.0);
	}

	float2 get_pixel_size()
	{
		return _ScreenParams.zw - float2(1.0, 1.0);
	}

#endif
