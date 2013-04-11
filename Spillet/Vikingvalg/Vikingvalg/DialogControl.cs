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
        public Color unselectedAnswerColor = new Color(100, 100, 100, 255);
        protected int _lineHeight;
        protected int _maxTextWidth = 500;

        public StaticSprite npcNameBox;
        public StaticSprite npcTalkBox;
        public StaticSprite playerNameBox;
        public StaticSprite playerTalkBox;

        private int _textOffsetY = 7;
        private int _npcTalkBoxLines = 0;
        private int _playerTalkBoxLines = 0;
        private int _talkBoxLines = 0;

        public Vector2 npcNamePos;
        public Vector2 playerNamePos;

        public String npcSays;
        public Vector2 npcSaysPos = Vector2.Zero;
        public List<String> playerAnswers = new List<string>();
        private Vector2 _playerAnswerPos = Vector2.Zero;
        public List<Rectangle> playerAnswersBox = new List<Rectangle>();

        public DialogControl(NeutralNpc npc)
        {
            _npc = npc;

            _boxColor = new Color(0, 0, 0, 100);
            _lineHeight = (int)_npc.inGameLevel.spriteService.TextSize("H").Y+12;

            //Litt massive opprettelser for å legge boksene i en salgs mal. Source har ikke noe å si?
            npcTalkBox = new StaticSprite("box", new Rectangle(40, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y-_textOffsetY, _maxTextWidth + 20, _textOffsetY),
                new Rectangle(0, 0, 1, 1), 1001f, _boxColor);
            npcNameBox = new StaticSprite("box", new Rectangle(70, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y-(_lineHeight+_textOffsetY),
                (int)_npc.inGameLevel.spriteService.TextSize(_npc.npcName).X + 10, _lineHeight), new Rectangle(0, 0, 1, 1), 1001f, _boxColor);
            playerTalkBox = new StaticSprite("box",
                new Rectangle((int)_npc.inGameLevel.spriteService.GameWindowSize.X - 560, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y-_textOffsetY, _maxTextWidth + 20, _textOffsetY),
                new Rectangle(0, 0, 1, 1), 1001f, _boxColor);
            playerNameBox = new StaticSprite("box",
                new Rectangle((int)_npc.inGameLevel.spriteService.GameWindowSize.X - 105, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y - (_lineHeight+_textOffsetY), 35, _lineHeight),
                new Rectangle(0, 0, 1, 1), 1001f, _boxColor);

            npcSaysPos.X = npcTalkBox.DestinationX + 10;
            npcSaysPos.Y = npcTalkBox.DestinationY + _textOffsetY;

            npcNamePos = new Vector2(npcNameBox.DestinationX + 5, npcNameBox.DestinationY + 6);
            playerNamePos = new Vector2(playerNameBox.DestinationX + 5, playerNameBox.DestinationY + 6);
        }

        public void Update(IManageInput inputService)
        {

        }

        public void ChangeNpcDialog(String changeTo)
        {
            bool wasHighestBox = false;
            if (_npcTalkBoxLines == _talkBoxLines)
                wasHighestBox = true;

            npcSays = _npc.inGameLevel.spriteService.WrapText(changeTo, _maxTextWidth);

            _npcTalkBoxLines = 1;
            //funnet her: http://stackoverflow.com/questions/541954/how-would-you-count-occurences-of-a-string-within-a-string-c
            _npcTalkBoxLines += npcSays.Length - npcSays.Replace("\n", "").Length;

            //Teksten er størst
            if (_npcTalkBoxLines > _playerTalkBoxLines && _npcTalkBoxLines > _talkBoxLines)
            {
                ChangeHeight(_npcTalkBoxLines - _talkBoxLines, true);
                _talkBoxLines = _npcTalkBoxLines;
            }
            //Teksten var den største, og er mindre enn før
            else if (_npcTalkBoxLines < _talkBoxLines && wasHighestBox)
            {
                if (_npcTalkBoxLines > _playerTalkBoxLines)
                {
                    ChangeHeight(_talkBoxLines - _npcTalkBoxLines, false);
                    _talkBoxLines = _npcTalkBoxLines;
                }
                else
                {
                    ChangeHeight(_talkBoxLines - _playerTalkBoxLines, false);
                    _talkBoxLines = _playerTalkBoxLines;
                }
            }
        }
        public void AddPlayerAnswer(String toAdd)
        {
            toAdd = _npc.inGameLevel.spriteService.WrapText((playerAnswers.Count + 1) + ". " + toAdd, _maxTextWidth);

            int numOfLines = 1;
            //funnet her: http://stackoverflow.com/questions/541954/how-would-you-count-occurences-of-a-string-within-a-string-c
            numOfLines += toAdd.Length - toAdd.Replace("\n", "").Length;

            if (_playerTalkBoxLines + numOfLines > _talkBoxLines)
            {
                ChangeHeight(numOfLines, true);
            }

            playerAnswers.Add(toAdd);
            playerAnswersBox.Add(new Rectangle(playerTalkBox.DestinationX + 10, playerTalkBox.DestinationRectangle.Top + _textOffsetY + (_lineHeight*_playerTalkBoxLines), 500, _lineHeight));
            _playerTalkBoxLines += numOfLines;
        }

        private void ChangeHeight(int numOfLines, bool bigger)
        {
            int heightChange = _lineHeight * numOfLines;
            if (!bigger)
                heightChange *= -1;

            npcNameBox.DestinationY -= heightChange;
            npcNamePos.Y = npcNameBox.DestinationY + 6;
            npcTalkBox.DestinationY -= heightChange;
            npcTalkBox.DestinationHeight += heightChange;
            npcSaysPos.Y = npcTalkBox.DestinationY + _textOffsetY;
            playerNameBox.DestinationY -= heightChange;
            playerNamePos.Y = playerNameBox.DestinationY + 6;
            playerTalkBox.DestinationY -= heightChange;
            playerTalkBox.DestinationHeight += heightChange;
            foreach (String playerAnswer in playerAnswers)
            {
                playerAnswersBox[playerAnswers.IndexOf(playerAnswer)] = new Rectangle(playerAnswersBox[playerAnswers.IndexOf(playerAnswer)].X,
                    playerAnswersBox[playerAnswers.IndexOf(playerAnswer)].Y - heightChange, playerAnswersBox[playerAnswers.IndexOf(playerAnswer)].Width,
                    playerAnswersBox[playerAnswers.IndexOf(playerAnswer)].Height);
            }
        }

        public Vector2 GetPlayerAnswerPos(int index)
        {
            _playerAnswerPos.X = playerAnswersBox[index].X;
            _playerAnswerPos.Y = playerAnswersBox[index].Y;
            return _playerAnswerPos;
        }
    }
}