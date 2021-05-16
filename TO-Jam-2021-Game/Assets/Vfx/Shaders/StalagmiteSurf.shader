Shader "Custom/StalagmiteSurf"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RenderSphere ("Render Sphere Vars", Vector) = (0,0,0,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "IgnoreProjector" = "True" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows


        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float4 color : COLOR;
			float3 worldPos : TEXCOORD1;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		float4 _RenderSphere;

		Input vert(inout appdata_full v)
		{
			Input o;
			o.worldPos = mul(unity_ObjectToWorld, v.vertex);
			o.color = v.color;
			return o;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			clip(distance(IN.worldPos, _RenderSphere.xyz) < _RenderSphere.w ? 1 : -1);

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
