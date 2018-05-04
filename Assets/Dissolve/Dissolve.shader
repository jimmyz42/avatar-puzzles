Shader "N3K/Dissolve"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DissolveTexture("Disolve Texture", 2D)= "white" {}
		_DisolveY("Current Y of the dissolve effect", Float)= 0
		_DisolveSize("Size of the effect", Float) = 2
		_StartingY("Starting point of the effect", Float) = .04
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _DissolveTexture;
			float _DisolveY;
			float _DisolveSize;
			float _StartingY;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = v.vertex.xyz;//mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float transition = _DisolveY - i.worldPos.y;
				clip(_StartingY + (transition + (tex2D(_DissolveTexture, i.uv)) * _DisolveSize));
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
