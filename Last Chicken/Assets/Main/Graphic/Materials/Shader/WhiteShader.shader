Shader "Custom/WhiteShader"
{
	//외부에서 값을 가져올때 사용하는 연결고리
	//Material 쉐이더 파일을 컨트롤 하는 클래스
	//Shader : 픽셀을 계산하는 스크립트 코드(빛과 텍스쳐 , 계산수식을 사용하여 명암 채도등을 표현함) 질감을 표현하는 소스코드

	Properties
	{
		//_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}

	//쉐이더 코드의 진입 시점
	//디바이스에서 순차적으로 검토하고 실행할수있는 코드라면 실행함
	SubShader
	{
		// 옵션에 대한 우선 설정
		Tags
		{
		//알파값을 활용하여 투명하게 출력
		"RenderType" = "Transparent"
		//쉐이더의 우선순위값은 Transparent로 사용
		"Opaque" = "Transparent"
	}

		//거리에 따라서 출력되는 방식을 지정한다.
		//디테일하게 모델링을 출력할것인지 설정
		LOD 200

		//SrcAlpha : 계산된 Src의 알파값
		//OneMinusSrcAlpha : (1 - SrcAlpha)
		//Blend : (모니터에 출력된 색상값 * OneMinusSrcAlpha)
		Blend SrcAlpha OneMinusSrcAlpha

		//광원 연사을 하지않겠다는 뜻
		Lighting Off

		//앞면과 뒷면을 모두 그리겠다는 뜻
		Cull Off

		//surface 쉐이더 코드 진입시전을 나타냄
		//일반적으로 CGPROGRAM의 외부에 옵션을 설정하도록 하고
		//내부에 변수와 실행할 코드를 작성한다.
		CGPROGRAM

		//표면을 계산하는 함수의 이름은 surf
		//광원을 계산하는 방식은 Standard(내장함수)
		//그림자를 계산하는 방식은 fullforwardshadows(내장함수)
		//alpha:fade 알파연산을 하겠다는 옵션값
		#pragma surface surf Standard alpha:fade //fullforwardshadows

		#pragma target 3.0

		//2D 텍스쳐를 담는 공간
		sampler2D _MainTex;

		struct Input
		{
			//UV좌표계값
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			//tex2D : 현재 UV좌표계에 해당하는 픽셀값을 받아옴
			//첫번째 매개변수로 텍츠처 이미지
			//두번째 매개변수로 좌표값을 받음
			//좌표에 해당하는 픽셀값을 리턴해줌
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

			//Albedo : 빛과 연산될 색상값
			//o.Albedo = c.rgb;

			//Emission : 빛과 연산되지않을 순수 색상값
			o.Emission = fixed3(1, 1, 1);	//현재계산된 색상값
			//최종적인 출력값 : Albedo + Emission


			o.Alpha = c.a;		//모니터상에 출력되었는 색상값
		}
		ENDCG
	}

	//기본적으로 동작할 쉐이더
	FallBack "Diffuse"
}
