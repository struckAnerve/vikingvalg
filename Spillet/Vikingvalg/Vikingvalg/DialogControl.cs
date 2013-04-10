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
    class DialogControl
    {
        protected NeutralNpc _npc;

        protected Color _boxColor;

        public StaticSprite npcNameBox;
        public StaticSprite npcTalkBox;
        public StaticSprite playerNameBox;
        public StaticSprite playerTalkBox;

        public Vector2 npcNamePos;
        public Vector2 playerNamePos;

        public String npcSays;
        public Vector2 npcSaysPos;
        public List<String> playerAnswers = new List<string>();
        public List<Vector2> playerAnswersPos = new List<Vector2>();

        public DialogControl(NeutralNpc npc)
        {
            _npc = npc;

            _boxColor = new Color(0, 0, 0, 100);


            //Litt massive opprettelser for å legge boksene i en salgs mal
            npcTalkBox = new StaticSprite("box", new Rectangle(40, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y-30, 520, 30),
                new Rectangle(0, 0, 520, 30), 701f, _boxColor);
            npcNameBox = new StaticSprite("box", new Rectangle(70, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y-60, 30, 30),
                new Rectangle(0, 0, 30, 30), 701f, _boxColor);
            playerTalkBox = new StaticSprite("box",
                new Rectangle((int)_npc.inGameLevel.spriteService.GameWindowSize.X - 560, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y - 30, 520, 30),
                new Rectangle(0, 0, 520, 30), 701f, _boxColor);
            playerNameBox = new StaticSprite("box",
                new Rectangle((int)_npc.inGameLevel.spriteService.GameWindowSize.X - 105, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y - 60, 35, 30),
                new Rectangle(0, 0, 30, 30), 701f, _boxColor);

            npcNamePos = new Vector2(npcNameBox.DestinationX + 5, npcNameBox.DestinationY + 7);
            playerNamePos = new Vector2(playerNameBox.DestinationX + 5, playerNameBox.DestinationY + 7);
        }

        public void Update(IManageInput inputService)
        {

        }

        public void ChangeNpcDialog(String changeTo)
        {
            npcSays = changeTo;
            npcSaysPos.X = npcTalkBox.DestinationX + 10;
            npcSaysPos.Y = npcTalkBox.DestinationY + 7;
        }
        public void AddPlayerAnswer(String toAdd)
        {
            playerAnswers.Add(toAdd);
            playerAnswersPos.Add(new Vector2(playerTalkBox.DestinationX + 10, playerTalkBox.DestinationY + 7 + (20*playerAnswersPos.Count())));
        }
    }
}
