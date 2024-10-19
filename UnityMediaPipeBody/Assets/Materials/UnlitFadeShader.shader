Shader "Custom/UnlitFadeByDistance"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MinDistance ("Min Fade Distance", Float) = 10
        _MaxDistance ("Max Fade Distance", Float) = 30
        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
                float distanceToCamera : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _MinDistance;
            float _MaxDistance;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // Calculate distance to the camera
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 cameraPos = _WorldSpaceCameraPos;
                o.distanceToCamera = distance(worldPos, cameraPos);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate opacity based on the distance
                float opacity = saturate((i.distanceToCamera - _MinDistance) / (_MaxDistance - _MinDistance));

                // Apply the fade logic (more opaque when far, more transparent when close)
                // When distance = MaxDistance, opacity should be 1 (fully opaque)
                // When distance = MinDistance, opacity should be 0 (fully transparent)
                
                fixed4 texColor = tex2D(_MainTex, i.uv);
                texColor.a *= opacity; // Adjust the alpha by the calculated opacity

                return texColor * _Color;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Unlit"
}