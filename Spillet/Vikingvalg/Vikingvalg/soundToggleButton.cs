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
    class soundToggleButton : ToggleButton
    {
        private IManageAudio _audioManager;

        public soundToggleButton(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, Game game)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        {
            //Henter audiomanager fra game
            _audioManager = (IManageAudio)(game.Services.GetService(typeof(IManageAudio)));
        }
        public soundToggleButton(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Game game)
            : this(artName, destinationRectangle, sourceRectangle, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 1000, game)
        {

        }
        /// <summary>
        /// Pauser eller starter musikk
        /// </summary>
        public override void buttonPressed()
        {
            if (!_toggled)
                _audioManager.PauseSounds();
            else if (_toggled)
                _audioManager.ResumeSounds();
            base.buttonPressed();
        }
    }
}
