using System.Xml;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace DeminaPipelineExtensions
{
	[ContentSerializerRuntimeType("Demina.TextureEntry, DeminaRuntime")]
	public class TextureEntryContent
	{
		public string LoadPath { get; set; }
		public string Name { get; set; }

		public bool UseDictionary { get; set; }

		public ExternalReference<TextureContent> Texture { get; set; }
		public ExternalReference<TextureDictionaryContent> TextureDictionary { get; set; }

		public TextureEntryContent(XmlNode xmlNode, ContentImporterContext context)
		{
			if (xmlNode.Attributes["dictionary"] != null && xmlNode.Attributes["name"] != null)
			{
				LoadPath = xmlNode.Attributes["dictionary"].Value;
				Name = xmlNode.Attributes["name"].Value;
				UseDictionary = true;
			}
			else
			{
				LoadPath = xmlNode.InnerText;
                Name = xmlNode.InnerText;
				UseDictionary = false;
			}
		}
	}
}
