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
    class NeutralNPC : StaticSprite
    {
        protected int _talkingRangeBoxOffset = 40;
        protected int _talkingRangeBoxHeight = 40;
        protected Rectangle _talkingRangeBox;
        public Rectangle TalkingRangeBox
        {
            get { return _talkingRangeBox; }
        }

        public NeutralNPC(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        {
            _talkingRangeBox = new Rectangle(destinationRectangle.Left - _talkingRangeBoxOffset, destinationRectangle.Bottom - _talkingRangeBoxHeight,
                destinationRectangle.Width + _talkingRangeBoxOffset * 2, _talkingRangeBoxHeight); 
        }
        public NeutralNPC(String artName, Rectangle destinationRectangle)
            : this(artName, destinationRectangle, new Rectangle(0, 0, destinationRectangle.Width, destinationRectangle.Height),
                new Color(255, 255, 255, 255), 0, Vector2.Zero, SpriteEffects.None, 0.5f)
        { }
    }
}
