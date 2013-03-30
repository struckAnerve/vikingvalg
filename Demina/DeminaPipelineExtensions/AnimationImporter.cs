using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace DeminaPipelineExtensions
{
	[ContentImporter(".anim", DisplayName = "Demina Animation Importer", DefaultProcessor = "AnimationProcessor")]
	public class AnimationImporter : ContentImporter<AnimationContent>
	{
		public override AnimationContent Import(string filename, ContentImporterContext context)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);

			AnimationContent content = new AnimationContent(xmlDocument, context);

			content.Filename = filename;
			content.Directory = filename.Remove(filename.LastIndexOf('\\'));

			return content;
		}
	}
}
