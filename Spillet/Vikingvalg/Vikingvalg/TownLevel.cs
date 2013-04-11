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
        private int _returnPositionY = 620;
        private NeutralNpc _shopkeeper;
        private NeutralNpc _oracle;

        public TownLevel(Player player1, IManageSprites spriteService, IManageCollision collisionService, InGameManager inGameService)
            : base(player1, spriteService, collisionService, inGameService)
        {
            _background = new StaticSprite("ground", new Rectangle(0, 0, (int)spriteService.GameWindowSize.X, (int)spriteService.GameWindowSize.Y));
            spriteService.LoadDrawable(_background);

            _shopkeeper = new ShopkeeperNpc("shopkeeper", new Rectangle(400, 300, 70, 200), _player1, this);
            spriteService.LoadDrawable(_shopkeeper);
            _oracle = new OracleNpc("oracle", new Rectangle(900, 450, 130, 150), _player1, this);
            spriteService.LoadDrawable(_oracle);
        }

        public override void InitializeLevel(int playerX, int playerY)
        {
            base.InitializeLevel(playerX, playerY);
            returnPositionY = _returnPositionY;
            AddInGameLevelDrawable(_shopkeeper);
            AddInGameLevelDrawable(_oracle);
        }

        public override void Update(IManageInput inputService, GameTime gameTime)
        {

            base.Update(inputService, gameTime);
        }
    }
}

