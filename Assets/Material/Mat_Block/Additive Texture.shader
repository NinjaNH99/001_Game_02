Shader "Mobile/SurfAdd" {
	Properties{
		_MainTex("Particle Texture", 2D) = "white" {}
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off Fog{ Color(0,0,0,0) }

		CGPROGRAM
#pragma exclude_renderers flash
#pragma surface surf Lambert vertex:vert noforwardadd

		sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex;
		float4 color : Color;
	};
	void vert(inout appdata_full v, out Input o) {
		UNITY_INITIALIZE_OUTPUT(Input,o);
		o.color = v.color;
	}

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb * IN.color.rgb;
		o.Alpha = c.a * IN.color.a;
	}

	ENDCG
	}
}
