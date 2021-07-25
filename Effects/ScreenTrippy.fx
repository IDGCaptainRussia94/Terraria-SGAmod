texture overlayTexture;
sampler uImage0 = sampler_state
{
    Texture = (overlayTexture);
    AddressU = Wrap;
    AddressV = Wrap;
};

sampler uImage1 : register(s1); // Up to three extra textures you can use for various purposes (for instance as an overlay).
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition; // The position of the camera.
float2 uTargetPosition; // The "target" of the shader, what this actually means tends to vary per shader.
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect; // Doesn't seem to be used, but included for parity.
float2 uZoom;

//Shader by IDGCaptainRussia94

static const float tau = 6.283185307;

static const float pi = 3.14159265359;


//Except this part below, these following 2 functions came from here https://www.chilliant.com/rgb2hsv.html (Copyright © 2002-2020 Ian Taylor)
//This code has been slightly modified to meet the needs for this shader!

  float3 HUEtoRGB(in float H)
  {
    float R = abs(H * 6 - 3) - 1;
    float G = 2 - abs(H * 6 - 2);
    float B = 2 - abs(H * 6 - 4);
    return saturate(float3(R,G,B));
  }
  
  float3 HSVtoRGB(in float3 HSV)
  {
  float progress = uProgress%1.00;

  if (progress<0)
  progress = (1-uProgress)%1;


    float3 RGB = HUEtoRGB(((HSV.x*uDirection.x)+(uDirection.y))%1.00);

    return ((RGB - 1) * HSV.y + 1) * HSV.z;
  }


float4 Wave(float2 coords : TEXCOORD0) : COLOR0
{

    float4 oldcolor = tex2D(uImage0, coords);

    float2 adder = float2(uTargetPosition.x%uScreenResolution.x,uTargetPosition.y%uScreenResolution.y)/uScreenResolution;

    float ysin = 0;
    if (uColor.r>0)
    ysin = (sin((coords.x+adder.x+uProgress)*tau*(uColor.r*100)))*(uIntensity/uScreenResolution.y);

    float xcos = 0;
    if (uColor.g>0)
    xcos = (cos((coords.y+adder.y-uProgress)*tau*(uColor.g*100)))*(uIntensity/uScreenResolution.x);


    float2 newcoords = float2(coords.x+xcos,coords.y+ysin);


	float4 newcolor = tex2D(uImage0, newcoords);
        
        //float luminosity = (newcolor.r + newcolor.g + newcolor.b) / 3;
        //float2 reverse = float2(((1-newcoords.x))%1.0,((1-newcoords.y))%1.0);

        float4 overlaycolor = tex2D(uImage1,(float2((newcoords+uImageSize2)*0.40)%1.0));

        float3 colorthis = float3((newcoords.x-newcoords.y-(overlaycolor.g*0.10))%1.00,1,1);

        newcolor.rgb = HSVtoRGB(colorthis);//lerp(newcolor.rgb,HSVtoRGB(colorthis),uOpacity);//,oldcolor.rgb),overlaycolor.r);
        newcolor.a = oldcolor.a;


	return newcolor*uOpacity;
}

technique Technique1
{
    pass ScreenTrippy
    {
        PixelShader = compile ps_2_0 Wave();
    }
}