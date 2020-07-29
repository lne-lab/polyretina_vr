#ifndef TAIL_DISTORTION_CGINC
#define TAIL_DISTORTION_CGINC

	#include "Coordinates.cginc"
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

	sampler2D _axon_tex;
	float2 _eye_gaze;
	float2 _headset_diameter;
	float _polyretina_radius;
	float _decay_const;

	/*
	 * Constants
	 */

	static const float	TAIL_LENGTH = 15;

#if defined(LOW_QUALITY)
	static const int	TAIL_PRECISION = 50;
#elif defined(MEDIUM_QUALITY)
	static const int	TAIL_PRECISION = 100;
#else
	static const int	TAIL_PRECISION = 150;
	#ifndef HIGH_QUALITY
	#define HIGH_QUALITY
	#endif
#endif

	static const float	TAIL_INCREMENT = TAIL_LENGTH / TAIL_PRECISION;

	static const float	FOV_BUFFER = 2000.0;
	static const float2 FOV_BUFFER_OFFSET = float2(500.0, 0.0);

	/*
	 * Functions
	 */

	void clip_polyretina(float2 uv)
	{
		// add a buffer to allow tails outside of the polyretina
		// add an offset to the buffer because tails are longer on the right of the optic disc
		clip((_polyretina_radius + FOV_BUFFER) - distance(pixel_to_retina(uv, _headset_diameter), FOV_BUFFER_OFFSET));
	}

	float calculate_phi(float phi0, float rho, float b, float c)
	{
		return phi0 + b * pow(rho - 4.0, c);
	}

	float calculate_tail(float4 data, float2 uv)
	{
		float phi0 = data.r;
		float rho = data.g;
		float b = data.b;
		float c = data.a;

		float2 angle = pixel_to_angle(uv, _headset_diameter);	// angle at current pixel
		float inv_decay_const = 1.0 / _decay_const;				// inverse of decay const is more efficient

		float output = 0.0;
		for (int i = 0; i < TAIL_PRECISION; ++i)
		{
			float phi = calculate_phi(phi0, rho, b, c);
			float2 tail = polar_to_pixel(rho, phi, _headset_diameter);	// uv coordinate of a pixel on the tail (relative to rho)

			float2 tail_eye_gaze = tail + _eye_gaze;
#ifdef RT_TARGET
			// grab passes work differently when rendering to an RT, so uv coordinates are inverted
			tail_eye_gaze.y = 1.0 - tail_eye_gaze.y;
#endif
			float luminance = tex2D(TEX, tail_eye_gaze).g;	// g because r is used to outline

			float dist = distance(angle, pixel_to_angle(tail, _headset_diameter));	// distance from current pixel to tail pixel
			float sensitivity = exp(-dist * inv_decay_const);
			float activation = min(luminance, luminance * sensitivity);
			output = max(output, activation);

			rho -= TAIL_INCREMENT;
		}

		return output;
	}

	/*
	 * Frag
	 */

	float4 tail_distortion(float2 uv : TEXCOORD0) : SV_TARGET
	{
		// clip all pixels if decay const is basically zero
		clip(_decay_const - 0.00001);

#ifdef GRAB_PASS
		_eye_gaze.y = -_eye_gaze.y;
#endif

#ifdef RT_TARGET
		// grab passes work differently when rendering to an RT, so uv coordinates are inverted
		float2 eye_uv = float2(uv.x, 1.0 - uv.y) - _eye_gaze;
#else
		float2 eye_uv = uv - _eye_gaze;
#endif

		// clip pixels far outside of the polyretina to increase efficiency
		clip_polyretina(eye_uv);

		float phosphene = tex2D(TEX, uv).g;	// g because r is used to outline
		float4 data = tex2D(_axon_tex, eye_uv);
		float tail = calculate_tail(data, eye_uv);
		float outside_od = step(4.0, data.g);
		float luminance = max(phosphene, tail) * outside_od;

		// final output
		float4 output = float4(luminance.xxx, 1.0);

#ifdef OUTLINE
		// draw outline
		output += (outline_polyretina(eye_uv, _headset_diameter, _polyretina_radius) +
			outline_polyretina(eye_uv, _headset_diameter, FOV_BUFFER_OFFSET, _polyretina_radius + (FOV_BUFFER - 20)) +
			outline_optic_disc(data.g));
#endif

		return output;
	}

#endif
