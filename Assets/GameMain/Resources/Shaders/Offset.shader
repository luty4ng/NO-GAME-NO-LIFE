Shader "Custom/Offset" {
    Properties{
        _MainTex("Main Tex",2D) = "white"{} 
        _Color("Color", Color) = (1,1,1,1) 
        _Speed ("Speed", Float) = 1

    }
    SubShader{
        Pass {
            Tags{"LightMode" = "ForwardBase"}
            CGPROGRAM
            #include "Lighting.cginc" 
            #pragma vertex vert
            #pragma fragment frag
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Speed;
            
            struct a2v
            {
                float4 vertex : POSITION;    
                float3 normal : NORMAL;        
                float4 texcoord : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 position : SV_POSITION; 
                float2 uv : TEXCOORD2;
            };
            
            v2f vert(a2v v)
            {
                v2f f;
                // f.position.y = f.position.y + _Time.y * _Speed; 
                f.position = UnityObjectToClipPos(v.vertex);
                f.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw * _Time.y * _Speed;
                return f;
            }
            
            fixed4 frag(v2f f) : SV_Target
            {
                fixed3 texColor = tex2D(_MainTex, f.uv.xy) * _Color.rgb;
                return fixed4(texColor, 1);
            }
            
            ENDCG
        }
        
    }
    FallBack "Diffuse"
}