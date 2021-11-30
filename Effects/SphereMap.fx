sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

texture mappedTexture;
float2 mappedTextureMultiplier;
float2 mappedTextureOffset;
float4 colorBlend;
float softEdge;
sampler mappedTexturesampler = sampler_state
{
    Texture = (mappedTexture);
    AddressU = Wrap;
    AddressV = Wrap;
};

//(back)ported from this to HLSL, I (IDGCaptainRussia94) did not originally write this:
//https://www.shadertoy.com/view/wdSXRz
//Thank you!

static const float tau = 6.283185307;

static const float pi = 3.14159265359;

float2 Distortion(float2 uv)
{

    uv -= 0.5;
    //uv.x *= iResolution.x / iResolution.y;
    uv *= 2.0;

    if (length(uv) > 1.0)
    {
    	discard;
    }

    uv = lerp(uv,normalize(uv)*(2.0*asin(length(uv)) / pi),0.5);
    //float3 normal = float3(uv, sqrt(1.0 - uv.x*uv.x - uv.y*uv.y));
    uv = normalize(uv)*(2.0*asin(length(uv)) / pi);

    uv += mappedTextureOffset;

    return uv;

}

float softTheEdge(float2 coords)
{
if (softEdge<=0)
return 1.0;

return clamp(softEdge-length(coords)*softEdge,0.0,1.0);
}

float4 SphereMapFunction(float2 fragCoord : TEXCOORD0) : COLOR0
{

    float2 oguv = fragCoord-0.5;
    oguv *= 2.0;

	float2 uv = Distortion(fragCoord);//fragCoord.xy / iResolution.xy;

    float4 overlaycolor = tex2D(mappedTexturesampler, uv*mappedTextureMultiplier);

	float4 fragColor = overlaycolor;

    return overlaycolor*colorBlend*softTheEdge(oguv);

}

float4 SphereMapAlphaFunction(float2 fragCoord : TEXCOORD0) : COLOR0
{

    float2 oguv = fragCoord-0.5;
    oguv *= 2.0;

	float2 uv = Distortion(fragCoord);//fragCoord.xy / iResolution.xy;

    float4 overlaycolor = tex2D(mappedTexturesampler, uv*mappedTextureMultiplier);

    float luma = (overlaycolor.r+overlaycolor.g+overlaycolor.b)/3.00;

    return overlaycolor*colorBlend*luma*softTheEdge(oguv);

}

technique Technique1
{
        pass SphereMap
    {
        PixelShader = compile ps_2_0 SphereMapFunction();
    }
            pass SphereMapAlpha
    {
        PixelShader = compile ps_2_0 SphereMapAlphaFunction();
    }
}