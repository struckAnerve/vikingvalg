using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Vikingvalg
{
    class FightingLevel : InGameLevel
    {
        //Y-posisjonen som spilleren skal settes på når han blir sendt tilbake til chooseDirectionLevel
        private int _returnPositionY = 245; 
        public AnimatedEnemy activeEnemy { get; private set; } //Fienden som angriper spilleren
        private List<AnimatedEnemy> levelEnemies = new List<AnimatedEnemy>(); //Liste over fiender på banen

        public FightingLevel(Player player1, Game game)
            : base(player1, game)
        {
            //Laster inn bakgrunnen og og healthbars
            _background = new StaticSprite("ground", new Rectangle(0, 0, 1245, 700));
            spriteService.LoadDrawable(_background);
            spriteService.LoadDrawable(new StaticSprite("healthBar"));
            spriteService.LoadDrawable(new StaticSprite("healthContainer"));
        }
        /// <summary>
        /// Initialiserer banen, tar imot posisjonen til spilleren
        /// </summary>
        /// <param name="playerX">x-posisjonen til spilleren</param>
        /// <param name="playerY">y-posisjonen til spilleren</param>
        public override void InitializeLevel(int playerX, int playerY)
        {
            base.InitializeLevel(playerX, playerY);
            returnPositionY = _returnPositionY;
            //Legger inn et tilfeldig antall fiender, det er minimum 1 fiende, og maksimum 5. Det er aldri fler fiender enn levelet til spilleren
            int maxEnemies = 1;
            for (int i = 1; i < _player1.combatLevel; i += 2) maxEnemies++;
            if (maxEnemies >= 5) maxEnemies = 5;
            int numEnemies = _inGameService.rand.Next(1, maxEnemies + 1);
            for (int i = 0; i <= numEnemies; i++)
            {
                if(_inGameService.rand.Next(0,2) == 0)
                    levelEnemies.Add(new BlobEnemy(new Rectangle(1245 + _inGameService.rand.Next(1, 6) * 50, _inGameService.rand.Next(2, 6) * 100, 400, 267), 0.5f, _player1, _inGameService.Game));
                else levelEnemies.Add(new WolfEnemy(new Rectangle(1245 + _inGameService.rand.Next(1, 6) * 50, _inGameService.rand.Next(2, 6) * 100, 400, 267), 0.3f, _player1, _inGameService.Game));
            }
            //setter den første fienden til å være den fienden som angriper
            activeEnemy = levelEnemies[0];
            _player1.activeEnemy = activeEnemy;
            /* Legger alle fiendene inn til å tegnes
             * Sette activ enemy i alle fiendene til den fienden 
             * som er satt som active enemy 
             * Setter mobIndex i hver fiende til
             * tilsvarende index i levelEnemies */
            foreach (AnimatedEnemy enemy in levelEnemies)
            {
                AddInGameLevelDrawable(enemy);
                enemy.activeEnemy = activeEnemy;
                enemy.mobIndex = levelEnemies.IndexOf(enemy);
            }
        }
        //Fjerner aktiv fiende hos spillere, tømmer listen over fiender
        public override void ClearLevel()
        {
            _player1.activeEnemy = null;
            levelEnemies.Clear();

            base.ClearLevel();
        }
        public override void Update(IManageInput inputService, GameTime gameTime)
        {
            //Sjekker om den aktive fienden er død, hvis den er det, fjern den, bytt til ny active enemy
            if (activeEnemy.CurrHp <= 0)
            {
                levelEnemies.Remove(activeEnemy);
                RemoveInGameLevelDrawable(activeEnemy);
                if (levelEnemies.Count > 0)
                {
                    activeEnemy = levelEnemies[0];
                    foreach (AnimatedCharacter enemy in levelEnemies)
                    {
                        enemy.activeEnemy = activeEnemy;
                    }
                    _player1.activeEnemy = activeEnemy;
                }
                else
                {
                    _player1.activeEnemy = null;
                }
            }

            base.Update(inputService, gameTime);
        }
    }
}
