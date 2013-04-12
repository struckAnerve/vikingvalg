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

using Demina;
namespace Vikingvalg
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InGameManager : GameComponent
    {
        IManageSprites spriteService;
        IManageStates stateService;
        IManageCollision collisionService;
        IManageInput inputService;
        public IManageAudio audioService;

        private ChooseDirectionLevel _chooseDirectionlevel;
        private FightingLevel _fightingLevel;
        private MiningLevel _miningLevel;
        private TownLevel _townLevel;

        public Random rand;

        private enum _possibleInGameStates { ChooseDirectionLevel, FightingLevel, MiningLevel, TownLevel };
        public String InGameState { get; private set; }

        private ToggleButton musicToggle;
        private ToggleButton soundToggle;

        private Player _player1;

        public List<Sprite> ToDrawInGame { get; private set; }
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
            spriteService = (IManageSprites)Game.Services.GetService(typeof(IManageSprites));
            stateService = (IManageStates)Game.Services.GetService(typeof(IManageStates));
            collisionService = (IManageCollision)Game.Services.GetService(typeof(IManageCollision));
            inputService = (IManageInput)Game.Services.GetService(typeof(IManageInput));
            audioService = (IManageAudio)Game.Services.GetService(typeof(IManageAudio));

            spriteService.LoadDrawable(new StaticSprite("musicOptions"));
            spriteService.LoadDrawable(new StaticSprite("soundOptions"));
            musicToggle = new musicToggleButton("musicOptions", new Rectangle((int)spriteService.GameWindowSize.X - 40, 0, 40, 40), new Rectangle(0, 0, 40, 40), Game);
            soundToggle = new soundToggleButton("soundOptions", new Rectangle((int)spriteService.GameWindowSize.X - 80, 0, 40, 40), new Rectangle(0, 0, 40, 40), Game);
            _persistentInGameUI.Add(musicToggle);
            _persistentInGameUI.Add(soundToggle);

            rand = new Random();

            //Midlertidige plasseringer (?)
            _player1 = new Player(new Rectangle(100, 100, 150, 330), 0.5f, Game);
            spriteService.LoadDrawable(_player1);

            _chooseDirectionlevel = new ChooseDirectionLevel(_player1, Game);
            _fightingLevel = new FightingLevel(_player1, Game);
            _miningLevel = new MiningLevel(_player1, Game);
            _townLevel = new TownLevel(_player1, Game);

            ChangeInGameState("TownLevel", 100, 450);

            base.Initialize();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Midlertidig kode for å teste endring av State
            if (inputService.KeyWasPressedThisFrame(Keys.Tab))
            {
                stateService.ChangeState("PauseMenu");
            }

            if (InGameState == "ChooseDirectionLevel")
            {
                _chooseDirectionlevel.Update(inputService, gameTime);
            }
            else if (InGameState == "FightingLevel")
            {
                _fightingLevel.Update(inputService, gameTime);
            }
            else if (InGameState == "MiningLevel")
            {
                _miningLevel.Update(inputService, gameTime);
            }
            else if (InGameState == "TownLevel")
            {
                _townLevel.Update(inputService, gameTime);
            }

            base.Update(gameTime);
        }

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
