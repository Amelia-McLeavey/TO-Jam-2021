Shader "Custom/ColourShift"
{
    Properties
    {
        _Color ("Colour", Color) = (0,0,0,0)
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

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
					//TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2Dproj(_GrabTex, i.grabPos);
				col = fixed4(
					lerp(col.r, _Color.r, _Color.a),
					lerp(col.g, _Color.g, _Color.a),
					lerp(col.b, _Color.b, _Color.a),
					1
				);
                return col;
            }

			fixed lerp(fixed a, fixed b, fixed t) {
				return a + t * (b - a);
			}
            ENDCG
        }
    }
}
