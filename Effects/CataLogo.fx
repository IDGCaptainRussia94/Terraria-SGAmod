sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

texture overlayTexture;

float edges = 3.0;
float angleAdd = 0;
float ballSize = 0.10;
float edgeSize = 0.40;
float ballEdgeGap = 0.05;


sampler overlaytexsampler = sampler_state
{
    Texture = (overlayTexture);
   AddressU = Wrap;
   AddressV = Wrap;
};

static const float tau = 6.283185307;

static const float pi = 3.14159265359;

float4 CataLogoFunction(float2 coords : TEXCOORD0) : COLOR0
{

            float4 fragColor = float4(0,0,0,0);

            // Normalized pixel coordinates (from 0 to 1)
            float2 uv = coords;

            float2 center = uv - 0.5;

            float r = (atan2(center.y, center.x) + angleAdd) / tau;
            float colring = r * edges;//dot(r, center);'

            float dist = length(center);
            float sizer = ballSize + ballEdgeGap;

            if ((colring%1.0 < 0.5) && ((dist > sizer && dist < edgeSize)))// || dist < ballSize)
            {

            float3 rgb = float3(1.0,1.0,1.0);

                //if (dist < ballSize)
                    //rgb = rgb*(1.0 - (dist / ballSize) * 0.50);
                //else

                    rgb = rgb*(((0.500 + sin(colring * tau) * 0.400)%1.00) * (1.0 - dist));
                return float4(rgb.rgb,1.0);
            }

            return fragColor;

}

float4 CataLogoBallFunction(float2 coords : TEXCOORD0) : COLOR0
{

            float4 fragColor = float4(0,0,0,0);

            float2 uv = coords;

            float2 center = uv - 0.5;

            float dist = length(center);

            if (dist < ballSize)
            {
            float3 rgb = float3(1.0,1.0,1.0);
            rgb = rgb*(1.0 - (dist / ballSize) * 0.50);
                return float4(rgb.rgb,1.0);
            }

            return fragColor;

}

float4 CataLogoBallInverseFunction(float2 coords : TEXCOORD0) : COLOR0
{

            float4 fragColor = float4(1.0,1.0,1.0,1.0);

            float2 uv = coords;

            float2 center = uv - 0.5;

            float dist = length(center);

            if (dist < ballSize)
            {
            float3 rgb = float3(1.0,1.0,1.0);
            rgb = rgb*(1.0 - (dist / ballSize) * 0.50);
                return float4(rgb.rgb,1.0-rgb.r);
            }

            return fragColor;

}

float4 CataLogoInverseFunction(float2 coords : TEXCOORD0) : COLOR0
{

            float4 fragColor = float4(1.0,1.0,1.0,1.0);

            // Normalized pixel coordinates (from 0 to 1)
            float2 uv = coords;

            float2 center = uv - 0.5;

            float r = (atan2(center.y, center.x) + angleAdd) / tau;
            float colring = r * edges;//dot(r, center);'

            float dist = length(center);

            //if (dist>edgeSize+0.03)
            //discard;

            float sizer = ballSize + ballEdgeGap;

            if ((colring%1.0 < 0.5) && ((dist > sizer && dist < edgeSize)))// || dist < ballSize)
            {

            float3 rgb = float3(1.0,1.0,1.0);

                //if (dist < ballSize)
                    //rgb = rgb*(1.0 - (dist / ballSize) * 0.50);
                //else

                    float alphaz = (((1.00 - sin(colring * tau) * 0.50)%1.00));
                return float4(rgb,1.0)*(alphaz*(dist/edgeSize));
            }

            return fragColor;

}

technique Technique1
{
        pass CataLogo
    {
        PixelShader = compile ps_2_0 CataLogoFunction();
    }
    pass CataLogoInverse
    {
        PixelShader = compile ps_2_0 CataLogoInverseFunction();
    } 
    pass CataLogoBall
    {
        PixelShader = compile ps_2_0 CataLogoBallFunction();
    }
    pass CataLogoBallInverse
    {
        PixelShader = compile ps_2_0 CataLogoBallInverseFunction();
    }


}