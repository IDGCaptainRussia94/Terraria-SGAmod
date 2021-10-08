texture _VoronoiTex;
float4 _CellColor = float4(1, .75, 0, 1); // Orange
float4 _EdgeColor = float4(1, .5, 0, 1); // Yellow-Orange
float2 _CellSize = float2(1.5, 2.0);
float _ScrollSpeed = 0.04;
float _FadeSpeed = 3;
float _ColorScale = 1.5652475842498528; // .7*sqrt(5)
float _Time; // Pass the time in seconds into here

sampler _Voronoi = sampler_state {
	Texture = (_VoronoiTex);
	AddressU = Wrap;
    AddressV = Wrap;
};

float4 frag(float2 uv : TEXCOORD0) : COLOR0 {
	float4 noise = tex2D(_Voronoi, float2(uv.x + _Time * _ScrollSpeed, uv.y) / _CellSize);
	return lerp(_EdgeColor, _CellColor, noise.z * _ColorScale * (cos(noise.y * _Time * _FadeSpeed + noise.x) * .75 + .5));
}

technique Technique1 {
	pass Star {
        PixelShader = compile ps_2_0 frag();
    }
}