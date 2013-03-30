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

using Demina;

namespace Vikingvalg
{
    public abstract class AnimatedSprite : Sprite
    {
        protected Rectangle _destinationRectangle;

        public AnimationPlayer spriteAnimation { get; set; }
        protected String AnimationState;
        public String ArtName { get; protected set; }
        public float Scale { get; protected set; }
        public bool Flipped { get; set; }
        public Rectangle DestinationRectangle
        {
            get { return _destinationRectangle; }
            protected set { _destinationRectangle = value; }
        }
        public Rectangle SourceRectangle { get; protected set; }
        public Vector2 Origin { get; protected set; }
        public SpriteEffects Effects { get; protected set; }
        public float LayerDepth { get; protected set; }

        public String AnimationDirectory { get; protected set; }
        public List<String> animationList { get; protected set; }
        public AnimationPlayer animationPlayer { get; set; }

        public AnimatedSprite(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, String animationDirectory)
            : base(color, rotation)
        {
            AnimationDirectory = animationDirectory;
            spriteAnimation = new AnimationPlayer();
            Flipped = false;
            AnimationState = "idle";
            Scale = 0.5f;
            ArtName = artName;
            DestinationRectangle = destinationRectangle;
            SourceRectangle = sourceRectangle;
            Origin = origin;
            Effects = effects;
            LayerDepth = layerDepth;
        }
    }
}
