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

        public TownLevel(Player player1, Game game)
            : base(player1, game)
        {
            _background = new StaticSprite("ground", new Rectangle(0, 0, (int)spriteService.GameWindowSize.X, (int)spriteService.GameWindowSize.Y));
            spriteService.LoadDrawable(_background);

            _shopkeeper = new NeutralNpc("shopkeeper", new Rectangle(400, 300, 118, 230), "Shopkeeper", _player1, this);
            spriteService.LoadDrawable(_shopkeeper);
            _oracle = new NeutralNpc("oracle", new Rectangle(900, 450, 148, 174), "Oracle", _player1, this);
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

