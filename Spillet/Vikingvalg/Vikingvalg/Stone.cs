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
    class Stone : StaticSprite, ICanCollide
    {
        private MiningLevel _miningLevel;

        private Rectangle _footBox;
        public Rectangle FootBox
        {
            get { return _footBox; }
            private set { }
        }

        //Brukes ikke (men trengs for ICanCollide)
        public bool BlockedLeft { get; set; }
        public bool BlockedRight { get; set; }
        public bool BlockedTop { get; set; }
        public bool BlockedBottom { get; set; }

        private int _endurance;

        public Stone(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, MiningLevel miningLevel)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        {
            _miningLevel = miningLevel;
            _endurance = 8;
            _footBox = new Rectangle(destinationRectangle.X, destinationRectangle.Bottom - 20, destinationRectangle.Width, 20);
        }
        public Stone(String artName, Rectangle destinationRectangle, int color, MiningLevel miningLevel)
            : this(artName, destinationRectangle, new Rectangle(0, 0, destinationRectangle.Width, destinationRectangle.Height),
                new Color(200, color + 30, color, 255), 0, Vector2.Zero, SpriteEffects.None, 0.5f, miningLevel)
        { }

        public void IsHit()
        {
            _endurance--;
            if (_endurance <= 0)
            {
                _miningLevel.RemoveInGameLevelDrawable(this);
            }
            else if (_endurance % 2 == 0)
            {
                _sourceRectangle.X += _sourceRectangle.Width;
            }
        }
    }
}
