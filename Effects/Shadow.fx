sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

texture overlayTexture;
texture noiseTexture;

float colorAmmount;
float2 screenSize;
float4 colorFrom;
float4 colorTo;
float4 colorOutline;
float edgeSmooth;
float noisePercent;
float4 noiseScalar;

sampler overlaytexsampler = sampler_state
{
    Texture = (overlayTexture);
   AddressU = Wrap;
   AddressV = Wrap;
};

sampler noisesampler = sampler_state
{
    Texture = (noiseTexture);
   AddressU = Wrap;
   AddressV = Wrap;
};

static const float tau = 6.283185307;

static const float pi = 3.14159265359;

float4 ColorFilterFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 voxelOverlayTexture = tex2D(overlaytexsampler, coords);

    float2 noiseCoord = float2((coords.x+noiseScalar.x)*noiseScalar.z,(coords.y+noiseScalar.y)*noiseScalar.w);

        float4 noiseOverlayTexture = tex2D(noisesampler, noiseCoord);

    float luminosity = voxelOverlayTexture.r;//(voxelOverlayTexture.r + voxelOverlayTexture.g + voxelOverlayTexture.b) / 3;

        if (luminosity<=edgeSmooth)
        {
        float2 adderX = float2(1.0/screenSize.x,0);
        float2 adderY = float2(0,1.0/screenSize.y);

         float4 tex1 = tex2D(overlaytexsampler, coords-adderX);
         float4 tex2 = tex2D(overlaytexsampler, coords-adderY);
         float4 tex3 = tex2D(overlaytexsampler, coords+adderX);
        float4 tex4 = tex2D(overlaytexsampler, coords+adderY);

        if (tex1.r>edgeSmooth)
        return colorOutline;
                if (tex2.r>edgeSmooth)
        return colorOutline;
                if (tex3.r>edgeSmooth)
        return colorOutline;
                if (tex4.r>edgeSmooth)
        return colorOutline;

    discard;
    }

    float colorPercent = round(luminosity*colorAmmount)/colorAmmount;

    float4 colorLerp = lerp(colorFrom,lerp(colorTo,float4(noiseOverlayTexture.rgb,colorTo.a),noisePercent),colorPercent);

	return colorLerp;
}

technique Technique1
{
        pass ColorFilter
    {
        PixelShader = compile ps_2_0 ColorFilterFunction();
    }
}