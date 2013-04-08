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
        public ChooseDirectionLevel(Player player1, IManageSprites spriteService, IManageCollision collisionService, InGameManager inGameService)
            : base(player1, spriteService, collisionService, inGameService)
        {
            _background = new StaticSprite("ground", new Rectangle(0, 0, (int)spriteService.GameWindowSize.X, (int)spriteService.GameWindowSize.Y));
            spriteService.LoadDrawable(_background);
        }

        public override void InitializeLevel(int playerX, int playerY)
        {
            base.InitializeLevel(playerX, playerY);
        }

        public override void Update(IManageInput inputService, GameTime gameTime)
        {
            //Hvis spilleren er helt til høyre av skjermen
            if(_player1.FootBox.Right >= spriteService.GameWindowSize.X)
            {
                //Hvis spilleren er i øvre tredjedel av skjermen skal man endre InGameLevelState til "FightingLevel"
                if (_player1.FootBox.Bottom < ((spriteService.GameWindowSize.Y - spriteService.WalkBlockTop) / 3) + spriteService.WalkBlockTop)
                {
                    ClearLevel();
                    inGameService.ChangeInGameState("FightingLevel", 50, _player1.FootBox.Y);
                }
                else if (_player1.FootBox.Top > ((spriteService.GameWindowSize.Y - spriteService.WalkBlockTop) / 3)*2 + spriteService.WalkBlockTop)
                {
                    ClearLevel();
                    inGameService.ChangeInGameState("TownLevel", 50, _player1.FootBox.Y);
                }
                //Hvis spilleren er i midtre tredjedel av skjermen skal man endre InGameLevelState til "MiningLevel"
                else
                {
                    ClearLevel();
                    inGameService.ChangeInGameState("MiningLevel", 50, _player1.FootBox.Y);
                }
            }

            base.Update(inputService, gameTime);
        }
    }
}
