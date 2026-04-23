Shader "Custom/DisplacementBlurEffect"
{
    Properties
    {
        _Color ("Tint Color", Color) = (1, 1, 1, 1) // New: Tint color property
        _MainTex ("Texture", 2D) = "white" {}
        _LightPos ("Light Position", Vector) = (0.5, 0.5, 0, 0)
        _Vector ("Displacement Vector", Vector) = (0.5, 0.5, 0, 0)
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

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

            fixed4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _LightPos;
            float4 _Vector;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 p = i.uv;

                float falloff = pow(saturate(1.0 - length(_LightPos.xy - p) * 1.0), 2.0);
                float2 m = -_Vector.xy * falloff * 1.5;
                
                fixed3 c = 0;

                [unroll]
                for(int j = 0; j < 10; j++)
                {
                    float s = 0.175 + 0.005 * j;

                    fixed r = tex2D(_MainTex, p + s * m).r;
                    fixed g = tex2D(_MainTex, p + (s + 0.025) * m).g;
                    fixed b = tex2D(_MainTex, p + (s + 0.05) * m).b;

                    c += fixed3(r, g, b);
                }

                c /= 10.0;
                
                return fixed4(c * _Color.rgb, 1.0);
            }
            ENDCG
        }
    }

    FallBack "Unlit/Texture"
}