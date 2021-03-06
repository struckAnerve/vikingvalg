﻿using System;
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
    /// <summary>
    /// Superklasse for teksturer som ikke bruker Demina
    /// </summary>
    public class StaticSprite : Sprite
    {
        //Her følger variabler/properties om alt man trenger for å tegne en Sprite
        protected Rectangle _sourceRectangle;

        public String ArtName { get; protected set; }
        public Rectangle DestinationRectangle
        {
            get { return _destinationRectangle; }
            set { _destinationRectangle = value; }
        }
        public int DestinationWidth { get { return _destinationRectangle.Width; } set { _destinationRectangle.Width = value; } }
        public int DestinationHeight { get { return _destinationRectangle.Height; } set { _destinationRectangle.Height = value; } }
        public int DestinationX { get { return _destinationRectangle.X; } set { _destinationRectangle.X = value; } }
        public int DestinationY { get { return _destinationRectangle.Y; } set { _destinationRectangle.Y = value; } }
        public Rectangle SourceRectangle
        {
            get { return _sourceRectangle; }
            protected set { _sourceRectangle = value; }
        }

        public Color Color { get; protected set; }
        public float Rotation { get; protected set; }
        public Vector2 Origin { get; protected set; }
        public SpriteEffects Effects { get; protected set; }

        public StaticSprite(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth) : base(layerDepth)
        {
            ArtName = artName;
            DestinationRectangle = destinationRectangle;
            SourceRectangle = sourceRectangle;
            Origin = origin;
            Effects = effects;
            Color = color;
            Rotation = rotation;
        }
        public StaticSprite(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, float layerDepth, Color color)
            : this(artName, destinationRectangle, sourceRectangle, color, 0, Vector2.Zero, SpriteEffects.None, layerDepth)
        { }
        public StaticSprite(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, float layerDepth)
            : this(artName, destinationRectangle, sourceRectangle,
                new Color(255, 255, 255, 1f), 0, Vector2.Zero, SpriteEffects.None, layerDepth)
        { }
        public StaticSprite(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle)
            : this(artName, destinationRectangle, sourceRectangle, 0f)
        { }
        public StaticSprite(String artName, Rectangle destinationRectangle, float layerDepth)
            : this(artName, destinationRectangle, new Rectangle(0, 0, destinationRectangle.Width, destinationRectangle.Height), layerDepth)
        { }
        public StaticSprite(String artName, Rectangle destinationRectangle)
            : this(artName, destinationRectangle,new Rectangle(0, 0, destinationRectangle.Width, destinationRectangle.Height))
        { }
        public StaticSprite(String artName, float layerDepth)
            : this(artName, new Rectangle(-10, -10, 10, 10), layerDepth)
        { }
        public StaticSprite(String artName)
            : this(artName, new Rectangle(-10, -10, 10, 10))
        { }
    }
}
