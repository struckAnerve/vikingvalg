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
            if (_player1.FootBox.Right >= spriteService.GameWindowSize.X && _player1.FootBox.Bottom < spriteService.GameWindowSize.Y / 2)
            {
                inGameService.ChangeInGameState("FightingLevel");
            }

            base.Update(inputService, gameTime);
        }
    }
}
