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
    class WolfEnemy : AnimatedEnemy
    {

        public WolfEnemy(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, float scale, Player player1)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, scale, player1, 50)
        {
            AnimationDirectory = @"wolfAnimation/";
            setSpeed(5);
            //kan flyttes til base?
            destinationRectangle.Width = (int)(destinationRectangle.Width * scale);
            destinationRectangle.Height = (int)(destinationRectangle.Height * scale);
            footBoxXOffset = (int)(20 * scale);
            footBoxYOffset = 0;
            footBoxWidth = (int)(destinationRectangle.Width + (300 * scale));
            footBoxHeight = (int)(60 * scale);
            _footBox = new Rectangle(destinationRectangle.X - footBoxWidth / 2 + footBoxXOffset, destinationRectangle.Y + footBoxYOffset, footBoxWidth, footBoxHeight);
        }
        public WolfEnemy(Rectangle destinationRectangle, float scale, Player player1)
            : this("mm", destinationRectangle, new Rectangle(0, 0, 375, 485), new Color(255, 255, 255, 1f), 0, Vector2.Zero, SpriteEffects.None, 0.6f, scale, player1)
        { }  
    }
}
