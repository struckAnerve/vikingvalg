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
    class Enemy : StaticSprite, ICanCollide
    {
        private Rectangle _footBox;

        public Rectangle FootBox
        {
            get { return _footBox; }
            set { }
        }

        public bool BlockedLeft { get; set; }
        public bool BlockedRight { get; set; }
        public bool BlockedTop { get; set; }
        public bool BlockedBottom { get; set; }

        public AnimatedSprite ColidingWith { get; set; }

        public Enemy(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        {
            _footBox = new Rectangle(destinationRectangle.X, (destinationRectangle.Y + destinationRectangle.Height - 40), destinationRectangle.Width, 40);
        }
        public Enemy(Rectangle destinationRectangle)
            : this("evil", destinationRectangle, new Rectangle(0, 0, 80, 78), new Color(255, 255, 255, 1f), 0, Vector2.Zero, SpriteEffects.None, 0.5f)
        { }
        public Enemy(Vector2 destinationPosition)
            : this(new Rectangle((int)destinationPosition.X, (int)destinationPosition.Y, 80, 78))
        { }
        public Enemy()
            : this(Vector2.Zero)
        { }

        public override void Update()
        {
            if ((BlockedLeft && _speed < 0) || (BlockedRight && _speed > 0))
            {
                _speed *= -1;
            }
                _destinationRectangle.X += _speed;
                _footBox.X = _destinationRectangle.X;
        }
    }
}