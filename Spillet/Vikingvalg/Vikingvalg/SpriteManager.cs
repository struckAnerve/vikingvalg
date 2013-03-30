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
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent, IManageSprites
    {
        private SpriteBatch _spriteBatch;
        private List<Sprite> _toDraw = new List<Sprite>();
        private Dictionary<String, Texture2D> _loadedArt = new Dictionary<String,Texture2D>();

        IManageInput inputService;
        IManageCollision collisionService;

        public SpriteManager(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
            collisionService = (IManageCollision)Game.Services.GetService(typeof(IManageCollision));
            inputService = (IManageInput)Game.Services.GetService(typeof(IManageInput));
        }

        public void AddDrawable(Sprite drawable)
        {
            if (drawable == null || _toDraw.Contains(drawable))
            {
                Console.WriteLine("Unable to add drawable!");
                return;
            }

            if (drawable is StaticSprite)
            {
                StaticSprite staticElement = (StaticSprite)drawable;
                if (!(_loadedArt.ContainsKey(staticElement.ArtName)))
                {
                    _loadedArt.Add(staticElement.ArtName, Game.Content.Load<Texture2D>(staticElement.ArtName));
                }
            }
            _toDraw.Add(drawable);

            if (drawable is ICanCollide)
            {
                ICanCollide canCollide = (ICanCollide)drawable;
                collisionService.AddCollidable(canCollide);
            }
        }

        public void RemoveDrawable(Sprite toRemove)
        {
            _toDraw.Remove(toRemove);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (Sprite updatable in _toDraw)
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
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            foreach (StaticSprite drawable in _toDraw)
            {
                _spriteBatch.Draw(_loadedArt[drawable.ArtName], drawable.DestinationRectangle, drawable.SourceRectangle, drawable.Color, drawable.Rotation,
                    drawable.Origin, drawable.Effects, drawable.LayerDepth);
            }
            _spriteBatch.End();
        }
    }
}
