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
    /// <summary>
    /// Superklasse for alle spillbaner
    /// </summary>
    abstract class InGameLevel
    {
        //komponenter
        protected InGameManager _inGameService;
        public IManageSprites spriteService;
        protected IManageCollision _collisionService;

        //bakgrunnen
        protected StaticSprite _background;

        //spilleren
        protected Player _player1;

        //liste over det som skal legges i tegnelisten ved neste Update
        protected List<Sprite> _toDrawQueue = new List<Sprite>();
        //liste over det som skal tegnes
        protected List<Sprite> _toDrawInGameLevel = new List<Sprite>();
        //list over det som skal fjernes fra tengelisten ved neste Update
        protected List<Sprite> _toRemoveInGameLevel = new List<Sprite>();
        
        //der spilleren skal plasseres når han går til neste bane
        public int returnPositionY { get; set; }

        //property som returnerer listen over det som skal tegnes
        public List<Sprite> ToDrawInGameLevel 
        {
            get { return _toDrawInGameLevel; }
        }

        public InGameLevel(Player player1, Game game)
        {
            this._inGameService = (InGameManager)(game.Services.GetService(typeof(InGameManager)));
            this.spriteService = (IManageSprites)(game.Services.GetService(typeof(IManageSprites)));
            this._collisionService = (IManageCollision)(game.Services.GetService(typeof(IManageCollision)));
            
            _player1 = player1;
        }

        /// <summary>
        /// Kalles på når man endrer bane
        /// </summary>
        /// <param name="playerX">spillerens x-posisjon</param>
        /// <param name="playerY">spillerens y-posisjon</param>
        public virtual void InitializeLevel(int playerX, int playerY)
        {
            //HealthBar skal bare tegnes i fightinglevel
            spriteService.DrawHealthBar = false;
            if (_inGameService.InGameState == "FightingLevel")
            {
                spriteService.DrawHealthBar = true;
            }
            //kaller på spillerens reset-funksjon
            _player1.Reset(playerX, playerY);
            //legger til spilleren og bakgrunnen til tegning
            AddInGameLevelDrawable(_player1);
            AddInGameLevelDrawable(_background);
        }

        /// <summary>
        /// Må kalles når man bytter bane.
        /// </summary>
        public virtual void ClearLevel()
        {
            //tømmer lister som sier hva som skal tegnes
            _toDrawQueue.Clear();
            _toRemoveInGameLevel.Clear();
            _toDrawInGameLevel.Clear();
            _collisionService.ClearCollisionList();
        }

        public virtual void Update(IManageInput inputService, GameTime gameTime)
        {
            //Går du helt til venstre på skjermen (uten å være i ChooseDirectionLevel) blir du tatt til ChooseDirectionLevel
            if (_player1.FootBox.Left <= 0 && _inGameService.InGameState != "ChooseDirectionLevel")
            {
                ClearLevel();
                _inGameService.ChangeInGameState("ChooseDirectionLevel", (int)spriteService.GameWindowSize.X - _player1.FootBox.Width - 2, returnPositionY);
            }

            //fjern det som skal fjernes fra tegnelisten
            if (_toRemoveInGameLevel != null)
            {
                foreach (Sprite toRemove in _toRemoveInGameLevel)
                {
                    _toDrawInGameLevel.Remove(toRemove);
                }
                _toRemoveInGameLevel.Clear();
            }
            //legg til det som skal legges til i tegnelisten
            if (_toDrawQueue != null)
            {
                foreach (Sprite toDraw in _toDrawQueue)
                {
                    _toDrawInGameLevel.Add(toDraw);
                }
                _toDrawQueue.Clear();
            }
            //oppdater det som ligger i tegnelisten
            foreach (Sprite toUpdate in _toDrawInGameLevel.ToList())
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

        /// <summary>
        /// Legg til en Sprite som skal tegnes
        /// </summary>
        /// <param name="toAdd"></param>
        public virtual void AddInGameLevelDrawable(Sprite toAdd)
        {
            if (toAdd == null || _toDrawInGameLevel.Contains(toAdd))
            {
                Console.WriteLine("InGameManager: Unable to add drawable!");
                return;
            }

            _toDrawQueue.Add(toAdd);

            //last inn Texture2D
            if (toAdd is AnimatedEnemy || toAdd is Stone)
            {
                spriteService.LoadDrawable(toAdd);
            }

            //Legg til i kollisjonssystemet dersom Spriten skal kunne kollidere
            if (toAdd is ICanCollide)
            {
                ICanCollide canCollide = (ICanCollide)toAdd;
                _collisionService.AddCollidable(canCollide);
            }
        }

        //Kalles på når du skal fjerne en Sprite fra å tegnes
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
