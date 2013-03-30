using System.Xml;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace DeminaPipelineExtensions
{
	[ContentSerializerRuntimeType("Demina.Animation, DeminaRuntime")]
	public class AnimationContent
	{
		public string Version { get { return "1.0"; } }

		[ContentSerializerIgnore]
		public string Filename { get; set; }
		[ContentSerializerIgnore]
		public string Directory { get; set; }

		public int FrameRate { get; set; }
		public int LoopFrame { get; set; }

		public List<TextureEntryContent> Textures { get; set; }

		public List<KeyframeContent> Keyframes { get; set; }

		public AnimationContent(XmlDocument xmlDocument, ContentImporterContext context)
		{
			Textures = new List<TextureEntryContent>();
			Keyframes = new List<KeyframeContent>();

			FrameRate = int.Parse(xmlDocument.SelectSingleNode("/Animation/FrameRate").InnerText);
			LoopFrame = int.Parse(xmlDocument.SelectSingleNode("/Animation/LoopFrame").InnerText);

			XmlNodeList textureNodeList = xmlDocument.SelectNodes("/Animation/Texture");
			foreach (XmlNode textureNode in textureNodeList)
			{
				TextureEntryContent textureEntryContent = new TextureEntryContent(textureNode, context);
				Textures.Add(textureEntryContent);
			}

			XmlNodeList keyframeNodeList = xmlDocument.SelectNodes("/Animation/Keyframe");
			foreach (XmlNode keyframeNode in keyframeNodeList)
			{
				KeyframeContent keyframeContent = new KeyframeContent(keyframeNode, context);
				Keyframes.Add(keyframeContent);
			}
		}
	}
}
