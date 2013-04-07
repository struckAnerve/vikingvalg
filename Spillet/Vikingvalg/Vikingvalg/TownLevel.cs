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
    class TownLevel : InGameLevel
    {

        private NeutralNPC _shopkeeper;
        private NeutralNPC _oracle;

        public TownLevel(Player player1, IManageSprites spriteService, IManageCollision collisionService, InGameManager inGameService)
            : base(player1, spriteService, collisionService, inGameService)
        {
            _background = new StaticSprite("ground", new Rectangle(0, 0, (int)spriteService.GameWindowSize.X, (int)spriteService.GameWindowSize.Y));
            spriteService.LoadDrawable(_background);

            _shopkeeper = new NeutralNPC("shopkeeper", new Rectangle(400, 300, 70, 200));
            spriteService.LoadDrawable(_shopkeeper);
            AddInGameLevelDrawable(_shopkeeper);
            _oracle = new NeutralNPC("oracle", new Rectangle(900, 450, 130, 150));
            spriteService.LoadDrawable(_oracle);
            AddInGameLevelDrawable(_oracle);
        }

        public override void InitializeLevel()
        {
            base.InitializeLevel();

        }
    }
}

