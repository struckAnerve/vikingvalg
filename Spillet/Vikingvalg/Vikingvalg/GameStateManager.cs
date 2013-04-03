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

            GameComponent inputManager = new InputManager(game);
            Game.Components.Add(inputManager);
            Game.Services.AddService(typeof(IManageInput), inputManager);

            GameComponent menuManager = new MenuManager(game);
            Game.Components.Add(menuManager);
            Game.Services.AddService(typeof(MenuManager), menuManager);

            GameComponent inGameManager = new InGameManager(game);
            Game.Components.Add(inGameManager);
            Game.Services.AddService(typeof(InGameManager), inGameManager);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            spriteService = (IManageSprites)Game.Services.GetService(typeof(IManageSprites));
            inGameService = (InGameManager)Game.Services.GetService(typeof(InGameManager));
            menuService = (MenuManager)Game.Services.GetService(typeof(MenuManager));

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
                ChangeState("MainMenu");
            }

            switch (GameState)
            {
                case "MainMenu":
                    menuService.Enabled = true;
                    inGameService.Enabled = false;

                    spriteService.ListsToDraw.Clear();
                    spriteService.ListsToDraw.Add(menuService.ToDrawMainMenu);
                    break;
                case "InGame":
                    inGameService.Enabled = true;
                    menuService.Enabled = false;

                    spriteService.ListsToDraw.Clear();
                    spriteService.ListsToDraw.Add(inGameService.ToDrawInGame);
                    break;
                case "PauseMenu":
                    inGameService.Enabled = false;
                    menuService.Enabled = true;

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
