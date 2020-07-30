Shader "Custom/ChangeColorSprite"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[Space(20)]
		[MaterialToggle]_UseColor1("컬러사용",int) = 0
		_ChangeColor1("변경색상1", Color) = (1,1,1,1)
		_Color1("변경되는색상1", Color) = (1,1,1,1)
		[Space(20)]
		[MaterialToggle]_UseColor2("컬러사용",int) = 0
		_ChangeColor2("변경색상2", Color) = (1,1,1,1)
		_Color2("변경되는색상2", Color) = (1,1,1,1)
		[Space(20)]
		[MaterialToggle]_UseColor3("컬러사용",int) = 0
		_ChangeColor3("변경색상3", Color) = (1,1,1,1)
		_Color3("변경되는색상3", Color) = (1,1,1,1)
		[Space(20)]
		[MaterialToggle]_UseColor4("컬러사용",int) = 0
		_ChangeColor4("변경색상4", Color) = (1,1,1,1)
		_Color4("변경되는색상4", Color) = (1,1,1,1)
		[Space(20)]
		Power("강도", Range(0,1)) = 0.1
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
				};

				fixed4 _Color;
				int _UseColor1;
				fixed4 _ChangeColor1;
				fixed4 _Color1;
				int _UseColor2;
				fixed4 _ChangeColor2;
				fixed4 _Color2;
				int _UseColor3;
				fixed4 _ChangeColor3;
				fixed4 _Color3;
				int _UseColor4;
				fixed4 _ChangeColor4;
				fixed4 _Color4;
				float Power;
				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color * _Color;
					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _AlphaTex;
				sampler2D _MaskTex;
				float _AlphaSplitEnabled;
				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);
	#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
	#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				float LL(fixed4 a, fixed4 b)
				{
					float r = sqrt((a.r - b.r)*(a.r - b.r) + (a.g - b.g)*(a.g - b.g) + (a.b - b.b)*(a.b - b.b));
					//r = normalize(r);
					return r;
				}

				float3 LM(fixed4 a, fixed4 b, fixed4 c)
				{
					float rr = a.r / b.r * c.r;
					float gg = a.g / b.g * c.g;
					float bb = a.b / b.b * c.b;

					return float3(rr,gg,bb);
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					fixed4 c = SampleSpriteTexture(IN.texcoord);

					if (LL(c, _ChangeColor1) <= Power && _UseColor1)
						c.rgb = LM(c, _ChangeColor1, _Color1);
					else if (LL(c, _ChangeColor2) <= Power && _UseColor2)
						c.rgb = LM(c, _ChangeColor2, _Color2);
					else if (LL(c, _ChangeColor3) <= Power && _UseColor3)
						c.rgb = LM(c, _ChangeColor3, _Color3);
					else if (LL(c, _ChangeColor4) <= Power && _UseColor4)
						c.rgb = LM(c, _ChangeColor4, _Color4);
					c *= IN.color;
					c.rgb *= c.a;
					return c;
				}
			ENDCG
			}
		}
}
