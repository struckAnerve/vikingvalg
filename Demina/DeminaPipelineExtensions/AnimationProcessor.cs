using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace DeminaPipelineExtensions
{
	[ContentProcessor(DisplayName = "Demina Animation Processor")]
	public class AnimationProcessor : ContentProcessor<AnimationContent, AnimationContent>
	{
		public override AnimationContent Process(AnimationContent animationContent, ContentProcessorContext context)
		{
			foreach (TextureEntryContent textureEntryContent in animationContent.Textures)
			{
				string loadPath = Path.GetFullPath(Path.Combine(animationContent.Directory, textureEntryContent.LoadPath));

				if (textureEntryContent.UseDictionary)
				{
					string asset = string.Format("{0}{1}", "tdict_", Path.GetFileNameWithoutExtension(loadPath));

					textureEntryContent.TextureDictionary = null;

					textureEntryContent.TextureDictionary = context.BuildAsset<TextureDictionaryContent, TextureDictionaryContent>(
						new ExternalReference<TextureDictionaryContent>(loadPath),
						"TextureDictionaryProcessor", null, "TextureDictionaryImporter", asset);
				}
				else
				{
					string[] directories = animationContent.Directory.Split(Path.DirectorySeparatorChar);

					string asset = string.Format("{0}{1}{2}{3}", "texture_", directories[directories.Length - 1], "_", Path.GetFileNameWithoutExtension(loadPath));

					OpaqueDataDictionary data = new OpaqueDataDictionary();
					data.Add("GenerateMipmaps", false);
					data.Add("ResizeToPowerOfTwo", false);
					data.Add("TextureFormat", TextureProcessorOutputFormat.Color);
					data.Add("ColorKeyEnabled", false);

                    // Note: The PremultipliedAlphaTextureProcessor is likely redundant, since the TextureProcessor
                    //       use premultiplied alpha by default now.
					textureEntryContent.Texture = context.BuildAsset<TextureContent, TextureContent>(
						new ExternalReference<TextureContent>(loadPath),
						"PremultipliedAlphaTextureProcessor", data, "TextureImporter", asset);
				}
			}

			return animationContent;
		}
	}
}
