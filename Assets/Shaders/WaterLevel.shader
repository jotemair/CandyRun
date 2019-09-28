// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Camera/WaterLevel"
{
	Properties
	{
		// _MainTex (with the underscore) is sort of a keyword. In case of camera shaders it will contain the image rendered by the camera
		// Has to be added as a property as well to function
		_MainTex("Main Texture", 2D) = "white" {}
	}

	SubShader
	{
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
				float4 clipPos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 wpos : TEXCOORD1;
				float3 vpos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
			};

			// Connection for the _MainTex
			sampler2D _MainTex;

			// Connection for accessing builtin depth texture
			// Camera DepthTextureMode needs to be Depth or DepthNormals
			// https://docs.unity3d.com/Manual/SL-CameraDepthTexture.html
			sampler2D _CameraDepthTexture;

			// Parameters set from script
			sampler2D _WaterTexture;

			// Texture properties
			// https://docs.unity3d.com/Manual/SL-PropertiesInPrograms.html
			// For _TexelSize, x contains 1.0/width, y contains 1.0 / height, z contains width, w contains height
			float4 _WaterTexture_TexelSize;

			float4 _Vector_X;
			float4 _Vector_Y;
			float4 _Screen_Center;

			float _WaterLevel;

			v2f vert(appdata IN)
			{
				v2f OUT;

				OUT.clipPos = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;
				OUT.wpos = mul(unity_ObjectToWorld, IN.vertex).xyz;
				OUT.vpos = IN.vertex.xyz;
				OUT.screenPos = ComputeScreenPos(OUT.clipPos);

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				float4 texColor = tex2D(_MainTex, IN.uv);

				float depthValue = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)).r);

				float3 worldSpacePos = (_Vector_X * IN.screenPos.x + _Vector_Y * IN.screenPos.y + _Screen_Center) * depthValue + _WorldSpaceCameraPos;

				float3 viewDir = worldSpacePos - _WorldSpaceCameraPos;
				float surfaceDistRatio = (_WaterLevel - _WorldSpaceCameraPos.y) / viewDir.y;
				float3 waterSurfacePoint = _WorldSpaceCameraPos + viewDir * surfaceDistRatio;

				float4 water = tex2D(_WaterTexture, float2(waterSurfacePoint.x % _WaterTexture_TexelSize.z, waterSurfacePoint.z % _WaterTexture_TexelSize.w));

				float4 finalColor = (worldSpacePos.y < _WaterLevel) ? water : texColor;

				return finalColor;
			}

			ENDCG
		}
	}

	FallBack "Diffuse"
}
