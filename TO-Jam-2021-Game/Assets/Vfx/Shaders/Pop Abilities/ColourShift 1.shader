Shader "Custom/ColourShift (Animated)"
{
    Properties
    {
        _Color ("Colour", Color) = (0,0,0,0)
		_Color2 ("Colour 2", Color) = (0,0,0,0)
		_Noise ("Noise Texture", 2D) = "white" {}
		_NoiseSpeed ("Scroll Speed Multiplier", Float) = 1
		_CentrePos ("Centre Position", Vector) = (0,0,0,0)
		_ClipThresh ("Clip Threshold", Range(0.0, 1.0)) = 0.5
		_DistAdjust ("DistAdjust: Scale, Add", Vector) = (1,0,0,0)
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
				float2 uv : TEXCOORD0;
                float4 grabPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            sampler2D _GrabTex;
            float4 _Color;
			float4 _Color2;
			sampler2D _Noise;
			float _NoiseSpeed;
			float4 _CentrePos;
			fixed _ClipThresh;
			float2 _DistAdjust;

			fixed4 rotatedNoise(float4 pos, float centre, float angle) {
				float sina = sin(angle);
				float cosa = cos(angle);
				float3x3 rot = float3x3(cosa, -sina, 0, sina, cosa, 0, 0, 0, 1);
				return tex2D(_Noise, mul(rot, pos-centre)+centre);
			}

            fixed4 frag (v2f i) : SV_Target
            {
				float yStretch = 9.0 / 16.0; //stretch is based off aspect ratio
				
				//remove depth factor and stretch Noise texture to be centred and square on the screen
				float4 pos = i.grabPos;
				pos.y /= pos.w;
				pos.x /= pos.w;
				pos.y = pos.y * yStretch;// +yStretch * 0.4;

				//scale texture to the size of the sphere
				float3 cameraRight = mul((float3x3)unity_CameraToWorld, float3(1, 0, 0));
				float4 rightSide = ComputeGrabScreenPos(UnityObjectToClipPos(_CentrePos.xyz + cameraRight * _CentrePos.w));
				float4 leftSide = ComputeGrabScreenPos(UnityObjectToClipPos(_CentrePos.xyz - cameraRight * _CentrePos.w));
				rightSide /= rightSide.w;
				leftSide /= leftSide.w;
				float2 left2right = rightSide.xy - leftSide.xy;
				float width = sqrt(left2right.x * left2right.x + left2right.y * left2right.y);

				pos.xy /= width;

				//translate texture to be centred in the middle of the sphere
				float4 centre = ComputeGrabScreenPos(UnityObjectToClipPos(_CentrePos.xyz));

				centre.xyz /= centre.w;
				centre.y *= yStretch;
				pos -= centre / width;
				pos.xy += 0.5;

				//create an alpha map based off distance from centre
				float2 tempPos = i.grabPos.xy / i.grabPos.w;
				//tempPos.y *= yStretch;
				float2 vectFromCentre = tempPos - float2(centre.x, centre.y/yStretch);
				vectFromCentre.y *= yStretch;
				float distFromCentre = saturate(sqrt(vectFromCentre.x * vectFromCentre.x + vectFromCentre.y * vectFromCentre.y) /*sin(_Time[1]) **/ + _DistAdjust.y*width);
				distFromCentre /= width;
				distFromCentre *= distFromCentre * _DistAdjust.x;
				//return (fixed4)distFromCentre;

				//clip noise after rotating pixels around the z-axis according to distFromCentre (red channel rotated opposite to green, then multiplied together)
				fixed noise = rotatedNoise(pos, centre, sin(_Time[2]) * (1 - distFromCentre) * _NoiseSpeed).r;
				noise *= rotatedNoise(pos, centre, -cos(_Time[2]) * (1 - distFromCentre) * _NoiseSpeed).g;
				noise *= tex2D(_Noise, pos).b;
				noise *= noise;
				noise *= distFromCentre;

				fixed clipValue = noise / _ClipThresh - 2 * _ClipThresh;
				clipValue = distFromCentre > 1 ? 1 : clipValue;
				clipValue = distFromCentre < 0 ? -1 : clipValue;
				//clip(clipValue);

				//Set colour
				fixed4 activeColor = clipValue > 0 ? _Color2 : _Color;
				fixed4 col = tex2Dproj(_GrabTex, i.grabPos);
				col = fixed4(
					lerp(col.r, activeColor.r, activeColor.a),
					lerp(col.g, activeColor.g, activeColor.a),
					lerp(col.b, activeColor.b, activeColor.a),
					1
				);

                return col;
            }

			
            ENDCG
        }
    }
}
