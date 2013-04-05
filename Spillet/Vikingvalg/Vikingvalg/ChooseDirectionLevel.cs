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
        { }

        public override void InitializeLevel()
        {
            base.InitializeLevel();
 
        }

        public override void Update(IManageInput inputService, GameTime gameTime)
        {
            //Hvis spilleren er helt til høyre av skjermen
            if(_player1.FootBox.Right >= spriteService.GameWindowSize.X)
            {
                //Hvis spilleren er i øvre tredjedel av skjermen skal man endre InGameLevelState til "FightingLevel"
                if (_player1.FootBox.Bottom < spriteService.GameWindowSize.Y / 3)
                {
                    inGameService.ChangeInGameState("FightingLevel");
                }
                //Hvis spilleren er i midtre tredjedel av skjermen skal man endre InGameLevelState til "MiningLevel"
                else if (_player1.FootBox.Top > spriteService.GameWindowSize.Y / 3 && _player1.FootBox.Bottom < spriteService.GameWindowSize.Y - (spriteService.GameWindowSize.Y / 3))
                {
                    inGameService.ChangeInGameState("MiningLevel");
                }
            }

            base.Update(inputService, gameTime);
        }
    }
}
