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
        Tags { "Queue"="Overlay+1" }  //  "Overlay" ��� "Overlay+1" ���

        // �ƿ������� ���� Pass (�� ��° ���� ������)
        Pass {
            Name "OUTLINE"
            Tags { "LightMode"="Always" }

            ZWrite On
            ZTest LessEqual
            Cull Front
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha  //  CGPROGRAM �ٱ��ʿ� ��ġ�ؾ� ��

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

            uniform float _OutlineWidth;   //  uniform �߰�
            uniform float4 _OutlineColor;  //  uniform �߰�

            v2f vert(appdata v)
            {
                v2f o;
                // �ܰ����� ���� ���� �ణ Ȯ��
                o.pos = UnityObjectToClipPos(v.vertex + normalize(v.normal) * _OutlineWidth);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }

        // �⺻ ���� ���� Pass (�⺻ ������)
        Pass {
            Name "OUTLINE_MODEL"
            Tags { "LightMode"="Always" }

            ZWrite On
            ZTest LessEqual
            Cull Back
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha  //  CGPROGRAM �ٱ��ʿ� ��ġ�ؾ� ��

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
                return half4(1, 1, 1, 1); // �⺻ �� ���� (���)
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"  //  FallBack�� SubShader �ٱ��ʿ� ��ġ�ؾ� ��
}
