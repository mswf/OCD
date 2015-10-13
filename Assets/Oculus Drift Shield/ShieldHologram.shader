Shader "Custom/ShieldHologram" 
{
	Properties 
	{
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
	}
	SubShader 
	{
		//Tags { "RenderType"="Transperant" }
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			uniform fixed4 _Color;
			
			struct vertInput
			{
				float4 vertex : POSITION;
			};
			
			struct vertOutput 
			{
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
			};
			
			
			vertOutput vert(vertInput v)
			{
				vertOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = _Color;
				return o;
			}
			
			half4 frag(vertOutput i) : COLOR
			{
				return i.color;
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
