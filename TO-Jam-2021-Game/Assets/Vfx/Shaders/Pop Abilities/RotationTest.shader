Shader "Custom/RotationTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Colour", Color) = (1,1,1,1)
		_Pos ("Position", Vector) = (0,0,0,0)
		_Rot ("Rotation", Vector) = (0,0,0,0)
		_Scale ("Scale", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float3 _Pos;
			float3 _Rot;
			float3 _Scale;

            v2f vert (appdata v)
            {
                v2f o;
				float4 vertex = v.vertex;
				float3 euler = _Rot * 0.0174533f;
				float3 cosine = float3(cos(euler.x), cos(euler.y), cos(euler.z));
				float3 sine = float3(sin(euler.x), sin(euler.y), sin(euler.z));

				float4x4 scale = {
					_Scale.x, 0, 0, 0,
					0, _Scale.y, 0, 0,
					0, 0, _Scale.z, 0,
					0, 0, 0, 1
				};
				float4x4 rotZ = {
					cosine.z, -sine.z, 0, 0,
					sine.z, cosine.z, 0, 0,
					0, 0, 1, 0,
					0, 0, 0, 1
				};
				float4x4 rotX = {
					1, 0, 0, 0,
					0, cosine.x, -sine.x, 0,
					0, sine.x, cosine.x, 0,
					0, 0, 0, 1
				};
				float4x4 rotY = {
					cosine.y, 0, sine.y, 0,
					0, 1, 0, 0,
					-sine.y, 0, cosine.y, 0,
					0, 0, 0, 1
				};
				float4x4 translation = {
					1, 0, 0, _Pos.x,
					0, 1, 0, _Pos.y,
					0, 0, 1, _Pos.z,
					0, 0, 0, 1,
				};

				float4x4 transform = mul(translation, mul(rotY, mul(rotX, mul(rotZ, scale))));
				vertex = mul(transform, vertex);

                o.vertex = UnityObjectToClipPos(vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = v.normal;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			half4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				col *= _Color;
				col *= (1-dot(i.normal, _WorldSpaceLightPos0.xyz)) *0.5 + 0.5;
                return col ;
            }
            ENDCG
        }
    }
}
