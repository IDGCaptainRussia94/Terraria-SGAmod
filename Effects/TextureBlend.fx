sampler2D input : register(s0);

texture Texture;
sampler texsampler = sampler_state
{
    Texture = (Texture);
    AddressU = Wrap;
    AddressV = Wrap;
};

texture noiseTexture;
sampler noisetexsampler = sampler_state
{
    Texture = (noiseTexture);
    AddressU = Wrap;
    AddressV = Wrap;
};

float2 coordMultiplier;
float2 coordOffset;

float2 noiseMultiplier;
float2 noiseOffset;

float strength = 1.0;
float4 colorTo;
float4 colorFrom;

float noiseProgress = 0;
float noiseBlendPercent = 1.0;
float textureProgress = 0;

static const float tau = 6.283185307;

static const float pi = 3.14159265359;


float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	
			float2 noiseCoord = ((uv.xy+noiseOffset)*noiseMultiplier);
	float4 noiseColor = tex2D( noisetexsampler , noiseCoord);
	
	float2 coord = ((uv.xy+coordOffset)*coordMultiplier);
	float3 textureColor1 = tex2D( texsampler , coord+float2(textureProgress, 0)).xyz;
	float3 textureColor2 = tex2D( texsampler , coord+float2(-textureProgress*1.5, 0)).xyz/2.0;
	float4 textureColor = float4((textureColor1+textureColor2)/1.0,1.0);
	
		float luma = (textureColor.r+textureColor.g+textureColor.b)/3.0;
			float luma2 = (noiseColor.r+noiseColor.g+noiseColor.b)/3.0;
	
		float lerppcent = (((luma+luma2)/2.0)+noiseProgress);
	textureColor = lerp(textureColor,noiseColor,noiseBlendPercent);
	
	float endcolorfloat = lerppcent;
	float4 endcolor2 = lerp(colorFrom,colorTo,0.50+sin(endcolorfloat*tau)/2.0);


	return textureColor*endcolor2*strength; 
}

technique Technique1
{
        pass TextureBlend
    {
        PixelShader = compile ps_2_0 main();
    }
}