Shader "TextMeshPro/BitmapWithShadow" {

	Properties {
		_MainTex        ("Font Atlas", 2D) = "white" {}
		_FaceTex        ("Font Texture", 2D) = "white" {}
		[HDR]_FaceColor ("Text Color", Color) = (1,1,1,1)
		
		_OutlineColor   ("Outline Color", Color) = (0,0,0,1)  // Outline color property
		_OutlineWidth   ("Outline Width", Float) = 1.0         // Outline width property in pixels
	
		_VertexOffsetX  ("Vertex OffsetX", float) = 0
		_VertexOffsetY  ("Vertex OffsetY", float) = 0
		_MaskSoftnessX  ("Mask SoftnessX", float) = 0
		_MaskSoftnessY  ("Mask SoftnessY", float) = 0
	
		_ClipRect("Clip Rect", vector) = (-32767, -32767, 32767, 32767)
	
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
	
		_CullMode("Cull Mode", Float) = 0
		_ColorMask("Color Mask", Float) = 15
	}
	
	SubShader{
	
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
	
		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
	
		Lighting Off
		Cull [_CullMode]
		ZTest [unity_GUIZTestMode]
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]
	
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
	
			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
	
			#include "UnityCG.cginc"
	
			struct appdata_t {
				float4 vertex        : POSITION;
				fixed4 color        : COLOR;
				float2 texcoord0    : TEXCOORD0;
				float2 texcoord1    : TEXCOORD1;
			};
	
			struct v2f {
				float4    vertex        : SV_POSITION;
				fixed4    color        : COLOR;
				float2    texcoord0    : TEXCOORD0;
				float2    texcoord1    : TEXCOORD1;
				float4    mask        : TEXCOORD2;
			};
	
			uniform    sampler2D     _MainTex;
			uniform    sampler2D     _FaceTex;
			uniform float4        _FaceTex_ST;
			uniform    fixed4        _FaceColor;
			
			uniform float        _VertexOffsetX;
			uniform float        _VertexOffsetY;
			uniform float4        _ClipRect;
			uniform float        _MaskSoftnessX;
			uniform float        _MaskSoftnessY;
			
			uniform fixed4       _OutlineColor;  // Outline color
			uniform float        _OutlineWidth;  // Outline width in pixels
	
			v2f vert (appdata_t v)
			{
				float4 vert = v.vertex;
				vert.x += _VertexOffsetX;
				vert.y += _VertexOffsetY;
	
				vert.xy += (vert.w * 0.5) / _ScreenParams.xy;
	
				float4 vPosition = UnityPixelSnap(UnityObjectToClipPos(vert));
	
				fixed4 faceColor = v.color;
				faceColor *= _FaceColor;
	
				v2f OUT;
				OUT.vertex = vPosition;
				OUT.color = faceColor;
				OUT.texcoord0 = v.texcoord0;
				OUT.texcoord1 = TRANSFORM_TEX(v.texcoord1, _FaceTex);
	
				return OUT;
			}
	
			// Function to check if a surrounding pixel is part of the text
			fixed4 SampleOutline(float2 uv, float outlineOffset, sampler2D tex) {
				return tex2D(tex, uv + outlineOffset);
			}
	
			fixed4 frag (v2f IN) : SV_Target
			{
				float2 uv = IN.texcoord0;
				fixed4 baseColor = tex2D(_MainTex, uv);
				fixed4 color = fixed4(tex2D(_FaceTex, IN.texcoord1).rgb * IN.color.rgb, IN.color.a * baseColor.a);

				// Sample surrounding pixels for the outline
				float2 pixelSize = 1.0 / _ScreenParams.xy;
				
				// Offsets for all 8 directions (left, right, top, bottom, and diagonals)
				float2 offsets[8] = { 
					float2(-1,  0), // Left
					float2( 1,  0), // Right
					float2( 0, -1), // Down
					float2( 0,  1), // Up
					float2(-1, -1), // Bottom Left
					float2( 1, -1), // Bottom Right
					float2(-1,  1), // Top Left
					float2( 1,  1)  // Top Right
				};
				
				float alphaThreshold = 0.1;
				fixed outlineAlpha = 0.0;

				// Iterate through all surrounding pixels using the offsets
				
				float2 sampleOffset = offsets[6] * pixelSize * _OutlineWidth;
				outlineAlpha += step(alphaThreshold, tex2D(_MainTex, uv + sampleOffset).a);
				

				// If the current pixel is transparent, but one of the surrounding pixels is part of the text, apply the outline
				if (baseColor.a < alphaThreshold && outlineAlpha > 0.0) {
					return _OutlineColor;
				}

				// Otherwise, render the text normally
				return color;
			}
			ENDCG
		}
	}
	
		CustomEditor "ShaderGUI"
	}
	