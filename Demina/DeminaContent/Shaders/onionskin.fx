sampler TextureSampler : register(s0);

float3 Color;
float Alpha;

float4 OnionSkinShader(float2 texCoord : TEXCOORD0) : COLOR0
{
    // Look up the texture color.
    float4 c = tex2D(TextureSampler, texCoord);
	float grayscale = dot(float3(0.3, 0.59, 0.11), c);
	return float4(grayscale * Color, min(c.a, c.a * Alpha));	
}

technique onionskin
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 OnionSkinShader();
    }
}
