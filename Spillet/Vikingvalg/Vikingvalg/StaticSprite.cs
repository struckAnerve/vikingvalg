using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Vikingvalg
{
    public class StaticSprite : Sprite
    {
        protected Rectangle _destinationRectangle;
        protected Rectangle _sourceRectangle;

        public String ArtName { get; protected set; }
        public Rectangle DestinationRectangle
        {
            get { return _destinationRectangle; }
            set { _destinationRectangle = value; }
        }
        public int DestinationWidth { get { return _destinationRectangle.Width; } set { _destinationRectangle.Width = value; } }
        public int DestinationX { get { return _destinationRectangle.X; } set { _destinationRectangle.X = value; } }
        public int DestinationY { get { return _destinationRectangle.Y; } set { _destinationRectangle.Y = value; } }
        public Rectangle SourceRectangle
        {
            get { return _sourceRectangle; }
            protected set { _sourceRectangle = value; }
        }
        public Vector2 Origin { get; protected set; }
        public SpriteEffects Effects { get; protected set; }

        public StaticSprite(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth) : base(color, rotation, layerDepth)
        {
            ArtName = artName;
            DestinationRectangle = destinationRectangle;
            SourceRectangle = sourceRectangle;
            Origin = origin;
            Effects = effects;
        }
        public StaticSprite(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, float layerDepth)
            : this(artName, destinationRectangle, sourceRectangle,
                new Color(255, 255, 255, 1f), 0, Vector2.Zero, SpriteEffects.None, layerDepth)
        { }
        
        public StaticSprite(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle)
            : this(artName, destinationRectangle, sourceRectangle, 0f)
        { }
        public StaticSprite(String artName, Rectangle destinationRectangle)
            : this(artName, destinationRectangle,new Rectangle(0, 0, destinationRectangle.Width, destinationRectangle.Height))
        { }
        public StaticSprite(String artName)
            : this(artName, new Rectangle(-10, -10, 10, 10))
        { }
    }
}
