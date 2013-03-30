using System.IO;

using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace DeminaPipelineExtensions
{
	[ContentProcessor(DisplayName = "Texture Dictionary Processor")]
	public class TextureDictionaryProcessor : ContentProcessor<TextureDictionaryContent, TextureDictionaryContent>
	{
		public override TextureDictionaryContent Process(TextureDictionaryContent textureDictionaryContent, ContentProcessorContext context)
		{
			string texturePath = Path.GetFullPath(Path.Combine(textureDictionaryContent.Directory, textureDictionaryContent.TexturePath));
			string asset = string.Format("{0}{1}", Path.GetFileNameWithoutExtension(textureDictionaryContent.Filename), "_texture");

			OpaqueDataDictionary data = new OpaqueDataDictionary();
			data.Add("GenerateMipmaps", false);
			data.Add("ResizeToPowerOfTwo", false);
			data.Add("TextureFormat", TextureProcessorOutputFormat.Color);
			data.Add("ColorKeyEnabled", false);
			textureDictionaryContent.Texture = context.BuildAsset<TextureContent, TextureContent>(
				new ExternalReference<TextureContent>(texturePath),
				"PremultipliedAlphaTextureProcessor", data, "TextureImporter", asset);

			return textureDictionaryContent;
		}
	}
}
