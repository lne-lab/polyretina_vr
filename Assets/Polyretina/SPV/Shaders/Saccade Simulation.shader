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

			static const float2 hexagon[] = 
			{ 
				float2(0, 0),
				float2(+1, 0),
				float2(-1, 0),
				float2(+0.5, +0.86603),
				float2(-0.5, +0.86603),
				float2(+0.5, -0.86603),
				float2(-0.5, -0.86603)
			};

			float2 calc_offset(float2 uv, float2 direction)
			{
				float2 r_uv = pixel_to_retina(uv, _headset_diameter);
				return retina_to_pixel(r_uv + direction, _headset_diameter);
			}

			float4 frag(float2 uv : TEXCOORD0) : SV_TARGET
			{
				uint frame = _frame_count / (_pulse_frequency * _frequency);
				uv = calc_offset(uv, float2(_electrode_pitch, _electrode_pitch) * hexagon[frame % _type]);
				return tex2D(_MainTex, uv);
			}
			ENDCG
		}
	}
}
