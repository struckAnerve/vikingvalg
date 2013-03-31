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
    public class InGameManager : Microsoft.Xna.Framework.GameComponent
    {
        IManageSprites spriteService;
        IManageStates stateService;
        IManageInput inputService;

        public InGameManager(Game game)
            : base(game)
        {
            GameComponent collisionManager = new CollisionManager(game);
            Game.Components.Add(collisionManager);
            Game.Services.AddService(typeof(IManageCollision), collisionManager);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            spriteService = (IManageSprites)Game.Services.GetService(typeof(IManageSprites));
            stateService = (IManageStates)Game.Services.GetService(typeof(IManageStates));
            inputService = (IManageInput)Game.Services.GetService(typeof(IManageInput));

            //Midlertidige plasseringer (?)
            float scale = 0.5f;
            Rectangle playerRectangle = new Rectangle(0, 0, 150, 330);
            Player p1 = new Player(new Rectangle(0, 0, (int)(playerRectangle.Width * scale), (int)(playerRectangle.Height * scale)));
            spriteService.AddInGameDrawable((Sprite)p1);

            Rectangle wolfRectangle = new Rectangle(0, 0, 450, 267);
            Wolf w1 = new Wolf(new Rectangle(500, 300, (int)(wolfRectangle.Width * scale), (int)(wolfRectangle.Height * scale)));
            spriteService.AddInGameDrawable((Sprite)w1);

            Enemy e1 = new Enemy(new Vector2(300, 200));
            spriteService.AddInGameDrawable((Sprite)e1);
            Enemy e2 = new Enemy(new Vector2(700, 300));
            spriteService.AddInGameDrawable((Sprite)e2);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Midlertidig kode for � teste endring av State
            if (inputService.KeyWasPressedThisFrame(Keys.Tab))
            {
                stateService.ChangeState("MainMenu");
            }

            base.Update(gameTime);
        }
    }
}
