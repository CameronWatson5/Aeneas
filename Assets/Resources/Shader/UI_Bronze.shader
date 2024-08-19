Shader "Unlit/UI_Bronze"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _BronzeColor ("Bronze Color", Color) = (0.361, 0.208, 0, 1)
        _HighlightColor ("Highlight Color", Color) = (0.7, 0.5, 0.3, 1)
        _Metallic ("Metallic", Range(0,1)) = 0.6
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _FresnelPower ("Fresnel Power", Range(0,10)) = 0.7
        _CenterHighlight ("Center Highlight Intensity", Range(0,1)) = 0.3
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float3 normal   : NORMAL;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
            };

            fixed4 _Color;
            fixed4 _BronzeColor;
            float _Metallic;
            float _Glossiness;
            float _FresnelPower;
            float _CenterHighlight;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = IN.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                OUT.worldNormal = UnityObjectToWorldNormal(IN.normal);
                OUT.viewDir = normalize(WorldSpaceViewDir(IN.vertex));
                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
                
                // Apply bronze base color
                fixed3 bronzeEffect = lerp(color.rgb, _BronzeColor.rgb, _Metallic);

                // Calculate specular highlight
                float3 lightDir = normalize(float3(1, 1, -1));
                float3 halfVector = normalize(lightDir + IN.viewDir);
                float specular = pow(max(0, dot(IN.worldNormal, halfVector)), _Glossiness * 100);

                // Calculate fresnel effect for edge highlighting
                float fresnel = pow(1.0 - saturate(dot(IN.worldNormal, IN.viewDir)), _FresnelPower);

                // Calculate center highlight
                float2 centeredUV = IN.texcoord * 2 - 1;
                float distanceFromCenter = length(centeredUV);
                float centerHighlight = 1 - distanceFromCenter;
                centerHighlight = pow(centerHighlight, 2) * _CenterHighlight;

                // Combine effects
                bronzeEffect += specular * _Glossiness;
                bronzeEffect += fresnel * _BronzeColor.rgb * 0.5;
                bronzeEffect += centerHighlight;

                color.rgb = bronzeEffect;
                return color;
            }
            ENDCG
        }
    }
}