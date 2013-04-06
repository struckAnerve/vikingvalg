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
    class MiningLevel : InGameLevel
    {
        private List<Stone> _stones = new List<Stone>();

        public MiningLevel(Player player1, IManageSprites spriteService, IManageCollision collisionService, InGameManager inGameService)
            : base(player1, spriteService, collisionService, inGameService)
        { }

        public override void InitializeLevel()
        {
            base.InitializeLevel();

            for (int stoneRow = 0; stoneRow < 4; stoneRow++)
            {
                for (int stoneColumn = 0; stoneColumn < 4; stoneColumn++)
                {
                    int stoneX = (270 * stoneColumn) + inGameService.rand.Next(51) + 150;
                    int stoneY = (170 * stoneRow) + inGameService.rand.Next(51) + 40;

                    int stoneColor = inGameService.rand.Next(140, 206);

                    int stone = inGameService.rand.Next(1, 4);
                    String stoneName = "Stone" + stone;

                    Rectangle stoneDestination = new Rectangle(stoneX, stoneY, 100, 0);
                    if (stone == 1)
                        stoneDestination.Height = 74;
                    else if (stone == 2)
                        stoneDestination.Height = 82;
                    else
                        stoneDestination.Height = 71;

                    Stone toAdd = new Stone(stoneName, stoneDestination, stoneColor, this);

                    _stones.Add(toAdd);
                    AddInGameLevelDrawable(toAdd);
                }
            }

            _player1.StonesToMine = _stones;
        }
    }
}
