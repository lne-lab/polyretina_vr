Shader "Unlit/Checkerboard"
{
    Properties
    {
        _Density ("Density", Vector) = (0, 0, 0, 0)
    }

    SubShader
    {
        Tags
		{ 
			"RenderType" = "Opaque"
		}

        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float2 _Density;

            v2f vert (float4 pos : POSITION, float2 uv : TEXCOORD0)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(pos);
                o.uv = uv * _Density;
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float2 xy = floor(i.uv) / 2.0;
                float square = frac(xy.x + xy.y) * 2.0;
                return square;
            }
            ENDCG
        }
    }
}
