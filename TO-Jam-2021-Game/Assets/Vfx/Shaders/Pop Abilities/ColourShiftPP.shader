Shader "Custom/ColourShiftPP"
{
    Properties
    {
		_MainTex("Main Texture", 2D) = "white" {}
        _Color ("Colour", Color) = (1,1,1,0)
		_RenderSphere ("Render Sphere Vars", Color) = (0,0,0,0)
		_DebugDist ("Debug Distance", Vector) = (0,0,0,0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 renderCirc : TEXCOORD1;
				float radius : TEXCOORD2;
            };

			float4 _RenderSphere;
			float4 _DebugDist;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

				o.renderCirc = UnityObjectToClipPos(_RenderSphere.xyz);
				o.renderCirc = ComputeScreenPos(o.renderCirc);
				float3 up = mul((float3x3)unity_CameraToWorld, float3(0, 0, 1));
				float4 rightPointCirc = UnityObjectToClipPos(_RenderSphere.xyz + up * _RenderSphere.w);
				rightPointCirc = ComputeScreenPos(rightPointCirc);
				o.radius = distance(rightPointCirc.xy, o.renderCirc.xy);

                return o;
            }

            fixed4 _Color;
			sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

				//fixed willColorLerp = distance(i.uv, i.renderCirc.xy) < 0.01 ? 0 : 1;
				fixed willColorLerp = distance(i.uv, _DebugDist.xy) < _DebugDist.w ? 1 : 0;
				col = fixed4(
					lerp(col.r, _Color.r, _Color.a * willColorLerp),
					lerp(col.g, _Color.g, _Color.a * willColorLerp),
					lerp(col.b, _Color.b, _Color.a * willColorLerp),
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
