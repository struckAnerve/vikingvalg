using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace Vikingvalg
{
    /// <summary>
    /// Holder oversikt over menytilstander, hva som skal oppdateres og tegnes
    /// </summary>
    public class MenuManager : GameComponent
    {
        //komponenter
        private IManageStates _stateService;
        private IManageInput _inputService;

        //de forskjellige menyene
        private MainMenu _mainMenu;
        private PauseMenu _pauseMenu;

        //lister over hva som skal tegnes i de forskjellige menyene
        public List<Sprite> ToDrawMainMenu { get; private set; }
        public List<Sprite> ToDrawPauseMenu { get; private set; }

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
            //initialiserer komponenter
            _stateService = (IManageStates)Game.Services.GetService(typeof(IManageStates));
            _inputService = (IManageInput)Game.Services.GetService(typeof(IManageInput));

            //oppretter hovedmenyen
            _mainMenu = new MainMenu((IManageSprites)Game.Services.GetService(typeof(IManageSprites)), _stateService);
            _mainMenu.MainState();
            ToDrawMainMenu = _mainMenu.toDrawMenuClass;

            //oppretter pausemenyen
            _pauseMenu = new PauseMenu((IManageSprites)Game.Services.GetService(typeof(IManageSprites)), _stateService);
            _pauseMenu.MainState();
            ToDrawPauseMenu = _pauseMenu.toDrawMenuClass;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //oppdaterer hovedmanyen dersom du er der, eller pausemenyen dersom du er der
            if (_stateService.GameState == "MainMenu")
            {
                _mainMenu.Update(_inputService, gameTime);
            }
            else if (_stateService.GameState == "PauseMenu")
            {
                _pauseMenu.Update(_inputService, gameTime);
            }

            base.Update(gameTime);
        }
    }
}
