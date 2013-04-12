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
        public Rectangle PositionRectangle { get; set; } //Hvor healthbaren skal plasseres
        public StaticSprite healthBarSprite { get; private set; } //Spriten til den røde delen av healhtbaren
        public StaticSprite healthContainerSprite { get; private set; } //Spriten til rammen rund healthbaren
        //Bredde og høyde, før og etter de har blitt skalert
        private int _width = 177; 
        private int _scaledWidth; 
        private int _height = 29; 
        private int _scaledHeight;
        private float _scale = 0.5f; //Hvor mye de skal skaleres med (Burde ha blitt sendt inn av karakteren
        private int _characterHeight; //Høyden til karakteren (for å posisjonere)
        public Healthbar(int maxHp, int characterHeight)
        {
            _scaledWidth = (int)( _width * _scale);
            _scaledHeight = (int)(_height * _scale);
            _characterHeight = (int)(characterHeight * _scale);
            healthBarSprite = new StaticSprite("healthBar", new Rectangle(0,0, _scaledWidth, _scaledHeight), new Rectangle(0,0, _width, _height));
            healthContainerSprite = new StaticSprite("healthContainer", new Rectangle(0, 0, _scaledWidth, _scaledHeight), new Rectangle(0, 0, _width, _height));
        }
        /// <summary>
        /// Oppdaterer bredden på den røde delen ut ifra hvor mye prosent av maxHP man har
        /// </summary>
        /// <param name="currHP">Nåværende HP</param>
        /// <param name="maxHP">Maks HP</param>
        public void updateHealtBar(int currHP, int maxHP)
        {
            float newWidth = ((float)_scaledWidth / 100f) * (((float)currHP / (float)maxHP) * 100f);
            healthBarSprite.DestinationWidth = (int)newWidth;
        }
        /// <summary>
        /// Setter posisjonen til healthbar
        /// </summary>
        /// <param name="receivedPosition">posisjon til karakteren som sender </param>
        public void setPosition(Rectangle receivedPosition)
        {
            healthBarSprite.DestinationX = receivedPosition.Center.X - _scaledWidth / 2;
            healthBarSprite.DestinationY = receivedPosition.Bottom - _characterHeight - 20;
            healthContainerSprite.DestinationX = receivedPosition.Center.X - _scaledWidth / 2;
            healthContainerSprite.DestinationY = receivedPosition.Bottom - _characterHeight - 20;
        }
        /// <summary>
        /// Setter bredden på den røde delen tilbake til original breddde
        /// </summary>
        public void reset()
        {
            healthBarSprite.DestinationWidth = _scaledWidth;
        }
    }
}
