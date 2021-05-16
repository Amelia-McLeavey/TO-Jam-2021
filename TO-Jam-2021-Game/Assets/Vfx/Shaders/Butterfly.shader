Shader "Custom/Butterfly"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_RenderSphere("Render Sphere Vars", Vector) = (0,0,0,1)
		_Color ("Colour", Color) = (1,1,1,0.5)
    }
    SubShader
    {
		Tags { "IgnoreProjector" = "True" "PreviewType" = "Plane" }
		//Blend SrcAlpha One
		Cull Off Lighting Off //ZWrite Off

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
				float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float4 color : COLOR;
            };

            sampler2D _MainTex;
			float4 _RenderSphere;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.color = v.color;
                return o;
            }

			fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
				clip(distance(i.worldPos, _RenderSphere.xyz) < _RenderSphere.w ? 1 : -1);
				
                fixed4 col = tex2D(_MainTex, i.uv);
				clip(floor(col.a - 0.5));

                return col * _Color * i.color * col.a;
            }
            ENDCG
        }
    }
}
