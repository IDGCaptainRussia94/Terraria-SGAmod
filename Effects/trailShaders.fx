matrix WorldViewProjection;

//Shader originally by Boffin and used and heavily modified by IDGCaptainRussia94 with permission! Do not use without his (Boffin) permission!

struct VertexShaderInput
{
	float2 TextureCoordinates : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float2 TextureCoordinates : TEXCOORD0;
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

float2 coordMultiplier;
float2 coordOffset;
float strength;
float rainbowScale;
float rainbowProgress;
float yFade;
float strengthPow;

//custom passes
texture imageTexture;
sampler imageSampler = sampler_state
{
    Texture = (imageTexture);
    AddressU = Wrap;
    AddressV = Wrap;
};

float2 rainbowCoordMultiplier;
float2 rainbowCoordOffset;
float3 rainbowColor;
texture rainbowTexture;
sampler rainbowSampler = sampler_state
{
    Texture = (rainbowTexture);
    AddressU = Wrap;
    AddressV = Wrap;
};

//These following 2 functions came from here https://www.chilliant.com/rgb2hsv.html (Copyright © 2002-2020 Ian Taylor)
//While not to the extent as in Hallowed.fx, This code also has been modified slightly to meet my needs for this shader

  float3 HUEtoRGB(in float H)
  {
    float R = abs(H * 6 - 3) - 1;
    float G = 2 - abs(H * 6 - 2);
    float B = 2 - abs(H * 6 - 4);
    return saturate(float3(R,G,B));
  }
  
  float3 HSVtoRGB(in float3 HSV)
  {
  float progress = rainbowProgress;

  if (progress<0)
  progress = (1-progress)%1;

    float3 RGB = HUEtoRGB(((HSV.x*rainbowScale)+(progress))%1);
    return ((RGB - 1) * HSV.y + 1) * HSV.z;
  }







    float3 HSVtoRGBFromTexture(VertexShaderOutput input,float2 coords,float3 HSV)
  {
  	float4 pixel = (tex2D(rainbowSampler, rainbowCoordOffset + input.TextureCoordinates * rainbowCoordMultiplier) * input.Color)*strength;

    float3 RGB = HUEtoRGB(((pixel.r+HSV.x)*rainbowScale)%1);
    return ((RGB - 1) * HSV.y + 1) * HSV.z;
  }


VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;

	output.TextureCoordinates = input.TextureCoordinates;

    return output;
}

float4 ColorPow(float4 color)
{

if (strengthPow>0)
{

float4 color2 = color;
float luma = (color2.r+color2.g+color2.b)/3.0;
color2 = color*pow(luma,strengthPow);

return color2;
}
return color;
}

//Applies a rainbow overlay
float4 RainbowEffect(VertexShaderOutput input) : COLOR
{
    float2 coords = coordOffset + input.TextureCoordinates * coordMultiplier;
	float4 pixel = (tex2D(imageSampler, coords) * input.Color)*strength;
	float4 rainpixel = (float4(HSVtoRGBFromTexture(input,coords,rainbowColor),1.0) * input.Color)*strength;
	pixel = saturate(pixel*rainpixel);
	return ColorPow(pixel);
}

//Sets the alpha color based on luminosity and applies a rainbow overlay
float4 RainbowEffectAlpha(VertexShaderOutput input) : COLOR
{
    float2 coords = coordOffset + input.TextureCoordinates * coordMultiplier;
	float4 pixel = (tex2D(imageSampler, coords) * input.Color)*strength;
	float4 rainpixel = (float4(HSVtoRGBFromTexture(input,coords,rainbowColor),1.0) * input.Color)*strength;
	float luma = (pixel.r+pixel.g+pixel.b)/3.0;
	pixel = saturate(pixel*rainpixel);
	return ColorPow(pixel)*luma;
}

//The 2 above but now a fade on the X axis
//Applies a rainbow overlay
float4 RainbowEffectFaded(VertexShaderOutput input) : COLOR
{
    float2 coords = coordOffset + input.TextureCoordinates * coordMultiplier;
	float4 pixel = (tex2D(imageSampler, coords) * input.Color)*strength;
	float4 rainpixel = (float4(HSVtoRGBFromTexture(input,coords,rainbowColor),1.0) * input.Color)*strength;
	pixel = saturate(pixel*rainpixel);
	return ColorPow(pixel) * (min(sin(input.TextureCoordinates.x * 3.14159265)*yFade,1.0000000));
}

//Sets the alpha color based on luminosity and applies a rainbow overlay
float4 RainbowEffectAlphaFaded(VertexShaderOutput input) : COLOR
{
    float2 coords = coordOffset + input.TextureCoordinates * coordMultiplier;
	float4 pixel = (tex2D(imageSampler, coords) * input.Color)*strength;
	float4 rainpixel = (float4(HSVtoRGBFromTexture(input,coords,rainbowColor),1.0) * input.Color)*strength;
	float luma = (pixel.r+pixel.g+pixel.b)/3.0;
	pixel = saturate(pixel*rainpixel);
	return ColorPow(pixel)*luma* (min(sin(input.TextureCoordinates.x * 3.14159265)*yFade,1.0000000));
}

