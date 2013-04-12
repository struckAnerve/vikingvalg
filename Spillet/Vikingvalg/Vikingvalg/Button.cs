using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vikingvalg
{
    /// <summary>
    /// Knappeobjekt. Forventer at bildet som brukes har to tilstander (hovret og ikke-hovret) som ligger ved siden av hverandre på x-aksen
    /// </summary>
    abstract class Button : StaticSprite, IUseInput
    {
        //rektangelet man kan trykke på for å aktivere knappen
        protected Rectangle _clickableBox;
        //om musen er over knappen eller ikke
        protected bool hovered;

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
            //om knappen hovres eller ikke
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
