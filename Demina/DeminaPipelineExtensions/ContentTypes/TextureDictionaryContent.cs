using System.Xml;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace DeminaPipelineExtensions
{
	[ContentSerializerRuntimeType("Demina.TextureDictionary, DeminaRuntime")]
	public class TextureDictionaryContent
	{
		public string Version { get { return "1.0"; } }

		[ContentSerializerIgnore]
		public string Filename { get; set; }
		[ContentSerializerIgnore]
		public string Directory { get; set; }

		[ContentSerializerIgnore]
		public string TexturePath { get; protected set; }
		public ExternalReference<TextureContent> Texture { get; set; }

		public Dictionary<string, TextureBoundsContent> TextureCollection { get; protected set; }

		public TextureDictionaryContent(XmlDocument xmlDocument, ContentImporterContext context)
		{
			TextureCollection = new Dictionary<string, TextureBoundsContent>();

			XmlNode pathNode = xmlDocument.SelectSingleNode("/TextureDictionary/TexturePath");
			TexturePath = pathNode.InnerText;

			XmlNodeList textureNodeList = xmlDocument.SelectNodes("/TextureDictionary/Texture");
			foreach (XmlNode textureNode in textureNodeList)
			{
				TextureBoundsContent textureBoundsContent = new TextureBoundsContent(textureNode, context);
				TextureCollection.Add(textureBoundsContent.Name, textureBoundsContent);
			}
		}
	}
}
