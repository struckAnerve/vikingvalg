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
        public AnimatedEnemy activeEnemy { get; private set; }
        private List<AnimatedEnemy> levelCharacters = new List<AnimatedEnemy>();
        public FightingLevel(Player player1, IManageSprites spriteService, IManageCollision collisionService, InGameManager inGameService)
            : base(player1, spriteService, collisionService, inGameService)
        {
        }

        public override void InitializeLevel()
        {
            base.InitializeLevel();
            levelCharacters.Add(new BlobEnemy(new Rectangle(100, 100, 400, 267), 0.5f, _player1));
            levelCharacters.Add(new WolfEnemy(new Rectangle(300, 300, 400, 267), 0.3f, _player1));
            levelCharacters.Add(new BlobEnemy(new Rectangle(100, 100, 400, 267), 0.5f, _player1));
            levelCharacters.Add(new WolfEnemy(new Rectangle(300, 300, 400, 267), 0.3f, _player1));
            levelCharacters.Add(new WolfEnemy(new Rectangle(300, 300, 400, 267), 0.3f, _player1));
            levelCharacters.Add(new WolfEnemy(new Rectangle(300, 300, 400, 267), 0.3f, _player1));
            activeEnemy = levelCharacters[0];
            _player1.activeEnemy = activeEnemy;
            foreach (AnimatedEnemy enemy in levelCharacters)
            {
                AddInGameLevelDrawable(enemy);
                enemy.activeEnemy = activeEnemy;
                enemy.mobIndex = levelCharacters.IndexOf(enemy);
            }
            //AddInGameLevelDrawable(new BlobEnemy(new Rectangle(100, 100, 400, 267), 0.5f, _player1));
        }
        public override void Update(IManageInput inputService, GameTime gameTime)
        {
            base.Update(inputService, gameTime);
            if (activeEnemy.hp <= 0)
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
            }
        }
    }
}
