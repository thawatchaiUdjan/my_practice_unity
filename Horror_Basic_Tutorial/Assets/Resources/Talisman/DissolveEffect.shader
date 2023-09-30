Shader "Custom/DissolveEffect"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_DissolveTexture ("Dissolve Map", 2D) = "white" {}
		_Amount ("Dissolve Amount", Range(0,1)) = 0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		_EdgeColor ("Edge Color",Color) = (1,1,1,1)
		_EdgeWidth ("Edge Thickness", Range(0.01, 0.2)) = 0.05

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
		// Cull Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

		sampler2D _DissolveTexture;
		half _Amount;
		half _EdgeWidth;
		half4 _EdgeColor;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			half dissolve_value = tex2D(_DissolveTexture, IN.uv_MainTex).r;
			clip(dissolve_value - _Amount);
			o.Emission = _EdgeColor * step(dissolve_value - _Amount, _EdgeWidth);
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