//Recreated Basic Effect
float4 BasicEffect(VertexShaderOutput input) : COLOR
{
	float4 pixel = (tex2D(imageSampler, coordOffset + input.TextureCoordinates * coordMultiplier) * input.Color)*strength;
	pixel = saturate(pixel);
	return ColorPow(pixel);
}

//Sets the alpha color based on luminosity and returns a black color
float4 BasicEffectDark(VertexShaderOutput input) : COLOR
{
	float4 pixel = (tex2D(imageSampler, coordOffset + input.TextureCoordinates * coordMultiplier) * input.Color)*strength;
	float luma = (pixel.r+pixel.g+pixel.b)/3.0;
	//pixel = saturate(pixel);
	return float4(0,0,0,luma);
}

//Sets the alpha color based on luminosity
float4 BasicEffectAlpha(VertexShaderOutput input) : COLOR
{
	float4 pixel = (tex2D(imageSampler, coordOffset + input.TextureCoordinates * coordMultiplier) * input.Color)*strength;
	float luma = (pixel.r+pixel.g+pixel.b)/3.0;
	pixel = saturate(pixel);
	return ColorPow(pixel)*luma;
}

//Sets the alpha color based on luminosity and fades on X
float4 BasicEffectAlphaFaded(VertexShaderOutput input) : COLOR
{
	float4 pixel = (tex2D(imageSampler, coordOffset + input.TextureCoordinates * coordMultiplier) * input.Color)*strength;
	float luma = (pixel.r+pixel.g+pixel.b)/3.0;
	pixel = saturate(pixel);
	return ColorPow(pixel)*luma*(min(sin(input.TextureCoordinates.x * 3.14159265)*yFade,1.0000000));
}

//Faded on the X axis
float4 BasicEffectFaded(VertexShaderOutput input) : COLOR
{
	float4 pixel = (tex2D(imageSampler, coordOffset + input.TextureCoordinates * coordMultiplier) * input.Color)*strength;
	pixel = saturate(pixel);
	return ColorPow(pixel) * (min(sin(input.TextureCoordinates.x * 3.14159265)*yFade,1.0000000));
}

//Same as above, but now faded on the Y axis
float4 BasicEffectFadedY(VertexShaderOutput input) : COLOR
{
	float4 pixel = (tex2D(imageSampler, coordOffset + input.TextureCoordinates * coordMultiplier) * input.Color)*strength;
	pixel = saturate(pixel);
	return ColorPow(pixel) * (min(sin(input.TextureCoordinates.y * 3.14159265)*yFade,1.0000000));
}







//Simple color gradient fade
float4 MainPS(VertexShaderOutput input) : COLOR
{
	return input.Color * (sin(input.TextureCoordinates.x * 3.14159265)) * strength;
}

//The above but on both axis
float4 MainPSSinShade(VertexShaderOutput input) : COLOR
{
    float2 coords = (2 * input.TextureCoordinates - 1) * 1.125;
    float dist = abs(sqrt((coords.x * coords.x) + (coords.y * coords.y)));
	float4 pixel = input.Color * (1.00-clamp(dist,0,1));
	pixel = saturate(pixel);

	return pixel * strength;
}

//not used, purpose not known, left over code by Boffin
float4 BasicImage(VertexShaderOutput input) : COLOR
{
    float alpha = (1.0 - strength) + tex2D(imageSampler, coordOffset + input.TextureCoordinates * coordMultiplier).r * strength;
	return input.Color * alpha;
}

technique BasicColorDrawing
{
	pass BasicEffectPass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 BasicEffect();
	}	
		pass RainbowEffectPass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 RainbowEffect();
	}				
	pass RainbowEffectAlphaPass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 RainbowEffectAlpha();
	}			
	pass FadedRainbowEffectPass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 RainbowEffectFaded();
	}				
	pass FadedRainbowEffectAlphaPass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 RainbowEffectAlphaFaded();
	}		
	pass BasicEffectAlphaPass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 BasicEffectAlpha();
	}	
	pass BasicEffectDarkPass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 BasicEffectDark();
	}			
	pass FadedBasicEffectAlphaPass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 BasicEffectAlphaFaded();
	}	
		pass FadedBasicEffectPass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 BasicEffectFaded();
	}
		pass FadedBasicEffectPassY
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 BasicEffectFadedY();
	}	
		pass DefaultPass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 MainPS();
	}
		pass DefaultPassSinShade
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 MainPSSinShade();
	}
	pass BasicImagePass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 BasicImage();
	}
}