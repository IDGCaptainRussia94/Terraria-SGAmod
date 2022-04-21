sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float glow;
float bloomPercent;
float strength;


float4 bloomPixel(float2 uv : TEXCOORD,float4 color,float2 offset,float bloomPercent) : COLOR 
{
float2 screenSize = uImageSize0;
float frameY = (uv.y * screenSize.y - uSourceRect.y) / uSourceRect.w;

  float2 adder = float2(offset.x/screenSize.x,offset.y/frameY);


	float2 offset2 = uv+adder;

	float4 colorBloom= tex2D( uImage0 , offset2);


return lerp(colorBloom,color,bloomPercent);
}

float4 BloomMethod(float2 uv : TEXCOORD,float2 bloomPercent,float glow) : COLOR 
{ 
	
	float divde = 1.0;
	float4 color; 
	color= tex2D( uImage0 , uv.xy); 

    float bloomish = 0.5*bloomPercent;
	
	color += bloomPixel(uv,color,float2(-1,-1),bloomish);
	color += bloomPixel(uv,color,float2(1,-1),bloomish);
	color += bloomPixel(uv,color,float2(-1,1),bloomish);
	color += bloomPixel(uv,color,float2(1,1),bloomish);

        bloomish = 0.25*bloomPercent;
	
		color += bloomPixel(uv,color,float2(-2,-2),bloomish);
	color += bloomPixel(uv,color,float2(2,-2),bloomish);
	color += bloomPixel(uv,color,float2(-2,2),bloomish);
	color += bloomPixel(uv,color,float2(2,2),bloomish);

	divde += 8.0;

	return (color/divde)*(glow+1.0); 
}

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    return BloomMethod(coords,0.50+sin((uTime+(coords.x*180.00)+(coords.y*240.00))/1200.00)/2.0,0.25);
}

float4 BloomPassFunction(float2 coords : TEXCOORD0) : COLOR0
{
    return BloomMethod(coords,bloomPercent,glow)*strength;
}

technique Technique1
{
    pass Bloom
    {
        PixelShader = compile ps_2_0 BloomPassFunction();
    }
    pass BloomDyePass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}