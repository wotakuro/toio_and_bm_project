Shader "Unlit/BackGroundEffect"
{
    Properties
    {
        _LineSize("LineSize", Float) = 0.01
        _LinePad("LinePadding", Float) = 0.1
        _LineColor("LineColor", Color) = (0,1,1,1)
        _BaseColor("BaseColor", Color) = (0,1,1,1)
        _ScrollParam("Scroll",Vector)=(0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _LineSize;
            float _LinePad;
            float4 _LineColor;
            float4 _BaseColor;
            float4 _MainTex_ST;
            float4 _ScrollParam;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float getUvValue(float2 uv) {
                float xVal = fmod(uv.x + _LineSize * 0.5, _LinePad);
                float yVal = fmod(uv.y + _LineSize * 0.5, _LinePad);
                xVal = 1.0 - saturate(abs(xVal - _LineSize) / _LineSize);
                yVal = 1.0 - saturate(abs(yVal - _LineSize) / _LineSize);
                return max(xVal , yVal);
            }


            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col;
                float val = getUvValue(i.uv + _ScrollParam.xy);

                col.r = val * _LineColor.r + (1-val)* _BaseColor.r;
                col.g = val * _LineColor.g + (1 - val) * _BaseColor.g;
                col.b = val * _LineColor.b + (1 - val) * _BaseColor.b;
                col.a = 1;
                return col;
            }

            ENDCG
        }
    }
}
