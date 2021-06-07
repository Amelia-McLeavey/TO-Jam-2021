Shader "Custom/ParticleClippingDirt"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_RenderSphere("Render Sphere Vars", Vector) = (0,0,0,1)
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
		Blend SrcAlpha One

		Cull Off Lighting Off ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float4 circlePos : TEXCOORD2;
				float radius : TEXCOORD3;
			};

			sampler2D _MainTex;
			fixed4 _Color;
			float4 _RenderSphere;

			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				o.circlePos = ComputeScreenPos(UnityObjectToClipPos(mul(unity_WorldToObject, float4(_RenderSphere.xyz, 1))));
				float4 rightCirc = ComputeScreenPos(UnityObjectToClipPos(mul(unity_WorldToObject, float4(_RenderSphere.xyz, 1) + float4(_RenderSphere.w, 0, 0, 0))));
				o.radius = distance(rightCirc.xy * rightCirc.z / rightCirc.w, o.circlePos.xy * o.circlePos.z / o.circlePos.w);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//get this position and circle position at screen space
				float4 screenPos = ComputeScreenPos(UnityObjectToClipPos(mul(unity_WorldToObject, i.worldPos)));
				screenPos.xy = screenPos * i.circlePos.z / screenPos.w;
				i.circlePos.xy = i.circlePos.xy * i.circlePos.z / i.circlePos.w;

				//translate y down to 0, then stretch to be circlular, then translate back (to avoid stretching position)
				fixed yStretch = 9.0 / 16.0;
				screenPos.y = (screenPos.y - i.circlePos.y) * yStretch + i.circlePos.y;

				//calculate distance between this and the circle position (both at screen space), then colour it if within the circle radius
				fixed dist = distance(screenPos.xy, i.circlePos.xy);
				fixed4 col = tex2D(_MainTex, i.uv);
				col = dist < i.radius - 0.0005 ? 2.0f * i.color * _Color * col : -i.color;

				return col;
			}
			ENDCG
		}
	}
}
