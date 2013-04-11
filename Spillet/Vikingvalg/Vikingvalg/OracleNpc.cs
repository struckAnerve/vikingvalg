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
    class OracleNpc : NeutralNpc
    {
        public OracleNpc(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, Player player, InGameLevel inGameLevel)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, player, inGameLevel)
        {
            npcName = "Oracle";
            dialogController = new DialogControl((NeutralNpc)this);

            dialogController.ChangeNpcDialog("You smell");
            dialogController.AddPlayerAnswer("Go home, old lady");
        }
        public OracleNpc(String artName, Rectangle destinationRectangle, Player player, InGameLevel inGameLevel)
            : this(artName, destinationRectangle, new Rectangle(0, 0, destinationRectangle.Width, destinationRectangle.Height),
            new Color(255, 255, 255, 255), 0, Vector2.Zero, SpriteEffects.None, destinationRectangle.Bottom, player, inGameLevel)
        { }
    }
}
