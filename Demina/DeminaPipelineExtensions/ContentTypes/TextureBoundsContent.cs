using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Globalization;

namespace DeminaPipelineExtensions
{
	[ContentSerializerRuntimeType("Demina.TextureBounds, DeminaRuntime")]
	public class TextureBoundsContent
	{
		public Rectangle Location { get; set; }
		public Vector2 Origin { get; set; }

		public string Name { get; protected set; }

		public TextureBoundsContent(XmlNode xmlNode, ContentImporterContext context)
		{
			Name = xmlNode.Attributes["name"].Value;
			Location = new Rectangle(int.Parse(xmlNode.SelectSingleNode("X").InnerText),
				int.Parse(xmlNode.SelectSingleNode("Y").InnerText),
				int.Parse(xmlNode.SelectSingleNode("Width").InnerText),
				int.Parse(xmlNode.SelectSingleNode("Height").InnerText));
			Origin = new Vector2(float.Parse(xmlNode.SelectSingleNode("OriginX").InnerText, CultureInfo.InvariantCulture),
				float.Parse(xmlNode.SelectSingleNode("OriginY").InnerText, CultureInfo.InvariantCulture));
		}
	}
}
