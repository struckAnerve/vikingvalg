using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DeminaPipelineExtensions
{
	[ContentTypeWriter]
	public class TextureDictionaryWriter : ContentTypeWriter<TextureDictionaryContent>
	{
		protected override void Write(ContentWriter output, TextureDictionaryContent value)
		{
			output.Write(value.Version);
			output.WriteExternalReference(value.Texture);
			output.WriteObject(value.TextureCollection);
		}

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Demina.TextureDictionaryReader, DeminaRuntime";
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "Demina.TextureDictionary, DeminaRuntime";
        }
	}
}
