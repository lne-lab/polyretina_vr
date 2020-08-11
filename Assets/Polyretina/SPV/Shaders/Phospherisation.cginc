#ifndef PHOSPHERISATION_CGINC
#define PHOSPHERISATION_CGINC

	#include "UnityCG.cginc"
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

	sampler2D _electrode_tex;
	float2 _eye_gaze;
	float2 _headset_diameter;
	float _electrode_radius;
	float _polyretina_radius;
	float _broken_chance;
	float _size_variance;
	float _intensity_variance;
	float _brightness;
	int _pulse;
	int _luminance_levels;

	/*
	 * Functions
	 */

	float rand(float seed_x, float seed_y, float seed_z)
	{
		return frac(sin(dot(float3(seed_x, seed_y, seed_z), float3(12.9898, 78.233, 45.5432))) * 43758.5453);
	}
	
	void calc_luminance(float2 eye_uv, out bool is_electrode, out float2 electrode_pos, out bool electrode_is_on, out float luminance)
	{
		// electrode position
		float4 data = tex2D(_electrode_tex, eye_uv);
		float2 position_um = data.rg;
		float2 position_px = retina_to_pixel(position_um, _headset_diameter) + _eye_gaze;

		// electrode position (out param)
		electrode_pos = position_px;

		// input luminance
		float3 input = tex2D(TEX, position_px).rgb;
		
		// electrode size, intensity and if broken
		float broken = step(data.b, _broken_chance);
		float size = 1.0 + lerp(-_size_variance, _size_variance, data.a);
		float intensity = 1.0 - lerp(0.0, _intensity_variance, rand(data.b, data.a, _Time.y));

		// distances
		float distance_to_electrode = distance(pixel_to_retina(eye_uv, _headset_diameter), position_um);
		float distance_to_fovea = length(position_um);
		
		// pixel is an electrode if... (out param)
		is_electrode =	step(distance_to_electrode, _electrode_radius * size)			*	// inside an electrode
						step(distance_to_fovea, _polyretina_radius - _electrode_radius)	*	// inside the polyretina
						(1 - broken);														// electrode is not broken
		
		// electrode luminance (out param)
		luminance = Luminance(input);														// base luminancy
		luminance = round(luminance * (_luminance_levels - 1)) / (_luminance_levels - 1);	// set to an interval of levels
		luminance *= is_electrode * _brightness * intensity;								// adjust luminance

		// electrode is on (out param)
		electrode_is_on = luminance > .001;

		// apply on/off frequency pulse
		luminance *= _pulse;
	}

	float calc_luminance(float2 eye_uv)
	{
		bool is_electrode;
		float2 electrode_pos;
		bool electrode_is_on;
		float luminance;

		calc_luminance(eye_uv, is_electrode, electrode_pos, electrode_is_on, luminance);
		return luminance;
	}

	/*
	 * Frag (without fading)
	 */

	float4 phospherisation(float2 uv : TEXCOORD0) : SV_TARGET
	{
#ifdef GRAB_PASS
		_eye_gaze.y = -_eye_gaze.y;
#endif

		float2 eye_uv = uv - _eye_gaze;
		float luminance = calc_luminance(eye_uv);
		float4 output = float4(luminance.xxx, 1.0);

#ifdef OUTLINE
		output += outline_polyretina(eye_uv, _headset_diameter, _polyretina_radius);
#endif

		return output;
	}

	/*
	 * Fading
	 */
	 
	/*
	 * Properties
	 */

	sampler2D _fade_tex;
	
	float2 _eye_gaze_delta;

	float _t1;
	float _t2;
	float _threshold;

	float _recovery_time;
	float _recovery_exponent;

	/*
	 * Functions
	 */

	float delta_time()
	{
		return unity_DeltaTime.r;
	}

	float update_fade(bool electrode_is_on, float brightness)
	{
		if (electrode_is_on)
		{
			if (brightness > _threshold)
			{
				// if electrode is on and we are above the t threshold, decay quickly (t1)
				float t1_old = _t1 * (brightness - _threshold) / (1 - _threshold);
				float t1_new = t1_old - delta_time();

				brightness = t1_new * (1 - _threshold) / _t1 + _threshold;
			}
			else
			{
				// otherwise, decay slowly (t2)
				float t2_old = _t2 * brightness / _threshold;
				float t2_new = t2_old - delta_time();

				brightness = t2_new * _threshold / _t2;
			}

			// stop brightness going into negative values
			brightness = max(brightness, 0);
		}
		else
		{
			// if electrode is off, recover exponentially
			float start_y = brightness;
			float start_x = (1 - pow(1 - start_y, 1 / _recovery_exponent)) * _recovery_time;

			float x = start_x + delta_time();

			if (x <= _recovery_time)
			{
				brightness = 1 - pow(1 - (x / _recovery_time), _recovery_exponent);
			}
			else
			{
				brightness = 1;
			}
		}

		return brightness;
	}

	/*
	 * Frag (with fading)
	 */

	struct MRT
	{
		float4 phos : SV_TARGET0;
		float4 fade : SV_TARGET1;
	};

	MRT phos_w_fade_mrt(float2 uv : TEXCOORD0)
	{
		// correct eye-gaze

#ifdef GRAB_PASS
		_eye_gaze.y = -_eye_gaze.y;
		_eye_gaze_delta.y = -_eye_gaze_delta.y;
#endif

		float2 eye_uv = uv - _eye_gaze;
		
		// retrieve needed variables
		bool is_electrode;
		float2 electrode_pos;
		bool electrode_is_on;
		float luminance;
		calc_luminance(eye_uv, is_electrode, electrode_pos, electrode_is_on, luminance);

		// retrieve last frames fade data
		float4 fade_data = tex2D(_fade_tex, electrode_pos + _eye_gaze_delta);

		// apply fade if requested
#ifdef USE_FADING
		luminance *= fade_data.r;				
#endif

		// output multiple render target
		MRT mrt;

		// output phosphene image
		mrt.phos = float4(luminance.xxx, 1.0);

		// with optional outline
#ifdef OUTLINE
		mrt.phos += outline_polyretina(eye_uv, _headset_diameter, _polyretina_radius);
#endif

		// ouput single-channel updated fade data
		mrt.fade = update_fade(electrode_is_on, fade_data.r) * is_electrode;

		return mrt;
	}
#endif
