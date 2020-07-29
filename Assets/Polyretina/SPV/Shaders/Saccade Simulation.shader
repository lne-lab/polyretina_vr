Shader "LNE/Saccade Simulation"
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
			#pragma fragment frag

			#define FIRST_PASS

			#include "UnityCG.cginc"
			#include "Vert.cginc"
			#include "Coordinates.cginc"

			sampler2D _MainTex;

			float2 _headset_diameter;
			float _electrode_pitch;
			uint _pulse_frequency;

			uint _type;
			uint _frequency;
			uint _frame_count;

			float2 calc_offset(float2 uv, float2 direction)
			{
				float2 r_uv = pixel_to_retina(uv, _headset_diameter);
				return retina_to_pixel(r_uv + direction, _headset_diameter);
			}

			float4 frag(float2 uv : TEXCOORD0) : SV_TARGET
			{
				uint frame = _frame_count / (_pulse_frequency * _frequency);
				float x = _electrode_pitch / 2;
				float y = x * 1.73205;

				if (frame % _type == 1)
				{
					uv = calc_offset(uv, float2(+_electrode_pitch, 0));
				}
				else if (frame % _type == 2)
				{
					uv = calc_offset(uv, float2(-_electrode_pitch, 0));
				}
				else if (frame % _type == 3)
				{
					uv = calc_offset(uv, float2(+x, +y));
				}
				else if (frame % _type == 4)
				{
					uv = calc_offset(uv, float2(-x, +y));
				}
				else if (frame % _type == 5)
				{
					uv = calc_offset(uv, float2(+x, -y));
				}
				else if (frame % _type == 6)
				{
					uv = calc_offset(uv, float2(-x, -y));
				}

				return tex2D(_MainTex, uv);
			}
			ENDCG
		}
	}
}
