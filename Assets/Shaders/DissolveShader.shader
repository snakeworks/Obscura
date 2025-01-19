Shader "Custom/Dissolve" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _DissolveThreshold("Dissolve Threshold", Range(0, 1)) = 0
        _EdgeColor("Edge Color", Color) = (0, 0, 0, 0)
    }

        SubShader{
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _DissolveThreshold;
                float4 _EdgeColor;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float noiseScale = 1.0;
                    fixed noise = frac(sin(dot(i.uv * noiseScale ,float2(12.9898,78.233))) * 43758.5453);
                    noise = noise * 0.5 + 0.5; // Remap the noise

                    fixed4 col = tex2D(_MainTex, i.uv);
                    fixed4 edgeCol = _EdgeColor;
                    edgeCol.a = col.a;

                    if (noise < _DissolveThreshold) {
                        clip(-1);
                    }
                    else if (noise < _DissolveThreshold + 0.1) {
                        return edgeCol;
                    }

                    UNITY_APPLY_FOG(i.fogCoord, col);
                    return col;
                }
                ENDCG
            }
        }
}
