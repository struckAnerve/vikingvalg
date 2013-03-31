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
        private Dictionary<String, Texture2D> _loadedStaticArt = new Dictionary<String,Texture2D>();

        //Midlertidig
        Texture2D smallthing;

        public List<List<Sprite>> ListsToDraw { get; set; }

        public SpriteManager(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            //midlertidig
            smallthing = Game.Content.Load<Texture2D>(@"redPixel");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            ListsToDraw = new List<List<Sprite>>();
            base.Initialize();
        }

        public void LoadDrawable(Sprite toLoad)
        {
            if (toLoad is StaticSprite)
            {
                StaticSprite staticElement = (StaticSprite)toLoad;
                if (!(_loadedStaticArt.ContainsKey(staticElement.ArtName)))
                {
                    _loadedStaticArt.Add(staticElement.ArtName, Game.Content.Load<Texture2D>(staticElement.ArtName));
                }
            }
            else if (toLoad is AnimatedSprite)
            {
                AnimatedSprite drawableAnimation = (AnimatedSprite)toLoad;
                foreach (String animationName in drawableAnimation.animationList)
                {
                    drawableAnimation.animationPlayer.AddAnimation(animationName, Game.Content.Load<Animation>(@"Animations/" + drawableAnimation.AnimationDirectory + animationName));
                }

                drawableAnimation.animationPlayer.StartAnimation("idle");
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (List<Sprite> listToDraw in ListsToDraw)
            {
                _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                foreach (Sprite drawable in listToDraw)
                {
                    if (drawable is StaticSprite)
                    {
                        StaticSprite staticDrawableSprite = (StaticSprite)drawable;
                        _spriteBatch.Draw(_loadedStaticArt[staticDrawableSprite.ArtName], staticDrawableSprite.DestinationRectangle, staticDrawableSprite.SourceRectangle, staticDrawableSprite.Color, staticDrawableSprite.Rotation,
                            staticDrawableSprite.Origin, staticDrawableSprite.Effects, staticDrawableSprite.LayerDepth);
                    }
                }
                _spriteBatch.End();
                foreach (Sprite drawable in listToDraw)
                {
                    if (drawable is AnimatedSprite)
                    {
                        AnimatedSprite drawableAnimation = (AnimatedSprite)drawable;
                        drawableAnimation.animationPlayer.Draw(_spriteBatch, drawableAnimation.DestinationRectangle, drawableAnimation.Flipped, drawableAnimation.Rotation, drawableAnimation.Scale);
                        _spriteBatch.Begin();
                        Player p1 = (Player)drawableAnimation;
                        drawBoxPerimeter(p1.FootBox);
                        _spriteBatch.End();
                    }
                }
            }

            base.Draw(gameTime);
        }

        private void drawBoxPerimeter(Rectangle box)
        {
            _spriteBatch.Draw(smallthing, new Vector2(box.X, box.Y), Color.White);
            _spriteBatch.Draw(smallthing, new Vector2(box.X + box.Width, box.Y + box.Height), Color.White);
            _spriteBatch.Draw(smallthing, new Vector2(box.X + box.Width / 2, box.Y + box.Height), Color.White);
            _spriteBatch.Draw(smallthing, new Vector2(box.X + box.Width, box.Y), Color.White);
            _spriteBatch.Draw(smallthing, new Vector2(box.X + box.Width / 2, box.Y), Color.White);
            _spriteBatch.Draw(smallthing, new Vector2(box.X, box.Y + box.Height), Color.White);
        }
    }
}
