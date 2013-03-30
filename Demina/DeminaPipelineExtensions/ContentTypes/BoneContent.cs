using System.Xml;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace DeminaPipelineExtensions
{
	[ContentSerializerRuntimeType("Demina.Bone, DeminaRuntime")]
	public class BoneContent
	{
		public string Name { get; set; }
		public bool Hidden { get; set; }
		public int ParentIndex { get; set; }
		public int TextureIndex { get; set; }
		public Vector2 Position { get; set; }
		public float Rotation { get; set; }
		public Vector2 Scale { get; set; }
		public bool TextureFlipHorizontal { get; set; }
		public bool TextureFlipVertical { get; set; }

		public BoneContent(XmlNode xmlNode, ContentImporterContext context)
		{
			Name = xmlNode.Attributes["name"].Value;
			Hidden = bool.Parse(xmlNode.SelectSingleNode("Hidden").InnerText);
			ParentIndex = int.Parse(xmlNode.SelectSingleNode("ParentIndex").InnerText);
			TextureIndex = int.Parse(xmlNode.SelectSingleNode("TextureIndex").InnerText);
			Position = new Vector2(float.Parse(xmlNode.SelectSingleNode("Position/X").InnerText, CultureInfo.InvariantCulture),
				float.Parse(xmlNode.SelectSingleNode("Position/Y").InnerText, CultureInfo.InvariantCulture));
			Rotation = float.Parse(xmlNode.SelectSingleNode("Rotation").InnerText, CultureInfo.InvariantCulture);
			Scale = new Vector2(float.Parse(xmlNode.SelectSingleNode("Scale/X").InnerText, CultureInfo.InvariantCulture),
				float.Parse(xmlNode.SelectSingleNode("Scale/Y").InnerText, CultureInfo.InvariantCulture));

			XmlNode tempNode = xmlNode.SelectSingleNode("TextureFlipHorizontal");
			if (tempNode != null)
				TextureFlipHorizontal = bool.Parse(tempNode.InnerText);
			else
				TextureFlipHorizontal = false;
			tempNode = xmlNode.SelectSingleNode("TextureFlipVertical");
			if (tempNode != null)
				TextureFlipVertical = bool.Parse(tempNode.InnerText);
			else
				TextureFlipVertical = false;
		}
	}
}
