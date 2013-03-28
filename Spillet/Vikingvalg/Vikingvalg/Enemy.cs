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
    class Enemy : StaticSprite
    {
        public Enemy(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        { }
        public Enemy(Rectangle destinationRectangle)
            : this("evil", destinationRectangle, new Rectangle(0, 0, 80, 78), new Color(255, 255, 255, 1f), 0, Vector2.Zero, SpriteEffects.None, 1)
        { }
        public Enemy(Vector2 destinationPosition)
            : this(new Rectangle((int)destinationPosition.X, (int)destinationPosition.Y, 80, 78))
        { }
        public Enemy()
            : this(Vector2.Zero)
        { }

        public override void Update()
        {
            if (_destinationRectangle.X < -10 || _destinationRectangle.X > 720)
            {
                _speed *= -1;
            }
            _destinationRectangle.X += _speed;
        }
    }
}