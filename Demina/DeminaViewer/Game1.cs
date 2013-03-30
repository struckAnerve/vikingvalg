using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace DeminaViewer
{
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		public Game1(string[] args)
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			if (args.Length != 2)
			{
				throw new Exception("This program requires the animation and zoom as command line arguments.");
			}

			animationFile = args[0];
            zoom = float.Parse(args[1]) / 100.0f;

			this.IsFixedTimeStep = false;

            Window.AllowUserResizing = true;
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			animation = Animation.LoadAnimation(GraphicsDevice, animationFile);
			transforms = new BoneTransformation[animation.Keyframes[0].DrawBones.Count];
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			KeyboardState newKeyboardState = Keyboard.GetState();

			if (newKeyboardState.IsKeyDown(Keys.Space) && keyboardState.IsKeyUp(Keys.Space))
			{
				currentTime = 0;
				currentKeyframeIndex = 0;
			}
			keyboardState = newKeyboardState;

			currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (currentKeyframeIndex == animation.Keyframes.Count - 1)
			{
				if (animation.Loop)
				{
					if (currentTime > animation.LoopTime)
					{
						currentTime -= animation.LoopTime;
						currentKeyframeIndex = 0;
					}
				}
				else
				{
					currentTime = animation.Keyframes[currentKeyframeIndex].FrameTime;
				}
			}
			else
			{
				if (currentTime > animation.Keyframes[currentKeyframeIndex + 1].FrameTime)
					currentKeyframeIndex++;
			}

			animation.GetBoneTransformations(transforms, currentKeyframeIndex, currentTime - animation.Keyframes[currentKeyframeIndex].FrameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(zoom * (animation.Keyframes[currentKeyframeIndex].FlipHorizontally ? -1 : 1), zoom * (animation.Keyframes[currentKeyframeIndex].FlipVertically ? -1 : 1), 1) * Matrix.CreateTranslation(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2, 0));

			for (int boneIndex = 0; boneIndex < animation.Keyframes[0].DrawBones.Count; boneIndex++)
			{
				Bone bone = animation.Keyframes[currentKeyframeIndex].DrawBones[boneIndex];
				if (bone.Hidden)
					continue;

				spriteBatch.Draw(animation.Textures[bone.TextureIndex], transforms[boneIndex].Position, null, Color.White, 
					transforms[boneIndex].Rotation, new Vector2(animation.Textures[bone.TextureIndex].Width / 2, animation.Textures[bone.TextureIndex].Height / 2),
					transforms[boneIndex].Scale, (bone.FlipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None) |
					(bone.FlipVertically ? SpriteEffects.FlipVertically : SpriteEffects.None), 0);
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}

		static void Main(string[] args)
		{
			using (Game1 game = new Game1(args))
			{
				game.Run();
			}
		}

		KeyboardState keyboardState;

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		int currentKeyframeIndex;
		float currentTime;

		string animationFile;
        float zoom;
		Animation animation;
		BoneTransformation[] transforms;
	}
}
