sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity : register(C0);
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float4 Outline(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	if (any(color))
		return color;

	float dx = 1 / uImageSize0.x;
    float dy = 1 / uImageSize0.y;

	bool found=false;
	    for (int i = -1; i <= 1; i++) {
        for (int j = -1; j <= 1; j++) {
            float4 c = tex2D(uImage0, coords + float2(dx * i, dy * j));
			if (any(c)){
			found=true;
			}
		}}

		if (found)
	return float4(uColor.r, uColor.g, uColor.b, 1);
	return color;
}

technique Technique1
{
    pass ShaderOutline
    {
        PixelShader = compile ps_2_0 Outline();
    }
}