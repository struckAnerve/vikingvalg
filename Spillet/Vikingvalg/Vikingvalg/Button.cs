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
    abstract class Button : StaticSprite, IUseInput
    {
        protected Rectangle _clickableBox; //Boks man kan klikke på
        protected bool hovered; //Om musen er over boksen eller ikke

        public Button(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        {
            _clickableBox = new Rectangle(destinationRectangle.X, destinationRectangle.Y, sourceRectangle.Width, sourceRectangle.Height);
        }
        /// <summary>
        /// Sjekker om musen er over boksen eller ikke
        /// </summary>
        public virtual void Update(IManageInput inputService)
        {
            if (hovered == false && _clickableBox.Contains(inputService.CurrMouse.X, inputService.CurrMouse.Y))
            {
                hovered = true;
                _sourceRectangle.X = _destinationRectangle.Width;
            }
            else if (hovered == true && !_clickableBox.Contains(inputService.CurrMouse.X, inputService.CurrMouse.Y))
            {
                hovered = false;
                _sourceRectangle.X = 0;
            }
        }
    }
}
