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
    class ChooseDirectionLevel : InGameLevel
    {
        private StaticSprite _sign;
        public ChooseDirectionLevel(Player player1, IManageSprites spriteService, IManageCollision collisionService, InGameManager inGameService)
            : base(player1, spriteService, collisionService, inGameService)
        {
            _background = new StaticSprite("chooseLevelGround", new Rectangle(0, 0, (int)spriteService.GameWindowSize.X, (int)spriteService.GameWindowSize.Y));
            spriteService.LoadDrawable(_background);
            _sign = new StaticSprite("sign", new Rectangle(650, 200, 206, 173), 200 + 206);
            spriteService.LoadDrawable(_sign);
        }

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
