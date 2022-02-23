#define PI 3.14159265359
#define TWO_PI 6.28318530718

//Shader originally by Boffin and used and heavily modified by IDGCaptainRussia94 with permission! Do not use without his (Boffin) permission!
//This is a heavily skimmed down version of TrailShaders.fx

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

matrix WorldViewProjection;
float2 coordMultiplier;
float2 coordOffset;
float strength;
float time;

//custom passes
texture imageTexture;
sampler imageSampler = sampler_state
{
    Texture = (imageTexture);
    AddressU = Wrap;
    AddressV = Wrap;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;

	output.TextureCoordinates = input.TextureCoordinates;

    return output;
}

float4 RotateTexture(VertexShaderOutput input) : COLOR
{ 
float2 uv = input.TextureCoordinates;
	uv = (uv+coordOffset)*coordMultiplier;
	//uv.x = uv.x%1.0;
	//uv.y = uv.y%1.0;
	float2 center = float2(uv.xy-0.5);
	float piTime = time*TWO_PI;
	
	float rotSin = sin(piTime);
		float rotCos = cos(piTime);
	
	
	float2x2 rotationMatrix = float2x2(rotCos,-rotSin,rotSin,rotCos);
	float2 uv2 = mul(center,rotationMatrix );
	float4 colorFinal= tex2D( imageSampler , uv2)*input.Color; 

	return colorFinal*strength; 
}

technique BasicColorDrawing
{
	pass RotateTexturePass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 RotateTexture();
	}
}