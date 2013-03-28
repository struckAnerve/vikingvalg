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
    class Player : StaticSprite, IUseInput
    {
        public Player(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        { }
        public Player(Rectangle destinationRectangle)
            : this("mm", destinationRectangle, new Rectangle(0, 0, 375, 485), new Color(255, 255, 255, 1f), 0, Vector2.Zero, SpriteEffects.None, 1)
        { }
        public Player(Vector2 destinationPosition)
            : this("mm", new Rectangle((int)destinationPosition.X, (int)destinationPosition.Y, 375, 485), new Rectangle(0, 0, 375, 485), new Color(255, 255, 255, 1f), 0, Vector2.Zero, SpriteEffects.None, 1)
        { }
        public Player()
            : this(Vector2.Zero)
        { }

        public void Update(IHandleInput inputService)
        {
            if(inputService.KeyIsDown(Keys.Right))
            {
                _destinationRectangle.X += _speed;
            }
            if (inputService.KeyIsDown(Keys.Left))
            {
                _destinationRectangle.X -= _speed;
            }
            if (inputService.KeyIsDown(Keys.Up))
            {
                _destinationRectangle.Y -= _speed;
            } 
            if (inputService.KeyIsDown(Keys.Down))
            {
                _destinationRectangle.Y += _speed;
            }
        }
    }
}
