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

// This is a shader. You are on your own with shaders. Compile shaders in an XNB project.

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	if (!any(color))
		return color;

	float2 there=float2((coords.x+(uTime/20.00))%1.00,(coords.y+(uTime/30.00))%1.00);

	float4 color1= tex2D( uImage1 , there);

	float readRed = uOpacity * 1.15;

	if((color1.r) > readRed)
		color.rgba = float4((color1.r-readRed)*2.00, 0,0, 1);


	float valz=((color1.r*3.00)*uOpacity);

	if (valz>1.00)
	valz=1.00;

	color.rgba *= valz;

	return color;
}

technique Technique1
{
    pass DeathAnimation
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}