using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

// Thanks to Shawn Hargreaves for this code!
// http://blogs.msdn.com/b/shawnhar/archive/2009/11/11/premultiplied-alpha-content-processor.aspx

namespace PremultipliedAlpha
{
	[ContentProcessor]
	class PremultipliedAlphaTextureProcessor : TextureProcessor
	{
		public override TextureContent Process(TextureContent input, ContentProcessorContext context)
		{
			input.ConvertBitmapType(typeof(PixelBitmapContent<Color>));

			foreach (MipmapChain mipChain in input.Faces)
			{
				foreach (PixelBitmapContent<Color> bitmap in mipChain)
				{
					for (int y = 0; y < bitmap.Height; y++)
					{
						for (int x = 0; x < bitmap.Width; x++)
						{
							Color c = bitmap.GetPixel(x, y);

							c.R = (byte)(c.R * c.A / 255);
							c.G = (byte)(c.G * c.A / 255);
							c.B = (byte)(c.B * c.A / 255);

							bitmap.SetPixel(x, y, c);
						}
					}
				}
			}

			return base.Process(input, context);
		}
	}
}