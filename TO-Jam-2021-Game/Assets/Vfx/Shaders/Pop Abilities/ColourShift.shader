Shader "Custom/ColourShift"
{
    Properties
    {
        _Color ("Colour", Color) = (0,0,0,0)
		_Alpha ("Alpha Multiplier", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType" = "Transparent" }
		ZTest Always

		GrabPass
		{
			"_GrabTex"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _GrabTex;
            float4 _Color;
			fixed _Alpha;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2Dproj(_GrabTex, i.grabPos);
				fixed alpha = _Color.a * _Alpha;

				col = fixed4(
					lerp(col.r, _Color.r, alpha),
					lerp(col.g, _Color.g, alpha),
					lerp(col.b, _Color.b, alpha),
					1
				);

                return col;
            }
            ENDCG
        }
    }
}
