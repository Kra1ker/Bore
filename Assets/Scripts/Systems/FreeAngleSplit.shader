Shader "Custom/FreeAngleSplit"
{
    Properties
    {
        _TexA ("Texture A", 2D) = "white" {}
        _TexB ("Texture B", 2D) = "white" {}
        _SplitDir ("Split Direction", Vector) = (1,0,0,0)
        _SplitOffset ("Split Offset", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _TexA;
            sampler2D _TexB;
            float4 _SplitDir;
            float _SplitOffset;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * 2 - 1;
                float d = dot(uv, normalize(_SplitDir.xy)) + _SplitOffset;

                if (d > 0)
                    return tex2D(_TexA, i.uv);
                else
                    return tex2D(_TexB, i.uv);
            }
            ENDCG
        }
    }
}
