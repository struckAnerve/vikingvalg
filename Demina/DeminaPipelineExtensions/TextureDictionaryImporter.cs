using System.Xml;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace DeminaPipelineExtensions
{
	[ContentImporter(".tdict", DisplayName = "Texture Dictionary Importer", DefaultProcessor = "TextureDictionaryProcessor")]
	public class TextureDictionaryImporter : ContentImporter<TextureDictionaryContent>
	{
		public override TextureDictionaryContent Import(string filename, ContentImporterContext context)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);

			TextureDictionaryContent content = new TextureDictionaryContent(xmlDocument, context);

			content.Filename = filename;
			content.Directory = filename.Remove(filename.LastIndexOf('\\'));

			return content;
		}
	}
}
