Shader "Custom/RingProgressBar_FullCircleFade"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _BarFill("_BarFill", Range(0.0, 1.0)) = 0.2
        _BarWidthOutSide("_BarWidthOutSide", Range(0.01, 1.0)) = 0.2
        _BarWidthInside("_BarWidthInside", Range(0.01, 1.0)) = 0.1
        _ColorEmpty("_ColorEmpty", Color) = (1,0,0,0.5) // 半透明红色
        _ColorFill("_ColorFill", Color) = (0,1,0,0.5) // 半透明绿色
        _FadeStart("_FadeStart", Range(0.0, 1.0)) = 0.25 // 渐变开始比例
        _FadeRange("_FadeRange", Range(0.0, 0.5)) = 0.2 // 渐变范围比例
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _BarFill;
            float _BarWidthOutSide;
            float _BarWidthInside;
            float4 _ColorEmpty;
            float4 _ColorFill;
            float _FadeStart;
            float _FadeRange;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // 计算圆环内外边界
                float outerRadius = 0.5f - _BarWidthOutSide * 0.5f;
                float innerRadius = outerRadius - (_BarWidthOutSide - _BarWidthInside);

                // 判断UV是否在环形内
                float dist = length(i.uv - 0.5f);
                float inRing = step(dist, outerRadius) - step(dist, innerRadius);

                // 计算角度，考虑正负方向，确保覆盖整个环形
                float angle = atan2(i.uv.y - 0.5f, i.uv.x - 0.5f);
                if (angle < 0) angle += 2 * 3.14159; // 负角度转为正角度
                angle /= (2 * 3.14159); // 角度归一化到0到1
                // 控制渐变的起始和结束位置，确保覆盖整个环形
                float fade = smoothstep(1 - _FadeStart - _FadeRange, 1 - _FadeStart, 1 - angle);
                fade = 1 - fade; // 反向渐变，使渐变从开始位置到结束位置逐渐透明
                // 根据进度和角度控制颜色和透明度
                half4 color = lerp(_ColorEmpty, _ColorFill, _BarFill);
                color.a *= inRing * fade; // 结合环形和角度渐变控制透明度
                clip((color.a - 0.01));
                return color;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}