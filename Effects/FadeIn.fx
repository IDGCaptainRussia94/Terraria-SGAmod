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

float4 SmoothYFadeFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);

    	if (!any(color))
		return color;

            float luminosity = (color.r + color.g + color.b) / 3;
            color.rgb = lerp(color.rgb*blendColor.rgb,fadeColor*luminosity,strength);

        	if (coords.y>fadeOffset.y)
            {
            float alphablend = ((coords.y-fadeOffset.y)*fadeMultiplier.y);

                color *= clamp(alphablend,0.0,1.0);
                if (color.a == 0)
                discard;
            }else
            {
            discard;
            }

	return color*alpha;
}

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

float4 NoAlphaFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
    	if (!any(color))
		return color;
        
        color = float4(color.rgb,1.0);

	return color*alpha;
}

float4 ColorToAlphaFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
    	if (!any(color))
		return color;
        float luma = (color.r+color.g+color.b)/3.0;
        
        color = float4(color.rgb*fadeColor,luma);

	return color*alpha;
}
float4 LumaRecolorFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
    	if (!any(color))
		return color;
        float luma = (color.r+color.g+color.b)/3.0;
        
        color = float4(fadeColor.rgb*(0.5+(luma/2.0)),color.a);

	return color*alpha;
}
float4 LumaRecolorAlphaFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
    	if (!any(color))
		return color;
        float luma = (color.r+color.g+color.b)/3.0;
        
        color = float4(fadeColor.rgb*(0.5+(luma/2.0)),luma);

	return color*luma*alpha;
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
        pass SmoothYFadePass
    {
        PixelShader = compile ps_2_0 SmoothYFadeFunction();
    }
            pass NoAlphaPass
    {
        PixelShader = compile ps_2_0 NoAlphaFunction();
    }
                pass ColorToAlphaPass
    {
        PixelShader = compile ps_2_0 ColorToAlphaFunction();
    }
                pass LumaRecolorPass
    {
        PixelShader = compile ps_2_0 LumaRecolorFunction();
    }                
    pass LumaRecolorAlphaPass
    {
        PixelShader = compile ps_2_0 LumaRecolorAlphaFunction();
    }
}