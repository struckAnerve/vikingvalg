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
    public class InGameManager : Microsoft.Xna.Framework.GameComponent
    {
        IManageSprites spriteService;
        IManageStates stateService;
        IManageCollision collisionService;
        IManageInput inputService;

        private ChooseDirectionLevel _chooseDirectionlevel;
        private FightingLevel _fightingLevel;
        private MiningLevel _miningLevel;
        private TownLevel _townLevel;

        public Random rand;

        private enum _possibleInGameStates { ChooseDirectionLevel, FightingLevel, MiningLevel, TownLevel };
        public String InGameState { get; private set; }

        private Player _player1;

        public List<Sprite> ToDrawInGame { get; private set; }

        public int level;

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

            rand = new Random();

            //Midlertidige plasseringer (?)
            _player1 = new Player(new Rectangle(100, 100, 150, 330), 0.5f);
            spriteService.LoadDrawable(_player1);

            level = 1;

            _chooseDirectionlevel = new ChooseDirectionLevel(_player1, spriteService, collisionService, this);
            _fightingLevel = new FightingLevel(_player1, spriteService, collisionService, this);
            _miningLevel = new MiningLevel(_player1, spriteService, collisionService, this);
            _townLevel = new TownLevel(_player1, spriteService, collisionService, this);

            ChangeInGameState("ChooseDirectionLevel",_player1.FootBox.X, _player1.FootBox.Y);

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
        }
    }
}
