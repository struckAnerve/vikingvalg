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
        protected InGameManager _inGameService;
        public IManageSprites spriteService;
        protected IManageCollision _collisionService;

        protected StaticSprite _background;

        protected Player _player1;

        protected List<Sprite> _toDrawQueue = new List<Sprite>();
        protected List<Sprite> _toDrawInGameLevel = new List<Sprite>();
        protected List<Sprite> _toRemoveInGameLevel = new List<Sprite>();
        public int returnPositionY { get; set; }
        //TODO fikse knapper
        private ToggleButton musicToggle;
        private ToggleButton soundToggle;
        public List<Sprite> ToDrawInGameLevel 
        {
            get { return _toDrawInGameLevel; }
        }

        public InGameLevel(Player player1, IManageSprites spriteService, IManageCollision collisionService, InGameManager inGameService)
        {
            this._inGameService = inGameService;
            this.spriteService = spriteService;
            this._collisionService = collisionService;
            //TODO fikse knapper
            spriteService.LoadDrawable(new StaticSprite("musicOptions"));
            spriteService.LoadDrawable(new StaticSprite("soundOptions"));
            musicToggle = new ToggleButton("musicOptions", new Rectangle(100, 0, 50, 48), new Rectangle(0, 0, 50, 48), Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 1000f);
            soundToggle = new ToggleButton("soundOptions", new Rectangle(200, 0, 50, 48), new Rectangle(0, 0, 50, 48), Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 1000f);
            
            _player1 = player1;
        }

        public virtual void InitializeLevel(int playerX, int playerY)
        {
            spriteService.DrawHealthBar = false;
            if (_inGameService.InGameState == "FightingLevel")
            {
                spriteService.DrawHealthBar = true;
            }
            _player1.Reset(playerX, playerY);
            AddInGameLevelDrawable(_player1);
            AddInGameLevelDrawable(_background);
            //TODO fikse knapper
            AddInGameLevelDrawable(musicToggle);
            AddInGameLevelDrawable(soundToggle);
        }

        public virtual void ClearLevel()
        {
            _toDrawQueue.Clear();
            _toRemoveInGameLevel.Clear();
            _toDrawInGameLevel.Clear();
            _collisionService.ClearCollisionList();
        }

        public virtual void Update(IManageInput inputService, GameTime gameTime)
        {
            if (_player1.FootBox.Left <= 0 && _inGameService.InGameState != "ChooseDirectionLevel")
            {
                ClearLevel();
                _inGameService.ChangeInGameState("ChooseDirectionLevel", (int)spriteService.GameWindowSize.X - _player1.FootBox.Width - 2, returnPositionY);
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
            if (toAdd is IPlaySound)
            {
                IPlaySound iPlaySoundObject = (IPlaySound)toAdd;
                _inGameService.audioService.addSoundPlayingObject(iPlaySoundObject);
            } 
            if (toAdd is AnimatedEnemy || toAdd is Stone)
            {
                spriteService.LoadDrawable(toAdd);
            }

            if (toAdd is ICanCollide)
            {
                ICanCollide canCollide = (ICanCollide)toAdd;
                _collisionService.AddCollidable(canCollide);
            }
        }

        public virtual void RemoveInGameLevelDrawable(Sprite toRemove)
        {
            _toRemoveInGameLevel.Add(toRemove);

            if (toRemove is ICanCollide)
            {
                ICanCollide collideRemove = (ICanCollide)toRemove;
                _collisionService.RemoveCollidable(collideRemove);
            }
        }
    }
}
