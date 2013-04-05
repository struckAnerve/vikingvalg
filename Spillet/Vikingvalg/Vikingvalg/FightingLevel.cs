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
    class FightingLevel : InGameLevel
    {
        public FightingLevel(Player player1, IManageSprites spriteService, IManageCollision collisionService, InGameManager inGameService)
            : base(player1, spriteService, collisionService, inGameService)
        { }

        public override void InitializeLevel()
        {
            base.InitializeLevel();

            AddInGameLevelDrawable(new WolfEnemy(new Rectangle(300, 300, 400, 267), 0.3f, _player1));
            //AddInGameLevelDrawable(new BlobEnemy(new Rectangle(100, 100, 400, 267), 0.5f, _player1));
        }
    }
}
