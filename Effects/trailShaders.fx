matrix WorldViewProjection;

//Shader largly by Boffin and used and modified with permission!

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

//custom passes
texture imageTexture;
sampler imageSampler = sampler_state
{
    Texture = (imageTexture);
    AddressU = Wrap;
    AddressV = Wrap;
};

//These 2 functions came from here https://www.chilliant.com/rgb2hsv.html

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


VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;

	output.TextureCoordinates = input.TextureCoordinates;

    return output;
}


//Recreated Basic Effect
float4 BasicEffect(VertexShaderOutput input) : COLOR
{
	float4 pixel = (tex2D(imageSampler, coordOffset + input.TextureCoordinates * coordMultiplier) * input.Color)*strength;
	pixel = saturate(pixel);
	return pixel;
}

//Same as above, but now faded on the X axis
float4 BasicEffectFaded(VertexShaderOutput input) : COLOR
{

	float4 pixel = (tex2D(imageSampler, coordOffset + input.TextureCoordinates * coordMultiplier) * input.Color)*strength;
	pixel = saturate(pixel);
	return pixel * (sin(input.TextureCoordinates.x * 3.14159265));
}

//Simple color gradient fade
float4 MainPS(VertexShaderOutput input) : COLOR
{
	return input.Color * (sin(input.TextureCoordinates.x * 3.14159265)) * strength;
}

//The above but on both axis
float4 MainPSSinShade(VertexShaderOutput input) : COLOR
{
	float base = sin(input.TextureCoordinates.y * 3.14159265);

	return input.Color * (sin(input.TextureCoordinates.x * 3.14159265)*base) * strength;
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
		pass FadedBasicEffectPass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 BasicEffectFaded();
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