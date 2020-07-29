#ifndef VERT_CGINC
#define VERT_CGINC

    #include "UnityCG.cginc"

    void vert(float4 wrld_pos : POSITION, out float2 uv : TEXCOORD0, out float4 clip_pos : SV_POSITION)
    {
        clip_pos = UnityObjectToClipPos(wrld_pos);
#ifdef FIRST_PASS
        uv = ComputeScreenPos(clip_pos);
#else
        uv = ComputeGrabScreenPos(clip_pos);
#endif
    }

#endif
