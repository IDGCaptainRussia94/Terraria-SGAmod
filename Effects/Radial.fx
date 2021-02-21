sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

texture overlayTexture;
float2 texMultiplier;
float2 texOffset;
float alpha;
float ringScale;
float ringOffset;
bool tunnel;
sampler overlaytexsampler = sampler_state
{
    Texture = (overlayTexture);
    AddressU = Wrap;
    AddressV = Wrap;
};

static const float tau = 6.283185307;

static const float pi = 3.14159265359;

//Shader by IDGCaptainRussia94 (4nd)

float4 RadialFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(overlaytexsampler, coords);
    	if (!any(color))
		return color;

    float2 radianCoords = (2 * coords - 1) * 1.125;
    
    float dist = abs(sqrt((radianCoords.x * radianCoords.x) + (radianCoords.y * radianCoords.y)));

    if (dist>1.00)
    {
    color = 0;
    return color;
    }



    float radianAngle = atan2(radianCoords.y, radianCoords.x);
        radianAngle -= tau * round(radianAngle / tau);

        float localDist = dist;
        if (tunnel)
        localDist = 0.50/dist;

        float2 overlayTextCoords = float2(abs(((radianAngle * 0.5 / pi + 0.500)*(texMultiplier.x))+texOffset.x)%1.00,abs((localDist*texMultiplier.y)+texOffset.y)%1.00);
        //float2 texOffset = float2(abs(texMultiplier.x),(texMultiplier.y))

        //float2 overlayTexCoords = float2(abs((overlayTextCoordsPre.x+(texOffset.x))*texMultiplier.x)%1.00,abs((overlayTextCoordsPre.y+(texOffset.y))*texMultiplier.y)%1.00);

    float4 newColor = tex2D(overlaytexsampler, overlayTextCoords)*clamp(1.000000-(abs(dist-ringOffset)/ringScale),0.000000,1.000000);

	return newColor*alpha;
}

technique Technique1
{
        pass Radial
    {
        PixelShader = compile ps_2_0 RadialFunction();
    }
}