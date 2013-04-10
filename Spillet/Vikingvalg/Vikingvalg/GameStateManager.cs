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
    public class GameStateManager : Microsoft.Xna.Framework.GameComponent, IManageStates
    {
        IManageSprites spriteService;
        IManageCollision collisionService;
        IManageAudio audioService;
        MenuManager menuService;
        InGameManager inGameService;

        private enum _possibleGameStates { MainMenu, InGame, PauseMenu };
        public String GameState { get; private set; }

        public GameStateManager(Game game)
            : base(game)
        {
            DrawableGameComponent spriteManager = new SpriteManager(game);
            Game.Components.Add(spriteManager);
            Game.Services.AddService(typeof(IManageSprites), spriteManager);

            GameComponent collisionManager = new CollisionManager(game);
            Game.Components.Add(collisionManager);
            Game.Services.AddService(typeof(IManageCollision), collisionManager);

            GameComponent inputManager = new InputManager(game);
            Game.Components.Add(inputManager);
            Game.Services.AddService(typeof(IManageInput), inputManager);

            GameComponent menuManager = new MenuManager(game);
            Game.Components.Add(menuManager);
            Game.Services.AddService(typeof(MenuManager), menuManager);

            GameComponent inGameManager = new InGameManager(game);
            Game.Components.Add(inGameManager);
            Game.Services.AddService(typeof(InGameManager), inGameManager);

            DrawableGameComponent audioManager = new AudioManager(game);
            Game.Components.Add(audioManager);
            Game.Services.AddService(typeof(IManageAudio), audioManager);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            spriteService = (IManageSprites)Game.Services.GetService(typeof(IManageSprites));
            collisionService = (IManageCollision)Game.Services.GetService(typeof(IManageCollision));
            inGameService = (InGameManager)Game.Services.GetService(typeof(InGameManager));
            menuService = (MenuManager)Game.Services.GetService(typeof(MenuManager));
            audioService = (IManageAudio)Game.Services.GetService(typeof(AudioManager));

            ChangeState("MainMenu");

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Game.IsActive && GameState == "InGame")
            {
                ChangeState("PauseMenu");
            }

            switch (GameState)
            {
                case "MainMenu":
                    menuService.Enabled = true;
                    inGameService.Enabled = false;
                    collisionService.Disable();

                    spriteService.ListsToDraw.Clear();
                    spriteService.ListsToDraw.Add(menuService.ToDrawMainMenu);
                    break;
                case "InGame":
                    inGameService.Enabled = true;
                    menuService.Enabled = false;
                    collisionService.Enable();

                    spriteService.ListsToDraw.Clear();
                    spriteService.ListsToDraw.Add(inGameService.ToDrawInGame);
                    break;
                case "PauseMenu":
                    inGameService.Enabled = false;
                    menuService.Enabled = true;
                    collisionService.Disable();

                    spriteService.ListsToDraw.Clear();
                    spriteService.ListsToDraw.Add(inGameService.ToDrawInGame);
                    spriteService.ListsToDraw.Add(menuService.ToDrawPauseMenu);
                    break;
            }

            base.Update(gameTime);
        }

        public void ChangeState(String changeTo)
        {
            if (!Enum.IsDefined(typeof(_possibleGameStates), changeTo))
            {
                Console.WriteLine("Unable to change state (you are trying to change to an unkown state: '" + changeTo + "')");
                return;
            }

            GameState = changeTo;
        }
    }
}
