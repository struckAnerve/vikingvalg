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

using Demina;
namespace Vikingvalg
{
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent, IManageSprites
    {
        private SpriteBatch _spriteBatch;
        private List<Sprite> _toDrawInGame = new List<Sprite>();
        private Dictionary<String, Texture2D> _loadedStaticArt = new Dictionary<String,Texture2D>();
        Texture2D smallthing;
        IManageInput inputService;
        IManageCollision collisionService;

        public SpriteManager(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            base.LoadContent();
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            smallthing = Game.Content.Load<Texture2D>(@"redPixel");
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            collisionService = (IManageCollision)Game.Services.GetService(typeof(IManageCollision));
            inputService = (IManageInput)Game.Services.GetService(typeof(IManageInput));
            base.Initialize();
        }

        public void AddInGameDrawable(Sprite drawable)
        {
            if (drawable == null || _toDrawInGame.Contains(drawable))
            {
                Console.WriteLine("Unable to add drawable!");
                return;
            }

            _toDrawInGame.Add(drawable);
            if (drawable is StaticSprite)
            {
                StaticSprite staticElement = (StaticSprite)drawable;
                if (!(_loadedStaticArt.ContainsKey(staticElement.ArtName)))
                {
                    _loadedStaticArt.Add(staticElement.ArtName, Game.Content.Load<Texture2D>(staticElement.ArtName));
                }
            }
            else if (drawable is AnimatedSprite)
            {
                AnimatedSprite drawableAnimation = (AnimatedSprite) drawable;
                foreach (String animationName in drawableAnimation.animationList)
                {
                    drawableAnimation.animationPlayer.AddAnimation(animationName, Game.Content.Load<Animation>(@"Animations/"+drawableAnimation.AnimationDirectory+animationName));
                }
                
                drawableAnimation.animationPlayer.StartAnimation("idle"); 
            }

            if (drawable is ICanCollide)
            {
                ICanCollide canCollide = (ICanCollide)drawable;
                collisionService.AddCollidable(canCollide);
            }
        }

        public void RemoveInGameDrawable(Sprite toRemove)
        {
            _toDrawInGame.Remove(toRemove);

            if (toRemove is ICanCollide)
            {
                ICanCollide collideRemove = (ICanCollide) toRemove;
                collisionService.RemoveCollidable(collideRemove);
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (Sprite updatable in _toDrawInGame)
            {
                if (updatable is IUseInput)
                {
                    IUseInput needsInput = (IUseInput)updatable;
                    needsInput.Update(inputService);
                }
                else
                {
                    updatable.Update();
                }
                if(updatable is AnimatedSprite)
                {
                    AnimatedSprite updatableAnimation = (AnimatedSprite)updatable;
                    updatableAnimation.animationPlayer.Update(gameTime);
                }   
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            foreach (Sprite drawable in _toDrawInGame)
            {
                if (drawable is StaticSprite)
                {
                    StaticSprite staticDrawableSprite = (StaticSprite)drawable;
                    _spriteBatch.Draw(_loadedStaticArt[staticDrawableSprite.ArtName], staticDrawableSprite.DestinationRectangle, staticDrawableSprite.SourceRectangle, staticDrawableSprite.Color, staticDrawableSprite.Rotation,
                        staticDrawableSprite.Origin, staticDrawableSprite.Effects, staticDrawableSprite.LayerDepth);
                }
            }
            _spriteBatch.End();
            foreach (Sprite drawable in _toDrawInGame)
            {
                if (drawable is AnimatedSprite)
                {
                    AnimatedSprite drawableAnimation = (AnimatedSprite)drawable;
                    drawableAnimation.animationPlayer.Draw(_spriteBatch, drawableAnimation.DestinationRectangle, drawableAnimation.Flipped, drawableAnimation.Rotation, drawableAnimation.Scale);
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(smallthing, new Vector2(drawableAnimation.DestinationRectangle.X, drawableAnimation.DestinationRectangle.Y), Color.White);
                    Player p1 = (Player)drawableAnimation;
                    _spriteBatch.Draw(smallthing, new Vector2(p1.FootBox.X,p1.FootBox.Y), Color.White);
                    _spriteBatch.Draw(smallthing, new Vector2(p1.FootBox.X + p1.FootBox.Width, p1.FootBox.Y + p1.FootBox.Height), Color.White);
                    _spriteBatch.Draw(smallthing, new Vector2(p1.FootBox.X + p1.FootBox.Width/2, p1.FootBox.Y + p1.FootBox.Height), Color.White);
                    _spriteBatch.Draw(smallthing, new Vector2(p1.FootBox.X + p1.FootBox.Width, p1.FootBox.Y), Color.White);
                    _spriteBatch.Draw(smallthing, new Vector2(p1.FootBox.X + p1.FootBox.Width/2, p1.FootBox.Y), Color.White);
                    _spriteBatch.Draw(smallthing, new Vector2(p1.FootBox.X, p1.FootBox.Y + p1.FootBox.Height), Color.White);
                    _spriteBatch.End();
                }
                
            }

            base.Draw(gameTime);
        }
    }
}
