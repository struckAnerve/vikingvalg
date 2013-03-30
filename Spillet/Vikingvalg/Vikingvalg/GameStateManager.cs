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


namespace Vikingvalg
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameStateManager : Microsoft.Xna.Framework.DrawableGameComponent, IManageStates
    {

        IManageSprites renderService;

        public GameStateManager(Game game)
            : base(game)
        {
            DrawableGameComponent renderer = new SpriteManager(game);
            Game.Components.Add(renderer);
            Game.Services.AddService(typeof(IManageSprites), renderer);

            GameComponent input = new InputManager(game);
            Game.Components.Add(input);
            Game.Services.AddService(typeof(IManageInput), input);

            GameComponent collisionDetector = new CollisionManager(game);
            Game.Components.Add(collisionDetector);
            Game.Services.AddService(typeof(IManageCollision), collisionDetector);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            renderService = (IManageSprites)Game.Services.GetService(typeof(IManageSprites));

            Player p1 = new Player(new Rectangle(200, 200, 150, 192));
            renderService.AddDrawable((Sprite)p1);

            Enemy e1 = new Enemy(new Vector2(300, 200));
            renderService.AddDrawable((Sprite)e1);
            Enemy e2 = new Enemy(new Vector2(700, 300));
            renderService.AddDrawable((Sprite)e2);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void ChangeState()
        {
        }
    }
}
