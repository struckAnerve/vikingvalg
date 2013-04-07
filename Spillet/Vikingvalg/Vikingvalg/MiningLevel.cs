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

        private StaticSprite _background;
        private List<Stone> _stones = new List<Stone>();

        public int GoldStones { get; set; }

        public MiningLevel(Player player1, IManageSprites spriteService, IManageCollision collisionService, InGameManager inGameService)
            : base(player1, spriteService, collisionService, inGameService)
        {
            //Laster inn "stoneHit" som Texture2d
            spriteService.LoadDrawable(new AnimatedStaticSprite("stonehit", new Rectangle(0, 0, 180, 99), Vector2.Zero, 4, 1, false));
            GoldStones = 3;
        }

        public override void InitializeLevel()
        {
            base.InitializeLevel();

            //Lager en liste med tall som representerer hvilke steiner som skal ha gull
            if (GoldStones > 9)
            {
                Console.WriteLine("Feil: Flere steiner enn tilgjengelig(9) er satt til å ha gull i seg.");
            }
            List<int> stonesWithGold = new List<int>();
            int stoneWithGold = 0;
            while (stonesWithGold.Count() < GoldStones)
            {
                stoneWithGold = inGameService.rand.Next(1, 10);
                if (!stonesWithGold.Contains(stoneWithGold))
                {
                    stonesWithGold.Add(stoneWithGold);
                }
            }

            //to for-løkker (x- og y-retning) for å tegne steiner i en firkantformasjon
            int stoneRowMax = 3;
            int stoneColumnMax = 3;
            for (int stoneRow = 0; stoneRow < stoneRowMax; stoneRow++)
            {
                for (int stoneColumn = 0; stoneColumn < stoneColumnMax; stoneColumn++)
                {
                    //sjekker om neste stein som opprettes skal ha gull (bestemmes utenfor disse for-løkkene)
                    bool hasGold = false;
                    if (stonesWithGold.Contains((stoneRow * stoneColumnMax) + (stoneColumn + 1)))
                    {
                        hasGold = true;
                    }

                    //bestemer "offset" på hver stens utgangsposisjon
                    int stoneX = (270 * stoneColumn) + inGameService.rand.Next(51) + 150;
                    int stoneY = (170 * stoneRow) + inGameService.rand.Next(51) + 40;

                    //bestemmer "offset" på hver stens farge
                    int stoneColor = inGameService.rand.Next(140, 206);

                    //bestemmer hvilken stein som skal tegnes
                    int stone = inGameService.rand.Next(1, 4);

                    //lager rektangel ut ifra hvilken stein som skal tegnes
                    Rectangle stoneDestination = new Rectangle(stoneX, stoneY, 100, 0);
                    int sourceYPos = 0;
                    if (stone == 1)
                        stoneDestination.Height = 74;
                    else if (stone == 2)
                    {
                        stoneDestination.Height = 82;
                        sourceYPos = 74;
                    }
                    else
                    {
                        stoneDestination.Height = 71;
                        sourceYPos = 74 + 82;
                    }

                    //oppretter steinen
                    Stone toAdd = new Stone(stoneDestination, sourceYPos, stoneColor, hasGold);

                    //legger til steinen i privat (med bare steiner) og offentlig (med alt som skal tegnes/oppdateres i spillet) liste
                    _stones.Add(toAdd);
                    AddInGameLevelDrawable(toAdd);
                }
            }

            _player1.StonesToMine = _stones;
        }
    }
}
