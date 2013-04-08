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
    abstract class InGameLevel
    {
        protected InGameManager inGameService;
        protected IManageSprites spriteService;
        protected IManageCollision collisionService;

        protected StaticSprite _background;

        protected Player _player1;

        protected List<Sprite> _toDrawQueue = new List<Sprite>();
        protected List<Sprite> _toDrawInGameLevel = new List<Sprite>();
        protected List<Sprite> _toRemoveInGameLevel = new List<Sprite>();
        public List<Sprite> ToDrawInGameLevel 
        {
            get { return _toDrawInGameLevel; }
        }

        public InGameLevel(Player player1, IManageSprites spriteService, IManageCollision collisionService, InGameManager inGameService)
        {
            this.inGameService = inGameService;
            this.spriteService = spriteService;
            this.collisionService = collisionService;
            _player1 = player1;
        }

        public virtual void InitializeLevel(int playerX, int playerY)
        {
            collisionService.ClearCollisionList();
            spriteService.DrawHealthBar = false;
            if (inGameService.InGameState == "FightingLevel")
            {
                spriteService.DrawHealthBar = true;
            }
            _toDrawInGameLevel.Clear();
            _player1.Reset(playerX, playerY);
            AddInGameLevelDrawable(_player1);
            AddInGameLevelDrawable(_background);
        }

        public virtual void Update(IManageInput inputService, GameTime gameTime)
        {
            if (_player1.FootBox.Left <= 0)
            {
                inGameService.ChangeInGameState("ChooseDirectionLevel", (int)spriteService.GameWindowSize.X - _player1.FootBox.Width, _player1.FootBox.Y);
            }
            if (_toRemoveInGameLevel != null)
            {
                foreach (Sprite toRemove in _toRemoveInGameLevel)
                {
                    _toDrawInGameLevel.Remove(toRemove);
                }
                _toRemoveInGameLevel.Clear();
            }
            if (_toDrawQueue != null)
            {
                foreach (Sprite toDraw in _toDrawQueue)
                {
                    _toDrawInGameLevel.Add(toDraw);
                }
                _toDrawQueue.Clear();
            }
            foreach (Sprite toUpdate in _toDrawInGameLevel)
            {
                if (toUpdate is IUseInput)
                {
                    IUseInput needsInput = (IUseInput)toUpdate;
                    needsInput.Update(inputService);
                }
                else if (toUpdate is AnimatedStaticSprite)
                {
                    AnimatedStaticSprite needsGameTime = (AnimatedStaticSprite)toUpdate;
                    if(needsGameTime.IsPlaying)
                        needsGameTime.Update(gameTime);
                }
                else
                {
                    toUpdate.Update();
                    if (toUpdate is Stone)
                    {
                        Stone stoneToUpdate = (Stone)toUpdate;
                        if (stoneToUpdate.stoneHitArt.IsPlaying)
                            stoneToUpdate.stoneHitArt.Update(gameTime);
                    }
                }
                if (toUpdate is AnimatedSprite)
                {
                    AnimatedSprite updatableAnimation = (AnimatedSprite)toUpdate;
                    if (updatableAnimation.animationPlayer.Update(gameTime) == true)
                    {
                        updatableAnimation.idle();
                    }
                }
            }
        }

        public virtual void AddInGameLevelDrawable(Sprite toAdd)
        {
            if (toAdd == null || _toDrawInGameLevel.Contains(toAdd))
            {
                Console.WriteLine("InGameManager: Unable to add drawable!");
                return;
            }

            _toDrawQueue.Add(toAdd);

            if (toAdd is AnimatedEnemy || toAdd is Stone)
            {
                spriteService.LoadDrawable(toAdd);
            }

            if (toAdd is ICanCollideBorder)
            {
                ICanCollideBorder canCollide = (ICanCollideBorder)toAdd;
                collisionService.AddCollidable(canCollide);
            }
        }

        public virtual void RemoveInGameLevelDrawable(Sprite toRemove)
        {
            _toRemoveInGameLevel.Add(toRemove);

            if (toRemove is ICanCollideBorder)
            {
                ICanCollideBorder collideRemove = (ICanCollideBorder)toRemove;
                collisionService.RemoveCollidable(collideRemove);
            }
        }
    }
}
