sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float3 fadeColor;
float fadePercentSize;
float2 fadeMultiplier;
float2 fadeOffset;
float strength;
float alpha;
float3 blendColor;

static const float tau = 6.283185307;

static const float pi = 3.14159265359;

//Shader by IDGCaptainRussia94 (3nd)



float4 FadeFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
    	if (!any(color))
		return color;

        float2 fadedvector = fadeOffset+(fadeMultiplier*coords);

	    if (abs(fadedvector.x-coords.x)<fadePercentSize)
        {
        	    if (abs(fadedvector.y-coords.y)<fadePercentSize)
                {
                color.rgb = lerp(color.rgb*blendColor.rgb,fadeColor,strength);
                }
        }
	return color*alpha;
}

float4 FadeInFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
    	if (!any(color))
		return color;
        
                color.rgb = lerp(color.rgb*blendColor.rgb,fadeColor,strength);

	return color*alpha;
}

technique Technique1
{
    pass FadePass
    {
        PixelShader = compile ps_2_0 FadeFunction();
    }
        pass FadeIn
    {
        PixelShader = compile ps_2_0 FadeInFunction();
    }
}