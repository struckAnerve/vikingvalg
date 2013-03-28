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
    public abstract class Sprite
    {
        protected Rectangle _destinationRectangle;

        public String ArtName { get; protected set; }
        public Rectangle DestinationRectangle {
            get { return _destinationRectangle; }
            protected set { _destinationRectangle = value; }
        }
        public Rectangle SourceRectangle { get; protected set; }
        public Color Color { get; protected set; }
        public float Rotation { get; protected set; }
        public Vector2 Origin { get; protected set; }
        public SpriteEffects Effects { get; protected set; }
        public float LayerDepth { get; protected set; }

        public Sprite(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            ArtName = artName;
            DestinationRectangle = destinationRectangle;
            SourceRectangle = sourceRectangle;
            Color = color;
            Rotation = rotation;
            Origin = origin;
            Effects = effects;
            LayerDepth = layerDepth;
        }

        public abstract void Update();
    }
}
