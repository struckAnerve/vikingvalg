using System;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeminaViewer
{
	public class Animation
	{
		public bool Loop;
		public float LoopTime;
		public List<Texture2D> Textures = new List<Texture2D>();
		public List<Keyframe> Keyframes = new List<Keyframe>();

		public void GetBoneTransformations(BoneTransformation[] transforms, int keyframeIndex, float time)
		{
			Keyframe currentKeyframe = Keyframes[keyframeIndex];
			Keyframe nextKeyframe;
			float t;

			if (keyframeIndex == Keyframes.Count - 1)
			{
				nextKeyframe = Keyframes[0];

				if (Loop)
					t = time / (LoopTime - currentKeyframe.FrameTime);
				else
					t = 0;
			}
			else
			{
				nextKeyframe = Keyframes[keyframeIndex + 1];
				t = time / (nextKeyframe.FrameTime - currentKeyframe.FrameTime);
			}

			for (int boneIndex = 0; boneIndex < Keyframes[0].UpdateBones.Count; boneIndex++)
			{
				Vector2 position = Vector2.Lerp(currentKeyframe.UpdateBones[boneIndex].Position, nextKeyframe.UpdateBones[boneIndex].Position, t);
				Vector2 scale = Vector2.Lerp(currentKeyframe.UpdateBones[boneIndex].Scale, nextKeyframe.UpdateBones[boneIndex].Scale, t);
				float rotation = MathHelper.Lerp(currentKeyframe.UpdateBones[boneIndex].Rotation, nextKeyframe.UpdateBones[boneIndex].Rotation, t);

				Matrix parentTransform = currentKeyframe.UpdateBones[boneIndex].ParentIndex == -1 ? Matrix.Identity : transforms[currentKeyframe.UpdateBones[boneIndex].ParentIndex].Transform;

				int drawIndex = currentKeyframe.UpdateBones[boneIndex].SelfIndex;

				transforms[drawIndex].Transform = Matrix.CreateScale(scale.X, scale.Y, 1) *
					Matrix.CreateRotationZ(rotation) *
					Matrix.CreateTranslation(position.X, position.Y, 0) *
					parentTransform;

				Vector3 position3, scale3;
				Vector2 direction;
				Quaternion rotationQ;

				transforms[drawIndex].Transform.Decompose(out scale3, out rotationQ, out position3);
				direction = Vector2.Transform(Vector2.UnitX, rotationQ);

				transforms[drawIndex].Position = new Vector2(position3.X, position3.Y);
				transforms[drawIndex].Scale = new Vector2(scale3.X, scale3.Y);
				transforms[drawIndex].Rotation = (float)Math.Atan2(direction.Y, direction.X);
			}
		}

		public static Animation LoadAnimation(GraphicsDevice graphicsDevice, string animationFile)
		{
			Animation animation = new Animation();

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(animationFile);

			int loopFrame = Int32.Parse(xmlDocument.SelectSingleNode("/Animation/LoopFrame").InnerText);
			float frameRate = 1.0f / Int32.Parse(xmlDocument.SelectSingleNode("/Animation/FrameRate").InnerText);

			animation.Loop = loopFrame != -1;
			if (animation.Loop)
				animation.LoopTime = frameRate * loopFrame;

			XmlNodeList nodeList = xmlDocument.SelectNodes("/Animation/Texture");
			foreach (XmlNode node in nodeList)
			{
				animation.Textures.Add(LoadTexture(graphicsDevice, node.InnerText, Path.GetFullPath(animationFile)));
			}

			nodeList = xmlDocument.SelectNodes("/Animation/Keyframe");
			foreach (XmlNode node in nodeList)
			{
				Keyframe keyframe = new Keyframe();

				keyframe.FrameTime = frameRate * Int32.Parse(node.Attributes["frame"].Value);

				if (node.Attributes["vflip"] != null)
					keyframe.FlipVertically = bool.Parse(node.Attributes["vflip"].Value);
				else
					keyframe.FlipVertically = false;

				if (node.Attributes["hflip"] != null)
					keyframe.FlipHorizontally = bool.Parse(node.Attributes["hflip"].Value);
				else
					keyframe.FlipHorizontally = false;


				XmlNodeList boneList = node.SelectNodes("Bone");
				foreach (XmlNode boneNode in boneList)
				{
					Bone bone = new Bone();

					bone.Name = boneNode.Attributes["name"].Value;
					bone.TextureIndex = Int32.Parse(boneNode.SelectSingleNode("TextureIndex").InnerText);
					bone.ParentIndex = Int32.Parse(boneNode.SelectSingleNode("ParentIndex").InnerText);
					bone.Hidden = bool.Parse(boneNode.SelectSingleNode("Hidden").InnerText);
					bone.Position = new Vector2(float.Parse(boneNode.SelectSingleNode("Position/X").InnerText, CultureInfo.InvariantCulture),
						float.Parse(boneNode.SelectSingleNode("Position/Y").InnerText, CultureInfo.InvariantCulture));
					bone.Rotation = float.Parse(boneNode.SelectSingleNode("Rotation").InnerText, CultureInfo.InvariantCulture);
					bone.Scale = new Vector2(float.Parse(boneNode.SelectSingleNode("Scale/X").InnerText, CultureInfo.InvariantCulture),
						float.Parse(boneNode.SelectSingleNode("Scale/Y").InnerText, CultureInfo.InvariantCulture));
					bone.SelfIndex = keyframe.DrawBones.Count;
					bone.FlipHorizontally = bool.Parse(boneNode.SelectSingleNode("TextureFlipHorizontal").InnerText);
					bone.FlipVertically = bool.Parse(boneNode.SelectSingleNode("TextureFlipVertical").InnerText);

					keyframe.DrawBones.Add(bone);
				}

				// keyframe.UpdateBones 
				foreach (Bone bone in keyframe.DrawBones)
				{
					BoneSortAdd(bone, keyframe.UpdateBones, keyframe.DrawBones);
				}

				animation.Keyframes.Add(keyframe);
			}

			return animation;
		}

		protected static void BoneSortAdd(Bone b, List<Bone> addList, List<Bone> sourceList)
		{
			if (addList.Contains(b))
				return;

			if (b.ParentIndex != -1)
				BoneSortAdd(sourceList[b.ParentIndex], addList, sourceList);

			addList.Add(b);
		}

		static Texture2D LoadTexture(GraphicsDevice graphicsDevice, string texturePath, string absolutePath)
		{
			if (!Path.IsPathRooted(texturePath))
			{
				texturePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(absolutePath), texturePath));
			}

			Texture2D texture;

			using (FileStream fileStream = File.Open(texturePath, FileMode.Open))
			{
				using (Texture2D originalTexture = Texture2D.FromStream(graphicsDevice, fileStream))
				{
					Color[] originalPixels = new Color[originalTexture.Width * originalTexture.Height];
					originalTexture.GetData<Color>(originalPixels);

					Color[] premultipliedPixels = new Color[originalTexture.Width * originalTexture.Height];

					for (int i = 0; i < originalPixels.Length; i++)
					{
						premultipliedPixels[i].R = (byte)(originalPixels[i].R * originalPixels[i].A / 255);
						premultipliedPixels[i].G = (byte)(originalPixels[i].G * originalPixels[i].A / 255);
						premultipliedPixels[i].B = (byte)(originalPixels[i].B * originalPixels[i].A / 255);
						premultipliedPixels[i].A = originalPixels[i].A;
					}

					texture = new Texture2D(graphicsDevice, originalTexture.Width, originalTexture.Height, false, SurfaceFormat.Color);
					texture.SetData<Color>(premultipliedPixels);
				}
			}

			return texture;
		}
	}

	public class Keyframe
	{
		public float FrameTime;
		public bool FlipVertically;
		public bool FlipHorizontally;
		public List<Bone> UpdateBones = new List<Bone>();
		public List<Bone> DrawBones = new List<Bone>();
	}

	public class Bone
	{
		public string Name;
		public bool Hidden;
		public Vector2 Position;
		public Vector2 Scale;
		public float Rotation;
		public int SelfIndex;
		public int TextureIndex;
		public int ParentIndex;
		public bool FlipVertically;
		public bool FlipHorizontally;
	}

	public struct BoneTransformation
	{
		public Matrix Transform;
		public Vector2 Position;
		public Vector2 Scale;
		public float Rotation;
	}
}
