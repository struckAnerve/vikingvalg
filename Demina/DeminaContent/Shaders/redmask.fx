// draw a translucent red mask instead of the real pixels

sampler TextureSampler : register(s0);

float4 RedMaskShader(float2 texCoord : TEXCOORD0) : COLOR0
{
    // Look up the texture color.
    float4 tex = tex2D(TextureSampler, texCoord);
    
    if(tex.a > 0)
		return float4(1, 0, 0, 0.1);
    else
		return float4(0, 0, 0, 0);
}


technique Desaturate
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 RedMaskShader();
    }
}
