Shader "Custom/Blobby"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Colour", Color) = (1,1,1,1)
		_Alpha ("Alpha Multiplier", Range(0, 1)) = 1
		_Stretch ("Stretch", Vector) = (0,0,0,0)
		_Speed ("Scroll Speed", Float) = 1
		_Outline ("Outline Thickness", Range(0, 1)) = 0
		_ClipThresh ("Clip Threshold", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "PreviewType" = "Plane"}
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off ZTest Always

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
				float4 color : COLOR;
            };

            sampler2D _MainTex;
			fixed4 _Color;
			fixed _Alpha;
			float3 _Stretch;
            float4 _MainTex_ST;
			float _Speed;
			float _Outline;
			float _ClipThresh;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed noise = 1 - tex2D(_MainTex, float2(i.uv.x * _Stretch.x, i.uv.y * _Stretch.y + _Time[2] * _Speed) % 1).r;
				noise *= tex2D(_MainTex, float2(i.uv.x * _Stretch.x + 0.5, i.uv.y * _Stretch.y + 0.5 + _Time[2] * _Speed) % 1).r;
				noise *= i.uv.y;
				noise *= _Alpha * i.color.a;

				fixed outline = saturate(floor(_Outline * _Alpha * i.color.a + i.uv.y));
				clip(noise - _ClipThresh + outline);

				fixed4 col = _Color * i.color;
				col.a *= saturate(1 - (1 - i.uv.y) / (1 - _ClipThresh));
				col.a *= _Alpha;
                return col;
            }
            ENDCG
        }
    }
}
