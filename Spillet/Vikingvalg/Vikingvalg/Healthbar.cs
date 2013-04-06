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
    class Healthbar
    {
        public int MaxHP{ get; set; }
        public Rectangle PositionRectangle { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public StaticSprite healthBarSprite { get; private set; }
        public StaticSprite healthContainerSprite { get; private set; }
        private int _width = 177;
        private int _scaledWidth;
        private int _height = 29;
        private int _scaledHeight;
        private float _scale = 0.5f;
        public Healthbar(int maxHp, Rectangle positionRectangle)
        {
            _scaledWidth = (int)( _width * _scale);
            _scaledHeight = (int)(_height * _scale);
            MaxHP = maxHp;
            PositionRectangle = positionRectangle;
            healthBarSprite = new StaticSprite("healthBar", new Rectangle(0,0, _scaledWidth, _scaledHeight), new Rectangle(0,0, _width, _height));
            healthContainerSprite = new StaticSprite("healthContainer", new Rectangle(0, 0, _scaledWidth, _scaledHeight), new Rectangle(0, 0, _width, _height));
        }
        public void updateHealtBar(int currHP)
        {
            float newWidth = ((float)_scaledWidth / 100f) * (((float)currHP / (float)MaxHP) * 100f);
            healthBarSprite.DestinationWidth = (int)newWidth;
        }
        public void setPosition(Rectangle receivedPosition)
        {
            healthBarSprite.DestinationX = receivedPosition.Center.X;
            healthBarSprite.DestinationY = receivedPosition.Top;
            healthContainerSprite.DestinationX = receivedPosition.Center.X;
            healthContainerSprite.DestinationY = receivedPosition.Top;
        }
    }
}
