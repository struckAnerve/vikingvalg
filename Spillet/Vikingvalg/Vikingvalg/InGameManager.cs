using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Vikingvalg
{
    /// <summary>
    /// Komponent som styrer tilstandene i selve spillet, hva som skal tegnes og oppdateres
    /// </summary>
    public class InGameManager : GameComponent
    {
        //komponenter som trengs i denne klassen
        private IManageSprites _spriteService;
        private IManageStates _stateService;
        private IManageCollision _collisionService;
        private IManageInput _inputService;
        private IManageAudio _audioService;

        //de forskjellige spillbanene
        private ChooseDirectionLevel _chooseDirectionlevel;
        private FightingLevel _fightingLevel;
        private MiningLevel _miningLevel;
        private TownLevel _townLevel;

        //en Random. Fin å kunne hente ut i de forskjellige spillbanene og objektene som opprettes der
        public Random rand;

        //mulige spillbaner
        private enum _possibleInGameStates { ChooseDirectionLevel, FightingLevel, MiningLevel, TownLevel };
        //nåværende spillbane
        public String InGameState { get; private set; }

        //knappene for å styre lyden
        private ToggleButton musicToggle;
        private ToggleButton soundToggle;

        //spilleren
        private Player _player1;

        //Liste over hva som skal tegnes når man er i selve spillet
        public List<Sprite> ToDrawInGame { get; private set; }
        //Liste over GUI som skal tegnes i alle spillbrett
        private List<Sprite> _persistentInGameUI = new List<Sprite>();

        public InGameManager(Game game)
            : base(game)
        { }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //laster inn komponenter
            _spriteService = (IManageSprites)Game.Services.GetService(typeof(IManageSprites));
            _stateService = (IManageStates)Game.Services.GetService(typeof(IManageStates));
            _collisionService = (IManageCollision)Game.Services.GetService(typeof(IManageCollision));
            _inputService = (IManageInput)Game.Services.GetService(typeof(IManageInput));
            _audioService = (IManageAudio)Game.Services.GetService(typeof(IManageAudio));

            //oppretter musikkkontrollerne
            _spriteService.LoadDrawable(new StaticSprite("musicOptions"));
            _spriteService.LoadDrawable(new StaticSprite("soundOptions"));
            musicToggle = new musicToggleButton("musicOptions", new Rectangle((int)_spriteService.GameWindowSize.X - 40, 0, 40, 40), new Rectangle(0, 0, 40, 40), Game);
            soundToggle = new soundToggleButton("soundOptions", new Rectangle((int)_spriteService.GameWindowSize.X - 80, 0, 40, 40), new Rectangle(0, 0, 40, 40), Game);
            _persistentInGameUI.Add(musicToggle);
            _persistentInGameUI.Add(soundToggle);

            //oppretter Random
            rand = new Random();

            //oppretter spilleren
            _player1 = new Player(new Rectangle(100, 100, 150, 330), 0.5f, Game);
            _spriteService.LoadDrawable(_player1);

            //oppretter spillbanene
            _chooseDirectionlevel = new ChooseDirectionLevel(_player1, Game);
            _fightingLevel = new FightingLevel(_player1, Game);
            _miningLevel = new MiningLevel(_player1, Game);
            _townLevel = new TownLevel(_player1, Game);

            //Selve spillet starter på denne banen
            ChangeInGameState("ChooseDirectionLevel", 100, 450);

            base.Initialize();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Trykk escape for å pause
            if (_inputService.KeyWasPressedThisFrame(Keys.Escape))
            {
                _stateService.ChangeState("PauseMenu");
            }

            //oppdaterer riktig spillbane
            if (InGameState == "ChooseDirectionLevel")
            {
                _chooseDirectionlevel.Update(_inputService, gameTime);
            }
            else if (InGameState == "FightingLevel")
            {
                _fightingLevel.Update(_inputService, gameTime);
            }
            else if (InGameState == "MiningLevel")
            {
                _miningLevel.Update(_inputService, gameTime);
            }
            else if (InGameState == "TownLevel")
            {
                _townLevel.Update(_inputService, gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Endre spillbane
        /// </summary>
        /// <param name="changeTo">navnet på banen du ønsker å endre til</param>
        /// <param name="playerX">spillerens x posisjon på den nye banen</param>
        /// <param name="playerY">spillerens y posisjon på den nye banen</param>
        public void ChangeInGameState(String changeTo, int playerX, int playerY)
        {
            ClearLevels(changeTo);
            if (!Enum.IsDefined(typeof(_possibleInGameStates), changeTo))
            {
                Console.WriteLine("Unable to change in-game state (you are trying to change to an unkown state: '" + changeTo + "')");
                return;
            }

            InGameState = changeTo;

            switch (InGameState)
            {
                case "ChooseDirectionLevel":
                    _chooseDirectionlevel.InitializeLevel(playerX, playerY);
                    ToDrawInGame = _chooseDirectionlevel.ToDrawInGameLevel;
                    break;
                case "FightingLevel":
                    _fightingLevel.InitializeLevel(playerX, playerY);
                    ToDrawInGame = _fightingLevel.ToDrawInGameLevel;
                    break;
                case "MiningLevel":
                    _miningLevel.InitializeLevel(playerX, playerY);
                    ToDrawInGame = _miningLevel.ToDrawInGameLevel;
                    break;
                case "TownLevel":
                    _townLevel.InitializeLevel(playerX, playerY);
                    ToDrawInGame = _townLevel.ToDrawInGameLevel;
                    break;
            }
            foreach (Sprite spriteToDraw in _persistentInGameUI) ToDrawInGame.Add(spriteToDraw);
        }
        private void ClearLevels(String nextLevel)
        {
            if (nextLevel != "ChooseDirectionLevel") _chooseDirectionlevel.ClearLevel();
            if (nextLevel != "FightingLevel") _fightingLevel.ClearLevel();
            if (nextLevel != "MiningLevel") _miningLevel.ClearLevel();
            if (nextLevel != "TownLevel") _townLevel.ClearLevel();
        }
    }
}
