using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Demina
{
	public class Bone
	{
		public string Name { get; set; }

		public int TextureIndex { get; set; }
		public int ParentIndex { get; set; }

		public Vector2 Position { get; set; }
		public Vector2 Scale { get; set; }
		public float Rotation { get; set; }

		public bool TextureFlipVertical { get; set; }
		public bool TextureFlipHorizontal { get; set; }

		public bool Hidden { get; set; }

		public Matrix Transform { get; protected set; }
		public Vector2 TransformedPosition { get; protected set; }
		public Vector2 TransformedScale { get; protected set; }
		public float TransformedRotation { get; protected set; }

		public void UpdateTransform(Matrix parentTransform)
		{
			Transform = Matrix.CreateScale(Scale.X, Scale.Y, 1) *
				Matrix.CreateRotationZ(Rotation) *
				Matrix.CreateTranslation(Position.X, Position.Y, 0) *
				parentTransform;

			Vector3 position3, scale3;
			Vector2 direction;
			Quaternion rotationQ;
			
			Transform.Decompose(out scale3, out rotationQ, out position3);
			direction = Vector2.Transform(Vector2.UnitX, rotationQ);

			TransformedPosition = new Vector2(position3.X, position3.Y);
			TransformedScale = new Vector2(scale3.X, scale3.Y);
			TransformedRotation = (float)Math.Atan2(direction.Y, direction.X);
		}

		public Bone(string name, int textureIndex, int parentIndex)
		{
			Name = name;

			TextureIndex = textureIndex;
			ParentIndex = parentIndex;

			Position = Vector2.Zero;
			Scale = Vector2.One;
			Rotation = 0;

			Hidden = false;

			TextureFlipHorizontal = false;
			TextureFlipVertical = false;

			Transform = Matrix.Identity;
		}

		public Bone(Bone bone)
		{
			Name = bone.Name;

			TextureIndex = bone.TextureIndex;
			ParentIndex = bone.ParentIndex;

			Position = bone.Position;
			Scale = bone.Scale;
			Rotation = bone.Rotation;

			Hidden = bone.Hidden;

			TextureFlipHorizontal = bone.TextureFlipHorizontal;
			TextureFlipVertical = bone.TextureFlipVertical;

			Transform = bone.Transform;
		}
	}
}
