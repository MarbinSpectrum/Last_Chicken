Shader "Custom/FluidSurfaceShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_AlphaCutout("Alpha Cutout", Range(0, 1)) = 0.15
		_RepeatSize("RepeatSize",Range(1, 20)) = 6
		_SimulationSpeed("SimulationSpeed",float) = 1
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

		Pass
		{
			//ZWrite required for proper rendering order of terrain layers
			ZWrite On
			Cull Off

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;
				return o;
			}

			sampler2D _MainTex;
			float _AlphaCutout;
			float _RepeatSize;
			float _SimulationSpeed;
			fixed4 frag(v2f i) : SV_Target
			{
				float temp = (_Time.y * _SimulationSpeed) % 1;
				temp *= _RepeatSize;
				temp = ceil(temp);
				temp /= _RepeatSize;

				fixed4 col = tex2D(_MainTex, i.uv + float2(temp,0)) * i.color;
				clip(col.a - _AlphaCutout);
				return col;
			}
			ENDCG
		}
	}
	Fallback "Sprites/Default"
}
