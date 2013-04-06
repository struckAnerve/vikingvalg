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
        public String AnimationState{get; protected set;}
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

        //Hitbox til spilleren
        protected Rectangle _footBox;

        public Rectangle FootBox
        {
            get { return _footBox; }
            set { }
        }

        public AnimatedSprite(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, float scale)
            : base(color, rotation, layerDepth)
        {
            spriteAnimation = new AnimationPlayer();
            Flipped = false;
            AnimationState = "idle";
            animationList = new List<String>();
            animationList.Add("idle");
            animationPlayer = new AnimationPlayer();
            Scale = scale;
            ArtName = artName;
            DestinationRectangle = destinationRectangle;
            SourceRectangle = sourceRectangle;
            Origin = origin;
            Effects = effects;
            LayerDepth = layerDepth;
        }
        public void idle()
        {
            if (AnimationState != "idle")
            {
                animationPlayer.TransitionToAnimation("idle", 0.2f);
                AnimationState = "idle";
            }
        }
    }
}
