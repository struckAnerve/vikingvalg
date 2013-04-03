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
    class BlobEnemy : AnimatedEnemy
    {

        public BlobEnemy(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, float scale)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, @"blobAnimation/", scale)
        {
            //kan flyttes til base?
            destinationRectangle.Width = (int)(destinationRectangle.Width * scale);
            destinationRectangle.Height = (int)(destinationRectangle.Height * scale);

            footBoxXOffset = (int)(20 * scale);
            footBoxYOffset = (int)(30 * scale);
            footBoxWidth = (int)(destinationRectangle.Width + (40 * scale));
            footBoxHeight = (int)(footBoxHeight * scale + 10);
            setFootBox(new Rectangle(destinationRectangle.X - footBoxWidth / 2 + footBoxXOffset, destinationRectangle.Y + footBoxYOffset, footBoxWidth, footBoxHeight));

            Flipped = false;
        }
        public BlobEnemy(Rectangle destinationRectangle, float scale)
            : this("mm", destinationRectangle, new Rectangle(0, 0, 375, 485), new Color(255, 255, 255, 1f), 0, Vector2.Zero, SpriteEffects.None, 0.6f, scale)
        { }
        public BlobEnemy(Rectangle destinationRectangle)
            : this(destinationRectangle, 1f)
        { }
        public BlobEnemy(Vector2 destinationPosition)
            : this(new Rectangle((int)destinationPosition.X, (int)destinationPosition.Y, 375, 485))
        { }
        public BlobEnemy()
            : this(Vector2.Zero)
        { }
    }
}
