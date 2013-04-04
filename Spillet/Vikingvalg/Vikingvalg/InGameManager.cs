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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InGameManager : Microsoft.Xna.Framework.GameComponent
    {
        IManageSprites spriteService;
        IManageStates stateService;
        IManageCollision collisionService;
        IManageInput inputService;

        private Player _p1;
        private List<Enemy> _enemyList = new List<Enemy>();

        public List<Sprite> ToDrawInGame { get; private set; }

        public InGameManager(Game game)
            : base(game)
        {
            GameComponent collisionManager = new CollisionManager(game);
            Game.Components.Add(collisionManager);
            Game.Services.AddService(typeof(IManageCollision), collisionManager);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            spriteService = (IManageSprites)Game.Services.GetService(typeof(IManageSprites));
            stateService = (IManageStates)Game.Services.GetService(typeof(IManageStates));
            collisionService = (IManageCollision)Game.Services.GetService(typeof(IManageCollision));
            inputService = (IManageInput)Game.Services.GetService(typeof(IManageInput));

            ToDrawInGame = new List<Sprite>();

            //Midlertidige plasseringer (?)
            float scale = 0.5f;
            Rectangle playerRectangle = new Rectangle(0, 0, 150, 330);

            _p1 = new Player(new Rectangle(0, 0, (int)(playerRectangle.Width * scale), (int)(playerRectangle.Height * scale)), scale);
            AddDrawable((Sprite)_p1);

            Rectangle wolfRectangle = new Rectangle(0, 0, 400, 267);
            scale = 0.3f;
            WolfEnemy wolf = new WolfEnemy(new Rectangle(300, 300, (int)(wolfRectangle.Width * scale), (int)(wolfRectangle.Height * scale)), scale);
            AddDrawable((Sprite)wolf);

            Rectangle blobRectangle = new Rectangle(0, 0, 400, 267);
            scale = 0.5f;
            BlobEnemy blob = new BlobEnemy(new Rectangle(100, 300, (int)(blobRectangle.Width * scale), (int)(blobRectangle.Height * scale)), scale);
            AddDrawable((Sprite)blob);

            base.Initialize();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Midlertidig kode for å teste endring av State
            if (inputService.KeyWasPressedThisFrame(Keys.Tab))
            {
                stateService.ChangeState("PauseMenu");
            }

            //Midlertidig for å teste å legge til enemy
            if(inputService.KeyWasPressedThisFrame(Keys.D0))
            {
                AddDrawable(new Enemy(Vector2.Zero));
            }

            foreach (Sprite toUpdate in ToDrawInGame)
            {
                if (toUpdate is IUseInput)
                {
                    IUseInput needsInput = (IUseInput)toUpdate;
                    needsInput.Update(inputService);
                }
                else
                {
                    toUpdate.Update();
                }
                if (toUpdate is AnimatedSprite)
                {
                    AnimatedSprite updatableAnimation = (AnimatedSprite)toUpdate;
                    updatableAnimation.animationPlayer.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        public void AddDrawable(Sprite toAdd)
        {
            if (toAdd == null || ToDrawInGame.Contains(toAdd))
            {
                Console.WriteLine("MenuManager: Unable to add drawable!");
                return;
            }

            spriteService.LoadDrawable(toAdd);
            ToDrawInGame.Add(toAdd);
            if (toAdd is Enemy)
            {
                Enemy addEnemy = (Enemy)toAdd;
                _enemyList.Add(addEnemy);
            }   

            if (toAdd is ICanCollide)
            {
                ICanCollide canCollide = (ICanCollide)toAdd;
                collisionService.AddCollidable(canCollide);
            }
        }

        public void RemoveDrawable(Sprite toRemove)
        {
            ToDrawInGame.Remove(toRemove);

            if (toRemove is Enemy)
            {
                Enemy removeEnemy = (Enemy)toRemove;
                _enemyList.Remove(removeEnemy);
            } 

            if (toRemove is ICanCollide)
            {
                ICanCollide collideRemove = (ICanCollide)toRemove;
                collisionService.RemoveCollidable(collideRemove);
            }
        }
    }
}
