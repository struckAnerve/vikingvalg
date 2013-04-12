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
            _talkingRangeBox.Y -= 20;

            npcName = "Oracle";
            dialogController = new DialogControl((NeutralNpc)this);

            InitialText();
        }
        public OracleNpc(String artName, Rectangle destinationRectangle, Player player, InGameLevel inGameLevel)
            : this(artName, destinationRectangle, new Rectangle(0, 0, destinationRectangle.Width, destinationRectangle.Height),
            new Color(255, 255, 255, 255), 0, Vector2.Zero, SpriteEffects.None, destinationRectangle.Bottom, player, inGameLevel)
        { }

        /// <summary>
        /// Teksten som skal være standard
        /// </summary>
        public override void InitialText()
        {
            dialogController.RemovePlayerAnswers();

            dialogController.ChangeNpcDialog("You smell");
            dialogController.AddPlayerAnswer("Go home, old lady", "insult");
        }

        //Utfører operasjoner utifra hvilken knapp som er trykket
        public override void AnswerClicked(PlayerTextAnswer answer)
        {
            switch (answer.answerDesc)
            {
                case "insult":
                    dialogController.RemovePlayerAnswers();
                    dialogController.ChangeNpcDialog("Why you little imbecile! You little bastard, you! YOU FROG EATING PIECE OF DROPPINGS, YOU! I'LL KILL YOUR SKINNY ASS!");
                    dialogController.AddPlayerAnswer("Maybe I should go home..", "endConvo");
                    break;
                case "endConvo":
                    inConversation = false;
                    InitialText();
                    break;
            }
        }
    }
}
