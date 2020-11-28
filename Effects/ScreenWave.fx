sampler uImage0 : register(s0); // The contents of the screen.
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

static const float tau = 6.283185307;

static const float pi = 3.14159265359;

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


	return newcolor;
}

technique Technique1
{
    pass ScreenWave
    {
        PixelShader = compile ps_2_0 Wave();
    }
}