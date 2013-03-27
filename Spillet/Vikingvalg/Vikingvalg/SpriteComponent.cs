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
    public class SpriteComponent : Microsoft.Xna.Framework.DrawableGameComponent, IDrawSprites
    {

        private SpriteBatch _spriteBatch;
        private List<Sprite> _toDraw = new List<Sprite>();
        private Dictionary<String, Texture2D> _loadedArt = new Dictionary<String,Texture2D>();

        public SpriteComponent(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
        }

        public void AddDrawable(Sprite drawable)
        {
            if (drawable == null || _toDraw.Contains(drawable))
            {
                Console.WriteLine("Unable to add drawable!");
                return;
            }

            if(!(_loadedArt.ContainsKey(drawable.ArtName)))
            {
                _loadedArt.Add(drawable.ArtName, Game.Content.Load<Texture2D>(drawable.ArtName));
            }
            _toDraw.Add(drawable);
        }

        public void RemoveDrawable(Sprite toRemove)
        {
            _toDraw.Remove(toRemove);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();

            foreach (Sprite drawable in _toDraw)
            {
                _spriteBatch.Draw(_loadedArt[drawable.ArtName], drawable.DestinationRectangle, drawable.SourceRectangle, drawable.Color, drawable.Rotation,
                    drawable.Origin, drawable.Effects, drawable.LayerDepth);
            }
            _spriteBatch.End();
        }
    }
}
