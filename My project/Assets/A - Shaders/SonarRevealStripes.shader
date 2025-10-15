Shader "Custom/SonarRevealStripes"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0,0,0,1)
        _StripeColor("Stripe Color", Color) = (1,1,1,1)
        _StripeCount("Stripe Count", Float) = 10
        _StripeFade("Stripe Fade Width", Float) = 0.005
        _Fade("Reveal Thickness", Float) = 0.1
        _OrbCenter("Orb Center", Vector) = (0,0,0,0)
        _OrbRadius("Orb Radius", Float) = 0.0
        _StripeSpeed("Stripe Speed", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Pass
        {
            Name "StripeReveal"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _BaseColor;
            float4 _StripeColor;
            float _StripeCount;
            float _StripeFade;
            float _Fade;
            float3 _OrbCenter;
            float _OrbRadius;
            float _StripeSpeed;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.worldPos = worldPos;
                OUT.positionHCS = TransformWorldToHClip(worldPos);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float dist = distance(IN.worldPos, _OrbCenter);

                // Outside the orb → fully black
                if (dist > _OrbRadius || dist < _OrbRadius - _Fade)
                    return _BaseColor;

                // Distance from the near edge of the orb
                float distFromSurface = dist - (_OrbRadius - _Fade);

                // Stripe spacing along the reveal thickness
                float stripeSpacing = _Fade / _StripeCount;

                // Stripe animation along the radial distance
                float t = _Time.y * _StripeSpeed;
                float stripeNorm = frac(distFromSurface / stripeSpacing - t);

                // Thin fading stripes
                float band = smoothstep(0.5 - _StripeFade, 0.5 + _StripeFade, stripeNorm);

                // Fade edge of orb
                float fadeEdge = smoothstep(_OrbRadius, _OrbRadius - _Fade, dist);

                float mask = band * fadeEdge;

                return lerp(_BaseColor, _StripeColor, mask);
            }
            ENDHLSL
        }
    }
}
