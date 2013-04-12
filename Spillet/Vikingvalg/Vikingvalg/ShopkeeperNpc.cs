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
            _talkingRangeBox.Y -= 15;

            npcName = "Merchant";
            dialogController = new DialogControl((NeutralNpc)this);

            InitialText();
        }
        public ShopkeeperNpc(String artName, Rectangle destinationRectangle, Player player, InGameLevel inGameLevel)
            : this(artName, destinationRectangle, new Rectangle(0, 0, destinationRectangle.Width, destinationRectangle.Height),
            new Color(255, 255, 255, 255), 0, Vector2.Zero, SpriteEffects.None, destinationRectangle.Bottom, player, inGameLevel)
        { }

        public override void InitialText()
        {
            dialogController.RemovePlayerAnswers();

            if (_player1.combatLevel < 10)
            {
                dialogController.ChangeNpcDialog("Hello, good sir! I have gathered some mighty fine raw materials from around the globe, "
                    + "and I'll give them to you for a good price! If you have enough experience, I'm sure you'll be able to make some great gear "
                    + "out of my goods. If you don't have enough gold or experience, I advice you to check out the gold mine or the hunting grounds.");
                dialogController.AddPlayerAnswer("*Buy some raw materials to make better equipment* [-" + 80*_player1.combatLevel +
                " experience, -" + 40*_player1.combatLevel + " gold]*", "combatLevelUp");
            }
            else
            {
                dialogController.ChangeNpcDialog("Oh my, how strong you have become! I have sold all my materials, and you have become the " +
                    "strongest person in Dragonview! Thinking about it, you have always been the strongest person in Dragonview..!");
                dialogController.AddPlayerAnswer("You are right. Maybe I have hit enough stuff for today.", "endConvo");
            }
            
            dialogController.AddPlayerAnswer("I'll be leaving now.", "endConvo");
        }

        public override void AnswerClicked(PlayerTextAnswer answer)
        {
            switch (answer.answerDesc)
            {
                case "combatLevelUp":
                    dialogController.RemovePlayerAnswers();
                    if (_player1.totalGold < 40 * _player1.combatLevel)
                    {
                        dialogController.ChangeNpcDialog("Aren't you a funny one! I'm never ever selling these for the price you are offering."
                            + " (you have insufficient gold)");
                        dialogController.AddPlayerAnswer("I better go mine some gold, then.", "endConvo");
                    }
                    else if (_player1.totalXP < 80 * _player1.combatLevel)
                    {
                        dialogController.ChangeNpcDialog("I am not comfortable selling my stuff to such an inexperienced guy as yourself. " +
                            " (you have insufficient experience)");
                        dialogController.AddPlayerAnswer("Maybe my reputation will grow if I go out in the forest and punch some wolves.",
                            "endConvo");
                    }
                    else
                    {
                        _player1.totalXP -= 80 * _player1.combatLevel;
                        _player1.totalGold -= 40 * _player1.combatLevel;
                        _player1.levelUp();
                        dialogController.ChangeNpcDialog("Mighty fine, sir, mighty fine! I thank you with all of my heart!");
                        dialogController.AddPlayerAnswer("You weren't lying, those raw materials was all i needed to craft some new" +
                            " equipment. Thank you!", "endConvo");
                    }
                    break;
                case "endConvo":
                    inConversation = false;
                    InitialText();
                    break;
            }
        }
    }
}
