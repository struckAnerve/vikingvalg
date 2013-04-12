using System;
using Microsoft.Xna.Framework;


namespace Vikingvalg
{
    /// <summary>
    /// Komponent som holder orden p� hvilken tilstand spillet er i (pauset, i spill, i meny osv.)
    /// </summary>
    public class GameStateManager : GameComponent, IManageStates
    {
        //komponenter som brukes
        private IManageSprites _spriteService;
        private IManageCollision _collisionService;
        private IManageAudio _audioService;
        private MenuManager _menuService;
        private InGameManager _inGameService;

        //liste over mulige spilltilstander
        private enum _possibleGameStates { MainMenu, InGame, PauseMenu };
        //n�v�rende spilltilstand
        public String GameState { get; private set; }

        public GameStateManager(Game game)
            : base(game)
        { }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //initialiserer komponenter
            _spriteService = (IManageSprites)Game.Services.GetService(typeof(IManageSprites));
            _collisionService = (IManageCollision)Game.Services.GetService(typeof(IManageCollision));
            _inGameService = (InGameManager)Game.Services.GetService(typeof(InGameManager));
            _menuService = (MenuManager)Game.Services.GetService(typeof(MenuManager));
            _audioService = (IManageAudio)Game.Services.GetService(typeof(AudioManager));

            //spillet vil starte i denne tilstanden
            ChangeState("MainMenu");

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //dersom spillvinduet mister fokus (og du ikke er i en meny allerede) vil spillet pauses
            if (!Game.IsActive && GameState == "InGame")
            {
                ChangeState("PauseMenu");
            }

            switch (GameState)
            {
                case "MainMenu":
                    _spriteService.ListsToDraw.Clear();
                    _spriteService.ListsToDraw.Add(_menuService.ToDrawMainMenu);
                    break;
                case "InGame":
                    _spriteService.ListsToDraw.Clear();
                    _spriteService.ListsToDraw.Add(_inGameService.ToDrawInGame);
                    break;
                case "PauseMenu":
                    _spriteService.ListsToDraw.Clear();
                    _spriteService.ListsToDraw.Add(_inGameService.ToDrawInGame);
                    _spriteService.ListsToDraw.Add(_menuService.ToDrawPauseMenu);
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

            switch (GameState)
            {
                case "MainMenu":
                    _menuService.Enabled = true;
                    _inGameService.Enabled = false;
                    _collisionService.Disable();
                    break;
                case "InGame":
                    _inGameService.Enabled = true;
                    _menuService.Enabled = false;
                    _collisionService.Enable();
                    break;
                case "PauseMenu":
                    _inGameService.Enabled = false;
                    _menuService.Enabled = true;
                    _collisionService.Disable();
                    break;
            }
        }
    }
}
