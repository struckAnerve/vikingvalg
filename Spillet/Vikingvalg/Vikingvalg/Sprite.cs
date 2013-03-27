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
        public String ArtName { get; private set; }
        public Rectangle DestinationRectangle { get; private set; }
        public Rectangle SourceRectangle { get; private set; }
        public Color Color { get; private set; }
        public float Rotation { get; private set; }
        public Vector2 Origin { get; private set; }
        public SpriteEffects Effects { get; private set; }
        public float LayerDepth { get; private set; }

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
    }
}
