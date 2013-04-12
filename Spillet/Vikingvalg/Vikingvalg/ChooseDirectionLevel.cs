using Microsoft.Xna.Framework;

namespace Vikingvalg
{
    /// <summary>
    /// Spillbane. Her starter spilleren, og du kan gå til de andre banene herifra.
    /// </summary>
    class ChooseDirectionLevel : InGameLevel
    {
        //oppretter skiltet
        private StaticSprite _sign;

        public ChooseDirectionLevel(Player player1, Game game)
            : base(player1, game)
        {
            //bakgrunnsbildet
            _background = new StaticSprite("chooseLevelGround", new Rectangle(0, 0, (int)spriteService.GameWindowSize.X, (int)spriteService.GameWindowSize.Y));
            spriteService.LoadDrawable(_background);
            //skiltet
            _sign = new StaticSprite("sign", new Rectangle(650, 200, 206, 173), 200 + 140);
            spriteService.LoadDrawable(_sign);
        }

        /// <summary>
        /// Kalles på når spilleren først ankommer banen
        /// </summary>
        /// <param name="playerX">spillerens x-posisjon</param>
        /// <param name="playerY">spillerens y-posisjon</param>
        public override void InitializeLevel(int playerX, int playerY)
        {
            base.InitializeLevel(playerX, playerY);

            AddInGameLevelDrawable(_sign);
        }

        public override void Update(IManageInput inputService, GameTime gameTime)
        {
            //Hvis spilleren er helt til høyre av skjermen
            if(_player1.FootBox.Right >= spriteService.GameWindowSize.X)
            {
                //Hvis spilleren er i øvre tredjedel av skjermen skal man endre InGameLevelState til "FightingLevel"
                if (_player1.FootBox.Bottom < 315 && _player1.FootBox.Bottom > 190)
                {
                    ClearLevel();
                    _inGameService.ChangeInGameState("FightingLevel", _player1.FootBox.Width +2, _player1.FootBox.Y);
                }
                //Hvis spilleren er i midtre tredjedel av skjermen skal man endre InGameLevelState til "MiningLevel"
                else if (_player1.FootBox.Bottom < 490 && _player1.FootBox.Bottom > 380)
                {
                    ClearLevel();
                    _inGameService.ChangeInGameState("MiningLevel", _player1.FootBox.Width + 2, _player1.FootBox.Y);
                }
                //Hvis spilleren er i nedre tredjedel av skjermen skal man endre InGameLevelState til "TownLevel"
                else if (_player1.FootBox.Bottom < 690 && _player1.FootBox.Bottom > 570)
                {
                    ClearLevel();
                    _inGameService.ChangeInGameState("TownLevel", _player1.FootBox.Width + 2, _player1.FootBox.Y);
                }
            }

            base.Update(inputService, gameTime);
        }
    }
}
