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

using Demina;
namespace Vikingvalg
{
    class WolfEnemy : AnimatedEnemy
    {

        public WolfEnemy(Rectangle destinationRectangle, float layerDepth, float scale, Player player1, Game game)
            : base(destinationRectangle, layerDepth, scale, player1, game)
        {
            //Setter diverse variabler som må settes fra denne klassen
            Directory = @"wolf";
            setSpeed(5);

            destinationRectangle.Width = (int)(destinationRectangle.Width * scale);
            destinationRectangle.Height = (int)(destinationRectangle.Height* scale);

            footBoxXOffset = (int)(20 * scale);
            footBoxYOffset = 0;
            footBoxWidth = (int)(destinationRectangle.Width + (300 * scale));
            footBoxHeight = (int)(60 * scale);
            //Regner ut og setter footbox/hitboxen til ulven
            _footBox = new Rectangle(destinationRectangle.X - footBoxWidth / 2 + footBoxXOffset, destinationRectangle.Y + footBoxYOffset, footBoxWidth, footBoxHeight);
            //Setter vanskelighetsgraden, sender ved combatLevel til spiller, grunn-hitpoints og grunn-skade
            setDifficulty(player1.combatLevel, 50, 12);
            setHpBar();

        }
        public WolfEnemy(Rectangle destinationRectangle, float scale, Player player1, Game game)
            : this(destinationRectangle, 0.6f, scale, player1, game)
        { }
        public override void attack1()
        {
            base.attack1();
            _attackDamageFrame = 2;
        }
        public override void attack2()
        {
            base.attack2();
            _attackDamageFrame = 2;
        }
    }
}
