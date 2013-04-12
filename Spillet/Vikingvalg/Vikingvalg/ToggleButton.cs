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
    abstract class ToggleButton : Button
    {
        protected bool _toggled; //Hvorvidt knappen har blitt trykket på eller ikke
        public ToggleButton(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        {
            _toggled = false;
        }
        public override void Update(IManageInput inputService)
        {
            //Hvis spilleren trykket på venstreknappen denne framen
            if (hovered && inputService.MouseWasPressedThisFrame("left"))
            {
                buttonPressed();
            }
            base.Update(inputService);
        }
        public virtual void buttonPressed()
        {
            //Flytter source.Y til riktig sted
            if (!_toggled)
            {
                _sourceRectangle.Y = _sourceRectangle.Height;
                _toggled = true;
            }
            else if (_toggled)
            {
                _sourceRectangle.Y = 0;
                _toggled = false;
            }
        }
    }
}
