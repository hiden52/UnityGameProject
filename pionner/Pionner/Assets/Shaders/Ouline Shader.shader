// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Outline Shader"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0,0,0,1),
        _OutlineWidth ("Outline Width", Float) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Overlay+1" }  //  "Overlay" 대신 "Overlay+1" 사용

        // 아웃라인을 위한 Pass (두 번째 모델을 렌더링)
        Pass {
            Name "OUTLINE"
            Tags { "LightMode"="Always" }

            ZWrite On
            ZTest LessEqual
            Cull Front
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha  //  CGPROGRAM 바깥쪽에 위치해야 함

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
            };

            uniform float _OutlineWidth;   //  uniform 추가
            uniform float4 _OutlineColor;  //  uniform 추가

            v2f vert(appdata v)
            {
                v2f o;
                // 외곽선을 위해 모델을 약간 확장
                o.pos = UnityObjectToClipPos(v.vertex + normalize(v.normal) * _OutlineWidth);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }

        // 기본 모델을 위한 Pass (기본 렌더링)
        Pass {
            Name "OUTLINE_MODEL"
            Tags { "LightMode"="Always" }

            ZWrite On
            ZTest LessEqual
            Cull Back
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha  //  CGPROGRAM 바깥쪽에 위치해야 함

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return half4(1, 1, 1, 1); // 기본 모델 색상 (흰색)
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"  //  FallBack은 SubShader 바깥쪽에 위치해야 함
}
