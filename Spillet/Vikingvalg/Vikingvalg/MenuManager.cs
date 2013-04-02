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
    public class MenuManager : Microsoft.Xna.Framework.GameComponent
    {
        IManageStates stateService;
        IManageInput inputService;

        Menu mainMenu;

        //endres
        public List<Sprite> ToDrawMainMenu { get; private set; }

        public MenuManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            stateService = (IManageStates)Game.Services.GetService(typeof(IManageStates));
            inputService = (IManageInput)Game.Services.GetService(typeof(IManageInput));

            mainMenu = new MainMenu((IManageSprites)Game.Services.GetService(typeof(IManageSprites)));
            mainMenu.MainState();
            ToDrawMainMenu = mainMenu.toDrawMenuClass;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (stateService.GameState == "MainMenu")
            {
                mainMenu.Update(inputService);

                //Midlertidig kode for å teste endring av State
                if (inputService.KeyWasPressedThisFrame(Keys.Tab))
                {
                    stateService.ChangeState("InGame");
                }
            }

            base.Update(gameTime);
        }
    }
}
