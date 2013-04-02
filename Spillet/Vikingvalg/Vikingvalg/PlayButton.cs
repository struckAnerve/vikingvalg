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
    class PlayButton : Button
    {
        public PlayButton(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, Menu menuController)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, menuController)
        {
        }
        public PlayButton(Vector2 position, Menu menuController)
            : this("playbutton", new Rectangle((int)position.X, (int)position.Y, 180, 75) , new Rectangle(0, 0, 180, 75), new Color(255, 255, 255, 1f), 0, Vector2.Zero, SpriteEffects.None, 0.5f, menuController)
        { }

        public override void Update(IManageInput inputService)
        {

            base.Update(inputService);
        }
    }
}
