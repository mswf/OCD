Shader "Custom/Lichtenstein-esque" {
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Number ("Size", Range(0,20)) = 7.0
	}
	SubShader 
	{
		 
		
		Pass
		{
			Tags { "RenderType"="Opaque" }
			LOD 200
		
			Blend SrcAlpha OneMinusSrcAlpha
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
				
			#include "UnityCG.cginc"
				
			uniform sampler2D _MainTex;
			uniform float _Number;
				
				struct v2f
				{
					float4 pos : SV_POSITION;
                    float2  uv : TEXCOORD0;
				}; 

				v2f vert(appdata_base v)
				{
					v2f o;
					
					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
					
					return o;
				}
			   
				float4 frag(v2f i) : COLOR
				{
					
					float radius = _Number * 0.5 * 0.75;
            	
            		// Current quad in pixels
            		
					float2 quadPos = floor(ComputerScreenPos.xy / _Number) * _Number;
					// Normalized quad position
					float2 quad = quadPos/uv.xy ;
				
					// Center of the quad
					float2 quadCenter = (quadPos + _Number/2.0 );
				
					// Distance to quad center	
					float dist = length(quadCenter - ComputerScreenPos.xy );
	
				
					if (dist > radius)
					{
						return float4(0.25);
					}
					else
					{
						return float4 (tex2D(_MainTex, uv), 0.0f, 1.0f);
					}           
					
					
				}
			ENDCG
		}
	}
	
	SubShader 
	{
		Pass
        {            
        	GLSLPROGRAM               
			
			#ifdef VERTEX
			
			varying vec2 uv;
			
			void main()
            {          
            	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
            	uv = gl_MultiTexCoord0.st;
            }
			
			#endif
			
			#ifdef FRAGMENT
			#include "UnityCG.glslinc"
			
			uniform sampler2D _MainTex;
			uniform float _Number;
			
			varying vec2 uv;
                
            void main(void)
            {
            	float radius = _Number * 0.5 * 0.75;
            	
            	// Current quad in pixels
				vec2 quadPos = floor(gl_FragCoord.xy / _Number) * _Number;
				// Normalized quad position
				vec2 quad = quadPos/uv.xy ;
				
				// Center of the quad
				vec2 quadCenter = (quadPos + _Number/2.0 );
				
				// Distance to quad center	
				float dist = length(quadCenter - gl_FragCoord.xy );
	
				
				if (dist > radius)
				{
					gl_FragColor = vec4(0.25);
				}
				else
				{
					vec4 texel = texture2D(_MainTex, uv);
					gl_FragColor = texel;
				}           
            }
			
			#endif
			
			ENDGLSL
		}
	} 
	FallBack "Diffuse"
}
