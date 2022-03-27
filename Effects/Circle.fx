sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float size;
float power;
float ringSize;
float ringThickness;
float strength;
float4 colorTo;
float4 colorFrom;

float4 frag(float2 uv : TEXCOORD0) : COLOR0 {

float2 coord = ((uv)-float2(0.5,0.5))*2.0;
float len = (abs(ringSize-(dot(coord,coord))))/size; 
	
	
	float finallen = pow(len,power)-ringThickness;
	
	float4 color = lerp(colorFrom,colorTo,1.0-clamp(finallen,0.0,1.0));

	return color*strength;
}

technique Technique1 
{
	pass Circle 
	{
        PixelShader = compile ps_2_0 frag();
    }
}