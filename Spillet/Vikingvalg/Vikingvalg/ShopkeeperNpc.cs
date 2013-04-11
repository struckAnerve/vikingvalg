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
    class ShopkeeperNpc : NeutralNpc
    {
        public ShopkeeperNpc(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, Player player, InGameLevel inGameLevel)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, player, inGameLevel)
        {
            npcName = "Shopkeeper";
            dialogController = new DialogControl((NeutralNpc)this);

            dialogController.ChangeNpcDialog("Hello, good sir! I have some fine wares for you to browse. If you want, I can sell you some raw " + 
                "materials, and you can craft equipment yourself. If not, bugger off, you are scaring the customers with that ugly face of yours. " +
                "But look at me, jabbering like an old fool. PLEASE SIR, I NEED MONEY!");
            dialogController.AddPlayerAnswer("Hello, shopkeeper");
            dialogController.AddPlayerAnswer("Could I get a discount from you, my friend?");
            dialogController.AddPlayerAnswer("Wakkala bing bang bo. Misala place to rest my friend. Never have I seen it before. Too long did not read");
        }
        public ShopkeeperNpc(String artName, Rectangle destinationRectangle, Player player, InGameLevel inGameLevel)
            : this(artName, destinationRectangle, new Rectangle(0, 0, destinationRectangle.Width, destinationRectangle.Height),
            new Color(255, 255, 255, 255), 0, Vector2.Zero, SpriteEffects.None, destinationRectangle.Bottom, player, inGameLevel)
        { }
    }
}
