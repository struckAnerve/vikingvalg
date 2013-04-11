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
    class Stone : StaticSprite, ICanCollide, IPlaySound
    {
        public AnimatedStaticSprite stoneHitArt { get; set; }

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


        public IManageAudio _audioManager { get; set; }
        public String Directory { get; set; }

        public int endurance;

        private bool _hasGold;

        public Stone(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, bool hasGold, Game game)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        {
            stoneHitArt = new AnimatedStaticSprite("stonehit", new Rectangle(_destinationRectangle.X - 40, _destinationRectangle.Y - ((99-_destinationRectangle.Height)/2), 180, 99), Vector2.Zero, 4, 50, true);
            stoneHitArt.IsPlaying = false;
            endurance = 8;
            _footBox = new Rectangle(destinationRectangle.X, destinationRectangle.Bottom - 20, destinationRectangle.Width, 20);
            setLayerDepth(_footBox.Bottom);
            _hasGold = hasGold;
            _audioManager = (IManageAudio)game.Services.GetService(typeof(IManageAudio));
            Directory = "stone";
        }
        public Stone(Rectangle destinationRectangle, int sourceYPos, int color, bool hasGold, Game game)
            : this("stone", destinationRectangle, new Rectangle(0, sourceYPos, destinationRectangle.Width, destinationRectangle.Height),
                new Color(200, color + 30, color, 255), 0, Vector2.Zero, SpriteEffects.None, destinationRectangle.Bottom, hasGold, game)
        { }

        public void IsHit()
        {
            stoneHitArt.currentFrame = 0;
            endurance--;
            stoneHitArt.IsPlaying = true;
            if (endurance <= 0)
            {
                if (_hasGold)
                {
                    stoneHitArt.ChangeYPosition(198);
                    _sourceRectangle.X = 500;
                    _audioManager.AddSound(Directory + "/money");
                }
                else
                {
                    stoneHitArt.ChangeYPosition(99);
                    _sourceRectangle.X = 400;
                    _audioManager.AddSound(Directory+"/crumble");
                }
            }
            else if (endurance % 2 == 0)
            {
                _sourceRectangle.X += _sourceRectangle.Width;
            }
        }
    }
}
