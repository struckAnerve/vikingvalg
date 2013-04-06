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
    class AnimatedCharacter : AnimatedSprite
    {
        public AnimatedSprite ColidingWith { get; set; }
        public int hp { get; set; }
        public bool BlockedLeft { get; set; }
        public bool BlockedRight { get; set; }
        public bool BlockedTop { get; set; }
        public bool BlockedBottom { get; set; }

        protected int _speed { get; set; }
        protected int _xSpeed { get; set; }
        protected int _ySpeed { get; set; }

        public Healthbar healthbar {get; set;}
        public AnimatedEnemy activeEnemy;
        public AnimatedCharacter(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, float scale, int hitPoints)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, scale)
        {
            hp = hitPoints;
            healthbar = new Healthbar(hitPoints, destinationRectangle);
        }
        protected void setSpeed(int speed)
        {
            _speed = speed;
            _xSpeed = speed;
            _ySpeed = speed / 2;
        }
    }
}
