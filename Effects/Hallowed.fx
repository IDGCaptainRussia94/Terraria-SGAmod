sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float3 prismColor;
float prismAlpha;

texture overlayTexture;
float3 overlayProgress;
float overlayAlpha;
float3 overlayStrength;
float overlayMinAlpha;
float alpha;

sampler overlaytexsampler = sampler_state
{
    Texture = (overlayTexture);
    AddressU = Wrap;
    AddressV = Wrap;
};

static const float tau = 6.283185307;

static const float pi = 3.14159265359;

// This is a shader. You are on your own with shaders. Compile shaders in an XNB project.
//Shader by IDGCaptainRussia94 (2nd)

//Except this part below, these 2 functions came from here https://www.chilliant.com/rgb2hsv.html

  float3 HUEtoRGB(in float H)
  {
    float R = abs(H * 6 - 3) - 1;
    float G = 2 - abs(H * 6 - 2);
    float B = 2 - abs(H * 6 - 4);
    return saturate(float3(R,G,B));
  }
  
  float3 HSVtoRGB(in float3 HSV)
  {
    float3 RGB = HUEtoRGB((HSV.x+(overlayProgress.z*1.00))%1);
    return ((RGB - 1) * HSV.y + 1) * HSV.z;
  }


float4 PrismFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
    	if (!any(color))
		return color;

        float sinOffset = sin(((overlayStrength.z)+coords.y)*pi)*overlayStrength.y;

    float luminosity = (color.r + color.g + color.b) / 3;
    float4 white = float4(1, 1, 1,1);
    float3 blendedColor = color.rgb*prismColor.rgb;
    float3 unshadedColor = color.rgb;

    color.rgb = lerp(unshadedColor,blendedColor,prismAlpha);
    float3 shadedColor = color.rgb;

    if (overlayAlpha>0)
    {
        float4 colorOverlay = tex2D(overlaytexsampler, coords+float2((overlayProgress.x+sinOffset)%1.0,(overlayProgress.y)%1.0));
        if (colorOverlay.r > overlayMinAlpha)
        {
        colorOverlay.rgb = colorOverlay.rgb*overlayStrength.x;
        float saturation=1;
        float value = 1;
        float3 rainbowColor = HSVtoRGB(float3(coords.x%1,saturation,value));
        color.rgb = lerp(shadedColor,colorOverlay.rgb*float3(rainbowColor.r,rainbowColor.g,rainbowColor.b),(colorOverlay.g-overlayMinAlpha)*(overlayAlpha+overlayMinAlpha));
        }
    }

    color.rgb *= luminosity;
    color = saturate(color);

	return color*alpha;
}

technique Technique1
{
    pass Prism
    {
        PixelShader = compile ps_2_0 PrismFunction();
    }
}