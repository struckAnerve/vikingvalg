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

using Demina;

namespace Vikingvalg
{
    class AnimatedCharacter : AnimatedSprite
    {
        public AnimatedSprite ColidingWith { get; set; }
        public int hp { get; set; }
        public bool BlockedLeft { get; set; }
        public bool BlockedRight { get; set; }
        public bool BlockedTop { get; set; }
        public bool BlockedBottom { get; set; }
        //Hitbox til spilleren
        protected Rectangle _footBox;

        public Rectangle FootBox
        {
            get { return _footBox; }
            set { }
        }

        public AnimatedCharacter(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, String animationDirectory, float scale)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, animationDirectory, scale)
        {
            //_footBox = new Rectangle(destinationRectangle.X - footBoxXOffset, destinationRectangle.Y + footBoxYOffset, footBoxWidth, footBoxHeight);
        }
    }
}
