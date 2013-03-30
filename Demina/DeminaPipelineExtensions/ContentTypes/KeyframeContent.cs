using System.Xml;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace DeminaPipelineExtensions
{
	[ContentSerializerRuntimeType("Demina.Keyframe, DeminaRuntime")]
	public class KeyframeContent
	{
		public int FrameNumber { get; set; }
		public List<BoneContent> Bones { get; set; }
		public string Trigger { get; set; }
		public bool FlipVertically { get; set; }
		public bool FlipHorizontally { get; set; }

		public KeyframeContent(XmlNode xmlNode, ContentImporterContext context)
		{
			Bones = new List<BoneContent>();

			FrameNumber = int.Parse(xmlNode.Attributes["frame"].Value);
			if (xmlNode.Attributes["trigger"] != null)
				Trigger = xmlNode.Attributes["trigger"].Value;
			else
				Trigger = "";

			if (xmlNode.Attributes["vflip"] != null)
				FlipVertically = bool.Parse(xmlNode.Attributes["vflip"].Value);
			else
				FlipVertically = false;

			if (xmlNode.Attributes["hflip"] != null)
				FlipHorizontally = bool.Parse(xmlNode.Attributes["hflip"].Value);
			else
				FlipHorizontally = false;

			XmlNodeList boneNodeList = xmlNode.SelectNodes("Bone");
			foreach (XmlNode boneNode in boneNodeList)
			{
				BoneContent boneContent = new BoneContent(boneNode, context);
				Bones.Add(boneContent);
			}
		}
	}
}
