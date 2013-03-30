using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DeminaPipelineExtensions
{
	[ContentTypeWriter]
	public class AnimationWriter : ContentTypeWriter<AnimationContent>
	{
		protected override void Write(ContentWriter output, AnimationContent value)
		{
			output.Write(value.Version);
			output.Write(value.FrameRate);
			output.Write(value.LoopFrame);

			output.Write(value.Textures.Count);
			foreach (TextureEntryContent textureEntryContent in value.Textures)
			{
				output.Write(textureEntryContent.UseDictionary);
				if (textureEntryContent.UseDictionary)
				{
					output.WriteExternalReference(textureEntryContent.TextureDictionary);
					output.Write(textureEntryContent.Name);
				}
				else
				{
					output.WriteExternalReference(textureEntryContent.Texture);
				}
			}

			output.WriteObject(value.Keyframes);
		}

		public override string GetRuntimeReader(TargetPlatform targetPlatform)
		{
			return "Demina.AnimationReader, DeminaRuntime";
		}

		public override string GetRuntimeType(TargetPlatform targetPlatform)
		{
			return "Demina.Animation, DeminaRuntime";
		}
	}
}
