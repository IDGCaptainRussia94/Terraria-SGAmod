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


texture distortionTexture;
sampler distortionTextureSampler = sampler_state
{
    Texture = (distortionTexture);
   AddressU = Wrap;
   AddressV = Wrap;
};

//Shader by IDGCaptainRussia94

static const float tau = 6.283185307;

static const float pi = 3.14159265359;

float4 Distort(float2 coords : TEXCOORD0) : COLOR0
{
	
	        //float2 pixelX = float2(1.0/uScreenResolution.x,0);
        //float2 pixelY = float2(0,1.0/uScreenResolution.y);
        float2 uvpos = coords.xy;
            uvpos.x *= uScreenResolution.x / uScreenResolution.y;

	float4 color= tex2D( uImage0 , coords); 
	float2 position = uTargetPosition;
	float2 dister = position-coords.xy;
	float dist = clamp(((distance(uvpos.xy,position)*uColor.g)-uColor.r),0.0,1.0);
	
	float distort = (1.0-clamp(dist*uIntensity,0,1))*uOpacity;
	
				float4 colornoise= tex2D( distortionTextureSampler , ((coords+uTime)-(dist*distort*0.25))%1.0); 
		float4 color2= tex2D( uImage0 , coords+(dist*distort*(0.15+colornoise.r*0.20))); 
	
	
		float3 luma = (color2.r+color2.g+color2.b)/3.0;

	return lerp(float4(0,0,0,0),float4(luma,color2.a),smoothstep(0,1,distort)); 
}

technique Technique1
{
    pass TimeDistort
    {
        PixelShader = compile ps_2_0 Distort();
    }
}