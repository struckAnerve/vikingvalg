using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Demina
{
	class Keyframe
	{
		public int FrameNumber { get; set; }
		public List<Bone> Bones { get; protected set; }
		public string Trigger { get; set; }
		public bool FlipVertically { get; set; }
		public bool FlipHorizontally { get; set; }

		public Keyframe(int frameNumber)
		{
			FrameNumber = frameNumber;
			Bones = new List<Bone>();
			updateOrderBones = new List<Bone>();
		}

		public Keyframe(Keyframe keyframe)
		{
			FrameNumber = keyframe.FrameNumber;
			Trigger = keyframe.Trigger;
			FlipVertically = keyframe.FlipVertically;
			FlipHorizontally = keyframe.FlipHorizontally;

			Bones = new List<Bone>();
			updateOrderBones = new List<Bone>();

			foreach (Bone b in keyframe.Bones)
				Bones.Add(new Bone(b));

			updateOrderBones.AddRange(Bones);
			SortBones();

			UpdateBones();
		}

		public static Keyframe Interpolate(int frameNumber, Keyframe key1, int key1FrameNumber, Keyframe key2, int key2FrameNumber)
		{
			Keyframe keyframe = new Keyframe(frameNumber);
			keyframe.FlipVertically = key1.FlipVertically;
			keyframe.FlipHorizontally = key1.FlipHorizontally;

			float t = (frameNumber - key1FrameNumber) / (float)(key2FrameNumber - key1FrameNumber);

			for (int boneIndex = 0; boneIndex < key1.Bones.Count; boneIndex++)
			{
				Bone bone = new Bone(key1.Bones[boneIndex].Name, key1.Bones[boneIndex].TextureIndex, key1.Bones[boneIndex].ParentIndex);
				bone.Position = Vector2.Lerp(key1.Bones[boneIndex].Position, key2.Bones[boneIndex].Position, t);
				bone.Scale = Vector2.Lerp(key1.Bones[boneIndex].Scale, key2.Bones[boneIndex].Scale, t);
				bone.Rotation = MathHelper.Lerp(key1.Bones[boneIndex].Rotation, key2.Bones[boneIndex].Rotation, t);
				bone.TextureFlipHorizontal = key1.Bones[boneIndex].TextureFlipHorizontal;
				bone.TextureFlipVertical = key1.Bones[boneIndex].TextureFlipVertical;
				bone.Hidden = key1.Bones[boneIndex].Hidden;

				keyframe.AddBone(bone);
			}

			keyframe.SortBones();
			keyframe.UpdateBones();
			return keyframe;
		}

		public static Keyframe Interpolate(int frameNumber, Keyframe key1, Keyframe key2)
		{
			return Interpolate(frameNumber, key1, key1.FrameNumber, key2, key2.FrameNumber);
		}

		public void AddBone(Bone bone)
		{
			Bones.Add(bone);
			updateOrderBones.Add(bone);
		}

		public void SortBones()
		{
			updateOrderBones.Clear();

			foreach (Bone b in Bones)
			{
				BoneSortAdd(b);
			}
		}

		protected void BoneSortAdd(Bone b)
		{
			if (updateOrderBones.Contains(b))
				return;

			if (b.ParentIndex != -1)
				BoneSortAdd(Bones[b.ParentIndex]);

			updateOrderBones.Add(b);
		}

		public void UpdateBones()
		{
			foreach (Bone bone in updateOrderBones)
			{
				if (bone.ParentIndex == -1)
					bone.UpdateTransform(Matrix.Identity);
				else
					bone.UpdateTransform(Bones[bone.ParentIndex].Transform);
			}
		}

		List<Bone> updateOrderBones;
	}
}
