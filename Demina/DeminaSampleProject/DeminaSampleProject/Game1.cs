using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Demina;

namespace DeminaSampleProject
{
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		AnimationPlayer animationPlayer = new AnimationPlayer();
		AnimationPlayer boxPlayer = new AnimationPlayer();

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			animationPlayer.AddAnimation("walk", Content.Load<Animation>("Animations/guy_walk"));
			animationPlayer.StartAnimation("walk");

			boxPlayer.AddAnimation("box", Content.Load<Animation>("Animations/box"));
			boxPlayer.StartAnimation("box");
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Q))
                animationPlayer.PlaySpeedMultiplier = 2.0f;
            else if (ks.IsKeyDown(Keys.W))
                animationPlayer.PlaySpeedMultiplier = 0.5f;
            else
                animationPlayer.PlaySpeedMultiplier = 1.0f;

			animationPlayer.Update(gameTime);
			boxPlayer.Update(gameTime);
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			animationPlayer.Draw(spriteBatch, new Vector2(400, 200));
			boxPlayer.Draw(spriteBatch, new Vector2(100, 100));
			base.Draw(gameTime);
		}
	}
}
