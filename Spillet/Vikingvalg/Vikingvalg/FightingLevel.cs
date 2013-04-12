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
        private int _returnPositionY = 245;
        public AnimatedEnemy activeEnemy { get; private set; }
        private List<AnimatedEnemy> levelCharacters = new List<AnimatedEnemy>();

        public FightingLevel(Player player1, Game game)
            : base(player1, game)
        {
            _background = new StaticSprite("ground", new Rectangle(0, 0, 1245, 700));
            spriteService.LoadDrawable(_background);
            spriteService.LoadDrawable(new StaticSprite("healthBar"));
            spriteService.LoadDrawable(new StaticSprite("healthContainer"));
        }

        public override void InitializeLevel(int playerX, int playerY)
        {
            base.InitializeLevel(playerX, playerY);
            returnPositionY = _returnPositionY;
            int playerRating = _player1.battleRating;
            int maxEnemies = 1;
            for (int i = 1; i < playerRating; i += 2) maxEnemies++;
            if (maxEnemies >= 5) maxEnemies = 5;
            int numEnemies = _inGameService.rand.Next(1, maxEnemies + 1);
            for (int i = 0; i <= numEnemies; i++)
            {
                if(_inGameService.rand.Next(0,2) == 0)
                    levelCharacters.Add(new BlobEnemy(new Rectangle(1245 + _inGameService.rand.Next(1, 6) * 50, _inGameService.rand.Next(2, 6) * 100, 400, 267), 0.5f, _player1, _inGameService.Game));
                else levelCharacters.Add(new WolfEnemy(new Rectangle(1245 + _inGameService.rand.Next(1, 6) * 50, _inGameService.rand.Next(2, 6) * 100, 400, 267), 0.3f, _player1, _inGameService.Game));
            }
            activeEnemy = levelCharacters[0];
            _player1.activeEnemy = activeEnemy;
            foreach (AnimatedEnemy enemy in levelCharacters)
            {
                AddInGameLevelDrawable(enemy);
                enemy.activeEnemy = activeEnemy;
                enemy.mobIndex = levelCharacters.IndexOf(enemy);
            }
        }

        public override void ClearLevel()
        {
            _player1.activeEnemy = null;
            levelCharacters.Clear();

            base.ClearLevel();
        }

        public override void Update(IManageInput inputService, GameTime gameTime)
        {
            if (activeEnemy.CurrHp <= 0)
            {
                levelCharacters.Remove(activeEnemy);
                RemoveInGameLevelDrawable(activeEnemy);
                if (levelCharacters.Count > 0)
                {
                    activeEnemy = levelCharacters[0];
                    foreach (AnimatedCharacter enemy in levelCharacters)
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
