Shader "Unlit/SpectrumShader"
{
    Properties 
    {
        _Offset ("Colour offset", Float) = 0
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
				float4 screenPos : TEXCOORD1;
			};

			float _Offset;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// animate the _Offset property to scroll the colours
				float colourOffset = i.screenPos.y + _Offset;
				fixed4 col = float4((colourOffset * 10) % 1.0f, colourOffset % 1.0f, (colourOffset / 10) % 1.0f, 1.0f);
				return col;
			}
			ENDCG
		}
	}
}
