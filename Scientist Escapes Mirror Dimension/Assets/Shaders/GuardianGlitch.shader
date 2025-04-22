Shader "Custom/GuardianGlitch" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        [Header(Glitch Effect)]
        _GlitchIntensity ("Glitch Intensity", Range(0, 0.1)) = 0.01 // Keep range small initially
        _GlitchFrequency ("Glitch Frequency", Range(1, 100)) = 50
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow

        // Use shader model 3.0 target
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _GlitchIntensity;
        float _GlitchFrequency;

        // Simple pseudo-random noise function 
        // (Replace with Perlin/Simplex noise for smoother results if needed)
        float random(float3 pos) {
             // Combine spatial and temporal components for noise
            float spatial_comp = dot(pos.xz, float2(12.9898, 78.233));
            float time_comp = _Time.y * _GlitchFrequency * 0.1; // Vary speed with frequency
            return frac(sin(spatial_comp + time_comp) * 43758.5453);
        }

        // Vertex modification function
        void vert (inout appdata_full v) {
            // Get world position for consistent noise across the model
             float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
             
             // Generate noise value between 0 and 1
             float noise = random(worldPos);
             
             // Calculate offset along the vertex normal
             // We subtract 0.5 and multiply by 2 to make the noise range from -1 to 1
             // Then scale by intensity
             float3 offset = v.normal * (noise - 0.5) * 2.0 * _GlitchIntensity; 

             // Apply the offset in object space
             v.vertex.xyz += offset;
        }

        // Surface shader function
        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
} 