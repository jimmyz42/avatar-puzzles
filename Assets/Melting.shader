Shader "Custom/NewSurfaceShader" {
	Properties {
		    _Color ("Color", Color) = (1,1,1,1)
			_MainTex ("Albedo (RGB)", 2D) = "white" {}
			_DissolveScale ("Dissolve Progression", Range(0.0, 1.0)) = 0.0
			_DissolveTex("Dissolve Texture", 2D) = "white" {}
			_GlowIntensity("Glow Intensity", Range(0.0, 5.0)) = 0.05
			_GlowScale("Glow Size", Range(0.0, 5.0)) = 1.0
			_Glow("Glow Color", Color) = (1, 1, 1, 1)
			_GlowEnd("Glow End Color", Color) = (1, 1, 1, 1)
			_GlowColFac("Glow Colorshift", Range(0.01, 2.0)) = 0.75
			_DissolveStart("Dissolve Start Point", Vector) = (1, 1, 1, 1)
			_DissolveEnd("Dissolve End Point", Vector) = (0, 0, 0, 1)
			_DissolveBand("Dissolve Band Size", Float) = 0.25
		
	}
	SubShader {
		Tags {    
		"Queue" = "Transparent"
		"RenderType"="Fade"  }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade
		//#pragma surface surf Standard /*...any additional features you want...*/ alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;

		

		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		

//Precompute dissolve direction.
static float3 dDir = normalize(_DissolveEnd - _DissolveStart);

//Precompute gradient start position.
static float3 dissolveStartConverted = _DissolveStart - _DissolveBand * dDir;

//Precompute reciprocal of band size.
static float dBandFactor = 1.0f / _DissolveBand;
// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			//Convert dissolve progression to -1 to 1 scale.
			half dBase = -2.0f * _DissolveScale + 1.0f;
 
			//Read from noise texture.
			fixed4 dTex = tex2D(_DissolveTex, IN.uv_MainTex);
			//Convert dissolve texture sample based on dissolve progression.
			half dTexRead = dTex.r + dBase;
 
			//Set output alpha value.
			half alpha = clamp(dTexRead, 0.0f, 1.0f);
			//o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			//o.Alpha = c.a;
			o.Alpha = alpha;
		}
		void vert (inout appdata_full v, out Input o) 
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
 
			//Calculate geometry-based dissolve coefficient.
			//Compute top of dissolution gradient according to dissolve progression.
			float3 dPoint = lerp(dissolveStartConverted, _DissolveEnd, _DissolveScale);
 
			//Project vector between current vertex and top of gradient onto dissolve direction.
			//Scale coefficient by band (gradient) size.
			o.dGeometry = dot(v.vertex - dPoint, dDir) * dBandFactor;	
			
			//Combine texture factor with geometry coefficient from vertex routine.
			half dFinal = dTexRead + IN.dGeometry;
 
			//Clamp and set alpha.
			half alpha = clamp(dFinal, 0.0f, 1.0f);
			o.Alpha = alpha;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
